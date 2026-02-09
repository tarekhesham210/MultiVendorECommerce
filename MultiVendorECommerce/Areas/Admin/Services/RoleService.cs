using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.ViewModels;
using System.Security.Claims;

namespace MultiVendorECommerce.Areas.Admin.Services
{
    public class RoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDb _context;

        public RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDb context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<Dictionary<UserType, List<ApplicationRole>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return roles.GroupBy(r => r.UserType).ToDictionary(
                g => g.Key,
                g => g.ToList()
            );

        }
        public NewRoleVM GetRoleTypes()
        {
            var roleTypes = _roleManager.Roles.Select(r => r.UserType).Distinct().ToList();
            var newroleVm = new NewRoleVM
            {
                Types = roleTypes.Select(r => new SelectListItem
                {
                    Value = ((int)r).ToString(),
                    Text = r.ToString()
                }).Distinct().ToList()
            };
            return newroleVm;
        }
        public async Task CreateRoleAsync(NewRoleVM newRole)
        {
            if (string.IsNullOrEmpty(newRole.Name) || newRole.RoleType == 0)
            {
                throw new ValidationException("Role Name or role type is invalide");
            }
            var roleExists = await _roleManager.RoleExistsAsync(newRole.Name);
            if (roleExists)
            {
                throw new BusinessRuleException("Role already exists.");
            }
            var result = await _roleManager.CreateAsync(new ApplicationRole { Name = newRole.Name, UserType = newRole.RoleType });
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to create role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task DeleteRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ValidationException("Role ID cannot be null or empty.");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to delete role: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task<RolePermissionsVM> ManageRolePermission(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ValidationException("Invalide role id");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException("Role is not found");
            }
            var allPermessions = PermissionHelper.GetAllPermissions();
            var rolepermissions = await _roleManager.GetClaimsAsync(role);

            var model = new RolePermissionsVM
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Permissions = allPermessions.Select(p => new CheckBox
                {
                    Name = p!,
                    IsSelected = rolepermissions.Any(rp => rp.Type == "Permission" && rp.Value == p)
                }).ToList()
            };
            return model;
        }

        internal async Task UpdateRolePermissions(RolePermissionsVM model)
        {
            if (string.IsNullOrEmpty(model.RoleId) || string.IsNullOrEmpty(model.RoleId))
            {
                throw new ValidationException("Invalid role.");
            }
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }
            var selectedPermissions = model.Permissions.
                Where(p => p.IsSelected).Select(p => p.Name).ToHashSet();

            var currentPermissions = (await _roleManager.GetClaimsAsync(role))
                .Where(c => c.Type == PermissionClaim.Permission)
                .Select(pc => pc.Value).ToHashSet();

            var permissionsToAdd = selectedPermissions.Except(currentPermissions);
            var permissionsToRemove = currentPermissions.Except(selectedPermissions);

            using var transaction = await _context.Database.BeginTransactionAsync();

            // Remove unselected permissions
            foreach (var permission in permissionsToRemove)
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, new Claim(PermissionClaim.Permission, permission));
                if (!removeResult.Succeeded)
                {
                   await transaction.RollbackAsync();
                    throw new OperationFailedException("Failed to remove permission: " + permission);
                }
            }
            // Add selected permissions
            foreach (var permission in permissionsToAdd)
            {
                var addResult = await _roleManager.AddClaimAsync(role, new Claim(PermissionClaim.Permission, permission));
                if (!addResult.Succeeded)
                {
                 await transaction.RollbackAsync();
                    throw new OperationFailedException("Failed to add permission: " + permission);
                }
            }

            await transaction.CommitAsync();

        }
    }
}

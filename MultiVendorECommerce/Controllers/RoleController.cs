using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.ViewModels;
using System.Security.Claims;

namespace MultiVendorECommerce.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDb _context;
        public RoleController(RoleManager<ApplicationRole> roleManager, ApplicationDb applicationDb)
        {
            _roleManager = roleManager;
            this._context = applicationDb;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return RedirectToAction("Index");
            }
            var roleExists = await _roleManager.RoleExistsAsync(Name);
            if (roleExists)
            {
                ModelState.AddModelError("", "Role already exists.");
                return RedirectToAction("Index");
            }
            var result = await _roleManager.CreateAsync(new ApplicationRole { Name=Name,UserType=Shared.Enums.UserType.Internal});
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Index");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {

            if (!string.IsNullOrWhiteSpace(id))
            {
                var allPermessions = PermissionHelper.GetAllPermissions();
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                else
                {
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
                    return View(model);
                }
            }

            ModelState.AddModelError("", "Invalid role id");
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRolePermissions(RolePermissionsVM model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            if (role == null)
            {
                return NotFound();
            }
            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var selectedPermissions = model.Permissions.Where(p => p.IsSelected).Select(p => p.Name).ToList();
            using var transaction = await _context.Database.BeginTransactionAsync();
            // Remove unselected permissions
            try
            {
                foreach (var claim in roleClaims.Where(rc => rc.Type == PermissionClaim.Permission && !selectedPermissions.Contains(rc.Value)))
                {
                    var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                    if (!removeResult.Succeeded)
                    {
                        throw new Exception("Failed to remove claim: " + claim.Value);
                    }
                }
                // Add selected permissions
                foreach (var permission in selectedPermissions.Where(sp => !roleClaims.Any(rc => rc.Type == PermissionClaim.Permission && rc.Value == sp)))
                {
                    var addResult = await _roleManager.AddClaimAsync(role, new System.Security.Claims.Claim(PermissionClaim.Permission, permission));
                    if (!addResult.Succeeded)
                    {
                        throw new Exception("Failed to add claim: " + permission);
                    }
                }
                await transaction.CommitAsync();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                ModelState.AddModelError("", "An error occurred while updating role permissions: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("", "Invalid role id");
                return RedirectToAction("Index");
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role not found");
                return RedirectToAction("Index");
            }
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Index");
            }
        }
    }
}

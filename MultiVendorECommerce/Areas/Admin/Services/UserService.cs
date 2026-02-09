using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.ViewModels;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Admin.Services
{
    public class UserService
    {
        private readonly ApplicationDb _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDb context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public   AddInternalUserVM AddInternalUser() {           
            var allowedRoles =  _roleManager.Roles.Where(r=>r.UserType==UserType.Internal);
            var userVM = new AddInternalUserVM
            {
                roles = allowedRoles.Select(r => new CheckBox
                {
                    Name = r.Name,
                    IsSelected = false
                }).ToList()
            };
            return userVM;
        }
        public async Task CreateInternalUserAsync(AddInternalUserVM userVM)
        {
            if (await IsEmailExist(userVM.Email))
                throw new ConflictException("Email already exists.");
            if (string.IsNullOrWhiteSpace(userVM.Password) || userVM.Password.Length < 8)
                throw new ValidationException("Password must be at least 8 characters.");
            var selectedRoles = userVM.roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();

            if (SelectedRolesHasInvalidRole(selectedRoles,UserType.Internal))
            {
                throw new BusinessRuleException("One or more selected roles are not allowed for Internal users.");
            }


            var user = new ApplicationUser
            {
                UserName = userVM.Email,
                Email = userVM.Email,
                UserType = UserType.Internal
            };
            var result = await _userManager.CreateAsync(user, userVM.Password);
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            result = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!result.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                throw new OperationFailedException("Failed to add role to user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

     

        public  List<InternalUserVM> GetInternalUsers()
        {
            return  _userManager.Users.Where(u => u.UserType == UserType.Internal).Select(u=>new InternalUserVM
            {
                Email = u.Email,
                Id = u.Id,
                UserName = u.UserName,
                Userstatus = u.LockoutEnd == null ? Userstatus.Active : Userstatus.Suspended
            }).ToList(); 
            
        }

        public async Task<UsersInRoleVM> GetUsersInRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ValidationException("Role ID cannot be null or empty.");
            }
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                throw new NotFoundException("Role not found.");
            }
           
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            var usersInRoleVM = new UsersInRoleVM
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Users = usersInRole.Select(u => new InternalUserVM
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    Userstatus = u.LockoutEnd == null ? Userstatus.Active : Userstatus.Suspended
                }).ToList()
            };
            return usersInRoleVM;
        }
        public async Task<UserRolesVM> GetUserRoles(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ValidationException("User ID cannot be null or empty.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            var allowedRoles = _roleManager.Roles.Where(r=>r.UserType==user.UserType);
            var roles = new UserRolesVM
            {
                Id = user.Id,
                Email = user.Email,
                Roles = allowedRoles.Select(r => new CheckBox
                {
                    Name = r.Name,
                    IsSelected = userRoles.Contains(r.Name)
                }).ToList()
            };
            return roles;
        }
        public async Task UpdateUserRoles(UserRolesVM model)
        {
            if (string.IsNullOrWhiteSpace(model.Id))
            {
                throw new ValidationException("User ID cannot be null or empty.");
            }
            var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();

            if (SelectedRolesHasInvalidRole(selectedRoles,UserType.Internal))
            {
                throw new BusinessRuleException("One or more selected roles are not exist for Internal users.");
            }
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            using var transaction = await _context.Database.BeginTransactionAsync();
            var addResult = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
            if (!addResult.Succeeded)
            {
                await transaction.RollbackAsync();
                throw new OperationFailedException("Failed to update user roles: " + string.Join(", ", addResult.Errors.Select(e => e.Description)));
            }
            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
            if (!removeResult.Succeeded)
            {
                await transaction.RollbackAsync();
                throw new OperationFailedException("Failed to update user roles: " + string.Join(", ", addResult.Errors.Select(e => e.Description)));

            }
            await transaction.CommitAsync();

        }

        public async Task ActivateUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ValidationException("User ID cannot be null or empty.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
          var result= await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to activate user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        public async Task SuspendUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ValidationException("UserId cannot be null or empty.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
          var result= await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to suspend user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        public async Task<ApplicationUser> GetUserById(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                throw new ValidationException("UserId cannot be null or empty.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            return user;
        }

        private async Task<bool> IsEmailExist(string email)
        {
            bool isEmailExist = false;
            var user= await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                isEmailExist = true;
            }
            return isEmailExist;
        }

        private async Task<bool> RoleIsExist(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
     
        private bool SelectedRolesHasInvalidRole(List<string> selectedRoles, UserType userType)
        {
            var allowedRoles = _roleManager.Roles.Where(r => r.UserType == userType).Select(r => r.Name);
            var invalidRoles = selectedRoles.Except(allowedRoles).ToList();
            return invalidRoles.Any();
        }


    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Services;
using MultiVendorECommerce.ViewModels;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Users.ViewInternal)]

        public async Task<IActionResult> GetInternalUsers(int page=1)
        {
            var users =await _userService.GetInternalUsers(page);
            return View(users);
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Users.ViewInRole)]

        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            
            var users = await _userService.GetUsersInRoleAsync(roleName);
            return View(nameof(GetUsersInRole),users);
        }
 
        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Users.AddInternal)]

        public IActionResult AddInternalUser()
        {
            var userVM =  _userService.AddInternalUser();
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Users.AddInternal)]

        public async Task<IActionResult> AddInternalUser(AddInternalUserVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            HttpContext.Items["CurrentModel"] = userVM;
            await _userService.CreateInternalUserAsync(userVM);
            return RedirectToAction(nameof(GetInternalUsers));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Users.UpdateUserRoles)]

        public async Task<IActionResult> UpdateUserRoles(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound("User ID cannot be null or empty.");
            }

            var roles = await _userService.GetUserRoles(id);

            return View(roles);
        }
        [HttpPost]
        [Authorize(Policy = Permissions.Admin.Users.UpdateUserRoles)]

        public async Task<IActionResult> UpdateUserRoles(UserRolesVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            HttpContext.Items["CurrentModel"] = model;

            await _userService.UpdateUserRoles(model);
            return RedirectToAction(nameof(GetInternalUsers));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Users.Suspend)]

        public async Task<IActionResult> SuspendUser(string id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetInternalUsers));
            }
            await _userService.SuspendUser(id);
            return RedirectToAction(nameof(GetInternalUsers));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy =Permissions.Admin.Users.Activate)]
        public async Task<IActionResult> ActivateUser(string id)
        {   
            if(string.IsNullOrWhiteSpace(id))
            {
                ModelState.AddModelError("", "Invalid user id.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(GetInternalUsers));
            }
            await _userService.ActivateUser(id);

            return RedirectToAction(nameof(GetInternalUsers));
        }

    }
}

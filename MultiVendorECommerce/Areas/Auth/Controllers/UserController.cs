using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Services.Interfaces;
using MultiVendorECommerce.ViewModels;
using System.Security.Claims;

namespace MultiVendorECommerce.Areas.Auth.Controllers
{
    [Area("Auth")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDb _context;
        private readonly IImageService _imageService;

        public UserController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDb context,
            IImageService imageService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _imageService = imageService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var currentUserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId);
            if (user is null)
            {
                return NotFound();
            }
            var EditUser = new EditUserDataVM
            {
                Id=user.Id,
                Email = user.Email,
                currentImage = user.Image
            };
            return View(EditUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserDataVM model)
        {
            if (ModelState.IsValid)
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var user = await _userManager.FindByIdAsync(currentUserId);
                if (user == null)
                {
                    return NotFound();
                }
                user.Email = model.Email;
                user.UserName = model.Email.Split("@")[0];
                if (model.Image != null && model.Image.Length != 0)
                {

                    var result = await _imageService.SaveImageAsync(model.Image,ImageSettings.UsersImagesFolder);
                    _imageService.DeleteImage(user.Image,ImageSettings.UsersImagesFolder);
                    user.Image = result;
                }
                    IdentityResult identityResult = await _userManager.UpdateAsync(user);
                    if (identityResult.Succeeded)
                    {
                        switch(user.UserType)
                        {
                            case UserType.Internal:
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                            case UserType.Vendor:
                                return RedirectToAction("Index", "Home", new { area = "Vendor" });
                            case UserType.Customer:
                                return RedirectToAction("Index", "Home", new { area = "Customer" });
                        }
                    }

                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("" ,error.Description);
                    }
                    return View(model);
              
            }
            return View(model);
        }
    }
}

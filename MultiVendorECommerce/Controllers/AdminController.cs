using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Auth.Services;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Services;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Services.Interfaces;
using PermissionBasedAuz.ViewModels;
using System.Threading.Tasks;

namespace PermissionBasedAuz.Controllers
{
   // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDb _context;
        private readonly VendorService _vendorService;
        private readonly AccountService _accountService;
        private readonly IImageService _imageService;

        public AdminController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager, ApplicationDb context, IImageService imageService, VendorService vendorService, AccountService accountService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _imageService = imageService;
            _vendorService = vendorService;
            _accountService = accountService;
        }



        public IActionResult GetUsers()
        {
            var users = _userManager.Users.Where(u=>u.UserType==UserType.Internal).ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult AddInternalUser()
        {
            return View();
        }

        public IActionResult GetCustomers()
        {
            var customers = _context.Customers.ToList();
            return View(customers);
        }
        [HttpGet]
        public async Task<IActionResult> GetPendingVendors()
        {
            var vendors = await _vendorService.GetPendingVendorsAsync();
            return View(vendors);
        }
        //[HttpGet]
        //public async Task<IActionResult> UserRoles(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var allRoles = _roleManager.Roles.ToList();
        //    var roles = new UserRolesVM
        //    {
        //        Id = user.Id,
        //        Email = user.Email,
        //        Roles = allRoles.Select(r => new CheckBox
        //        {
        //            Name = r.Name,
        //            IsSelected = userRoles.Contains(r.Name)
        //        }).ToList()
        //    };

        //    return View(roles);
        //}
        //[HttpPost]
        //public async Task<IActionResult> UpdateUserRoles(UserRolesVM model)
        //{

        //    var user = await _userManager.FindByIdAsync(model.Id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var selectedRoles = model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToList();

        //    using var transaction = await _context.Database.BeginTransactionAsync();
        //    var addResult = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));
        //    if (!addResult.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Failed to add roles");
        //        await transaction.RollbackAsync();
        //        return View(model);
        //    }
        //    var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));
        //    if (!removeResult.Succeeded)
        //    {
        //        ModelState.AddModelError("", "Failed to remove roles");
        //        await transaction.RollbackAsync();
        //        return View(model);
        //    }
        //    await transaction.CommitAsync();
        //    return RedirectToAction("Index");

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuspendUser(string id)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                return RedirectToAction("Index");
            }
            try
            {
                await _accountService.SuspendUser(id);
                return RedirectToAction("Index");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction("Index");
            }

        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveVendor(int id, string userId)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                return RedirectToAction(nameof(GetPendingVendors));
            }

            try
            {
                await _vendorService.ApproveVendor(id);
                await _accountService.ActivateUser(userId);
                return RedirectToAction(nameof(GetPendingVendors));

            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetPendingVendors));
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetPendingVendors));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuspendVendor(int id,string userId)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data.");
                return RedirectToAction(nameof(GetApprovedVendors));
            }
            try
            {
                await _vendorService.SuspendVendor(id);
                await _accountService.SuspendUser(userId);
                return RedirectToAction(nameof(GetApprovedVendors));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetApprovedVendors));
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return RedirectToAction(nameof(GetApprovedVendors));
            }
        }
      
        [HttpGet]
        public async Task<IActionResult> GetApprovedVendors()
        {
           var approvedVendors= await _vendorService.GetApprovedVendorsAsync();
            return View(approvedVendors);
        }


    }
}

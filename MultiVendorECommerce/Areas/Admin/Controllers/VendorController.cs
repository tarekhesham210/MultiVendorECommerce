using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Constants;

namespace MultiVendorECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VendorController : Controller
    {
        private readonly VendorService _vendorService;

        public VendorController(VendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpGet]
        [Authorize(Policy =Permissions.Admin.Vendor.ViewAprroved)]
        public async Task<IActionResult> ApprovedVendors(int page=1)
        {
            var vendors = await _vendorService.GetApprovedVendorsAsync(page);
            return View(vendors);
        }
        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Vendor.ViewPending)]

        public async Task<IActionResult> PendingVendors(int page=1)
        {
            var vendors = await _vendorService.GetPendingVendorsAsync(page);
            return View(vendors);
        }
        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Vendor.ViewRejected)]

        public async Task<IActionResult> RejectedVendors(int page=1)
        {
            var rejectedVendors = await _vendorService.GetRejectedVendorsAsync(page);
            return View(rejectedVendors);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Vendor.Reject)]
        public async Task<IActionResult> RejectVendor(int id)
        {
            if (id <= 0)
                ModelState.AddModelError("", "Invalid data.");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(PendingVendors));
            }
            await _vendorService.RejectVendor(id);
            return RedirectToAction(nameof(PendingVendors));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Vendor.Suspend)]
        public async Task<IActionResult> SuspendVendor(int id)
        {
            if (id <= 0)
                ModelState.AddModelError("", "Invalid data.");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(ApprovedVendors));
            }
            await _vendorService.SuspendVendor(id);
            return RedirectToAction(nameof(ApprovedVendors));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Vendor.Aprrove)]
        public async Task<IActionResult> ApproveVendor(int id)
        {
            if (id <= 0)
                ModelState.AddModelError("", "Invalid data.");
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(ApprovedVendors));
            }
            await _vendorService.ApproveVendor(id);
            return RedirectToAction(nameof(ApprovedVendors));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Vendor.ViewDetails)]
        public async Task<IActionResult> VendorDetails(int id)
        {
            var vendor = await _vendorService.GetVendorDetails(id);         
            return View(vendor);
        }
    }
}

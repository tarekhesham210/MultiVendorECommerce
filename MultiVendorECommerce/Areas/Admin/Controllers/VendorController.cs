using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
using System.Threading.Tasks;

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
        public async Task<IActionResult> ApprovedVendors()
        {
            var vendors = await _vendorService.GetApprovedVendorsAsync();
            return View(vendors);
        }
        [HttpGet]
        public async Task<IActionResult> PendingVendors()
        {
            var vendors = await _vendorService.GetPendingVendorsAsync();
            return View(vendors);
        }
        [HttpGet]
        public async Task<IActionResult> RejectedVendors()
        {
            var rejectedVendors = await _vendorService.GetRejectedVendorsAsync();
            return View(rejectedVendors);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> VendorDetails(int id)
        {
            var vendor = await _vendorService.GetVendorDetails(id);         
            return View(vendor);
        }
    }
}

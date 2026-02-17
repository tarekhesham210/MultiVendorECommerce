using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Constants;

namespace MultiVendorECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        [Authorize(Policy =Permissions.Admin.Customer.ViewAll)]
        public async Task<IActionResult> Index(int page=1)
        {
            var customers =await _customerService.GetAllCustomers(page);
            return View(customers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Customer.Block)]
        public async Task<IActionResult> BlockCustomer(int id)
        {
            if(id<=0)
                ModelState.AddModelError("", "Invalid data.");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            await _customerService.BlockCustomer(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Customer.Activate)]

        public async Task<IActionResult> ActivateCustomer(int id)
        {
            if(id<=0)
                ModelState.AddModelError("", "Invalid data.");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            await _customerService.ActivateCustomer(id);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Customer.ViewDetails)]

        public async Task<IActionResult> CustomerDetails(int id)
        {
            var customerDetails =await _customerService.GetCustomerDetails(id);
            return View(customerDetails);
        }
    }
}

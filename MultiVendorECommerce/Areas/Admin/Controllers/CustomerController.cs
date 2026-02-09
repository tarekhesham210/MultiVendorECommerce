using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index()
        {
            var customers =await _customerService.GetAllCustomers();
            return View(customers);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> CustomerDetails(int id)
        {
            var customerDetails =await _customerService.GetCustomerDetails(id);
            return View(customerDetails);
        }
    }
}

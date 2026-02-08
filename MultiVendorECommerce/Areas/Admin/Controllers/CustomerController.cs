using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Admin.Services;
using System.Threading.Tasks;

namespace PermissionBasedAuz.Areas.Admin.Controllers
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
    }
}

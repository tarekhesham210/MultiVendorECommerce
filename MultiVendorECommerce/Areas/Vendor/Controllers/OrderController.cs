using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Vendor.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    [Authorize(Roles ="Vendor")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> NewOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Auth" });
            }
            var orders = await _orderService.GetVendornewOrdersAsync(userId);

            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int orderItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { area = "Auth" });
            }
            await _orderService.ConfirmOrder(userId, orderItemId);
            return RedirectToAction(nameof(NewOrders));
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Customer.Services;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using System.Security.Claims;

namespace MultiVendorECommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var checkout = await _orderService.PlaceOrderAsync(userId);
            return View(checkout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(CheckoutVM checkout)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Checkout));
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orderId = await _orderService.PlaceOrderAsync(userId, checkout);
            return RedirectToAction("Index", controllerName: "Home");
        }
        [HttpGet]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _orderService.GetOrderDetails(id);
            return View(order);
        }
        [HttpGet]
        public async Task<IActionResult> MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = await _orderService.GetCustomerOrders(userId);
            return View(orders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            await _orderService.CancelOrder(userId, orderId);
            return RedirectToAction(nameof(MyOrders));
        }
    }
}

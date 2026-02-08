using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Customer.Services;
using PermissionBasedAuz.Areas.Customer.ViewModels;
using System.Security.Claims;

namespace PermissionBasedAuz.Areas.Customer.Controllers
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
    }
}

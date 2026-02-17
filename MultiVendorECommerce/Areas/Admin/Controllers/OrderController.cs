using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
using MultiVendorECommerce.Constants;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        [Authorize(Policy =Permissions.Admin.Order.ViewPending)]
        public async Task<IActionResult> PendingOrders()
        {
            var pendingOrders =await _orderService.GetPendingOrdersAsync();
            return View(pendingOrders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Order.Confirm)]

        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            await _orderService.ConfirmOrder(orderId);
            return RedirectToAction(nameof(PendingOrders));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = Permissions.Admin.Order.Reject)]

        public async Task<IActionResult> RejectOrder(int orderId)
        {
            await _orderService.RejectOrder(orderId);
            return RedirectToAction(nameof(PendingOrders));
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Admin.Order.ViewNewDetails)]

        public async Task<IActionResult> NewOrderDetails(int orderId)
        {
            var order =await _orderService.GetNewOrderDetails(orderId);
            return View(order);
        }
    }
}

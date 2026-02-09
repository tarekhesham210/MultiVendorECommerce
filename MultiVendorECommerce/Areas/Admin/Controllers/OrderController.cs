using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Admin.Services;
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

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> PendingOrders()
        {
            var pendingOrders =await _orderService.GetPendingOrdersAsync();
            return View(pendingOrders);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            await _orderService.ConfirmOrder(orderId);
            return RedirectToAction(nameof(PendingOrders));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectOrder(int orderId)
        {
            await _orderService.RejectOrder(orderId);
            return RedirectToAction(nameof(PendingOrders));
        }

        [HttpGet]
        public async Task<IActionResult> NewOrderDetails(int orderId)
        {
            var order =await _orderService.GetNewOrderDetails(orderId);
            return View(order);
        }
    }
}

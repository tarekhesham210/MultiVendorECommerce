using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Customer.Services;
using System.Security.Claims;

namespace PermissionBasedAuz.Areas.Customer.Components
{
    public class CartCountViewComponent : ViewComponent
    {
        private readonly CartService _cartService; // أو الـ DbContext مباشرة

        public CartCountViewComponent(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int count = 0;

            if (userId != null)
            {
                count = await _cartService.GetCartItemsCountAsync(userId);
            }

            return View("_CartCountPartial", count); 
        }
    }
}

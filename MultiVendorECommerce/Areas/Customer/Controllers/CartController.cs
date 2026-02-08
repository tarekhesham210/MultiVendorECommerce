using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Customer.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PermissionBasedAuz.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Customer")]
        public async Task<IActionResult> AddToCart(int VariantId,int Quantity)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            var cartCount = await _cartService.AddToCustomerCart(userId, VariantId, Quantity);
           
            if (cartCount == 0) 
            { 
                return Json(new
                {
                    success = false,
                    message = "Count of product in cart must be coverd by stock items"
                });
            }
            return Json(new
                {
                    success = true,
                    message = "Product Added To Cart Successfully",
                    cartCount
                }); 
         

        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart= await _cartService.GetCartDetailsAsync(userId);
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int VariantId,int Quantity)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var updated=await _cartService.UpdateItemQuantityAsync(userId,Quantity ,VariantId);
            
            return Json(new {success= updated });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int VariantId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var deleted=await _cartService.RemoveItemAsync(userId, VariantId);
            return Json(new {success= deleted });
        }
    }
}

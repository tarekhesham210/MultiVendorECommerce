using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Areas.Customer.ViewModels;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Shared.Services.Interfaces;
using System.Threading.Tasks;

namespace PermissionBasedAuz.Shared.Services.Implementation
{
    public class CartQueryService:ICartQueryService
    {
        private readonly ApplicationDb _context;

        public CartQueryService(ApplicationDb context)
        {
            _context = context;
        }

        public async Task<CartDetailsVM> GetCartDetailsAsync(int cartId)
        {
            var cart=await _context.CartItems.Where(i=>i.CartId==cartId).Select(x => new CartItemDetailsVM 
            {
               VariantId=x.ProductVariantId,
               Price=x.PriceAtTime,
               Quantity=x.Quantity,
               VariantName=x.Variant.Product.Name,
               ImageUrl=x.Variant.Image.ImageUrl ?? x.Variant.Product.Images.First().ImageUrl,
               
            }).ToListAsync();

            return new CartDetailsVM { cartItems=cart};
        }
    }
}

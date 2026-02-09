using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
{
    public class CartItemRepo : ICartItemRepository
    {
        private readonly ApplicationDb _context;

        public CartItemRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            await _context.CartItems.AddAsync(item);
        }

        public void RemoveCartItem(CartItem item)
        {
           _context.CartItems.Remove(item);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

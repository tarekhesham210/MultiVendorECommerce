using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
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

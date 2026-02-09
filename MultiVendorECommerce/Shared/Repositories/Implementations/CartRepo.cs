using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
{
    public class CartRepo :ICartRepository
    {
        private readonly ApplicationDb _context;

        public CartRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddCart(Cart cart)
        {
           await _context.Carts.AddAsync(cart);
        }

        public async Task<Cart?> GetCartByIdAsync(int id) 
        {
            return await _context.Carts.Include(c=>c.Items).FirstOrDefaultAsync(c=>c.Id==id);
        }
        public async Task<Cart?> GetCartByCustomerIdAsync(int id) 
        {
            return await _context.Carts.Include(c=>c.Items).FirstOrDefaultAsync(c=>c.CustomerId==id);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void DeleteCart(Cart cart)
        {
            _context.Carts.Remove(cart);
        }
    }
}

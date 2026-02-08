using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class OrderRepo : IOrderRepository
    {
        private readonly ApplicationDb _context;

        public OrderRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

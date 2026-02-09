using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
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

        public async Task<Order?> GetOrderByCustomerIdAsync(int customerId,int orderId)
        {
            return await _context.Orders.Include(o=>o.Items).ThenInclude(i=>i.Variant).FirstOrDefaultAsync(o => o.CustomerId == customerId && o.Id==orderId);
        }

        public async Task<IEnumerable<Order>> GetPendingOrdersAsync()
        {
            return await _context.Orders.Where(o => o.Status == OrderStatus.Pending).ToListAsync();

        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

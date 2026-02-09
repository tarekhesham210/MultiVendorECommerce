using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
{
    public class OrderItemRepo : IOrderItemRepository
    {
        private readonly ApplicationDb _context;

        public OrderItemRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddOrderItemAsync(OrderItem orderItem)
        {
           await _context.OrderItems.AddAsync(orderItem);
        }

        public Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderItem>> GetVendorPendingOrderItemsAsync(int vendorId)
        {
            return await _context.OrderItems
                .Where(oi => oi.VendorId == vendorId && oi.Order.Status == OrderStatus.Pending&& !oi.VendorConfirmation)
                .Include(i=>i.Variant)
                .ThenInclude(v=>v.Product).ToListAsync();
        }

        public void RemoveOrderItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<OrderItem?> GetOrderItemsByIdAsync(int orderItemId)
        {
            return await _context.OrderItems.FindAsync(orderItemId);
        }
    }
}

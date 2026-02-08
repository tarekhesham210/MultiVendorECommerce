using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
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

        public void RemoveOrderItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }

        public Task SavAsync()
        {
            throw new NotImplementedException();
        }
    }
}

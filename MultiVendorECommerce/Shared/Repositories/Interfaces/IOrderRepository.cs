using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface IOrderRepository
    {
         Task AddOrderAsync(Order order);
         Task<Order> GetOrderByIdAsync(int orderId);
         Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId);
         Task SaveAsync();
    }
}

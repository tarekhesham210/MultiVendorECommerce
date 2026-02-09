using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IOrderRepository
    {
         Task AddOrderAsync(Order order);
         Task<Order?> GetOrderByIdAsync(int orderId);
        Task<Order?> GetOrderByCustomerIdAsync(int customerId, int orderId);
        Task<IEnumerable<Order>> GetPendingOrdersAsync();
         Task SaveAsync();
    }
}

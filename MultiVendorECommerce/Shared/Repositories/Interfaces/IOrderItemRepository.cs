using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task<OrderItem?> GetOrderItemsByIdAsync(int orderItemId);
        Task SaveAsync();
        Task AddOrderItemAsync(OrderItem orderItem);
        void RemoveOrderItem(OrderItem orderItem);
         Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);
        Task<IEnumerable<OrderItem>> GetVendorPendingOrderItemsAsync(int vendorId);
    }
}

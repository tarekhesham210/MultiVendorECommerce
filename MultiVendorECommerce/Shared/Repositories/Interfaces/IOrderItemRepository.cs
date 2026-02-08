using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        Task SavAsync();
        Task AddOrderItemAsync(OrderItem orderItem);
        void RemoveOrderItem(OrderItem orderItem);
         Task<List<OrderItem>> GetOrderItemsByOrderIdAsync(int orderId);

    }
}

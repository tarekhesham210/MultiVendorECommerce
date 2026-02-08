using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task SaveAsync();
        Task AddCartItemAsync(CartItem item);
        void RemoveCartItem(CartItem item);
    }
}

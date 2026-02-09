using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface ICartItemRepository
    {
        Task SaveAsync();
        Task AddCartItemAsync(CartItem item);
        void RemoveCartItem(CartItem item);
    }
}

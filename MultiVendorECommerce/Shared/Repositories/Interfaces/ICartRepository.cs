using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task SaveAsync();
        Task AddCart(Cart cart);
        Task<Cart?> GetCartByIdAsync(int id);
        Task<Cart?> GetCartByCustomerIdAsync(int id);
        void DeleteCart(Cart cart);


    }
}
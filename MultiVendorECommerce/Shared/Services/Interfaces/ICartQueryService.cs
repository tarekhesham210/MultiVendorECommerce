using PermissionBasedAuz.Areas.Customer.ViewModels;

namespace PermissionBasedAuz.Shared.Services.Interfaces
{
    public interface ICartQueryService
    {
        Task<CartDetailsVM> GetCartDetailsAsync(int cartId);
    }
}

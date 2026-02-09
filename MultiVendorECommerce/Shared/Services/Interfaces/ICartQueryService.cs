using MultiVendorECommerce.Areas.Customer.ViewModels;

namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface ICartQueryService
    {
        Task<CartDetailsVM> GetCartDetailsAsync(int cartId);
    }
}

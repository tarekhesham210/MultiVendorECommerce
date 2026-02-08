using PermissionBasedAuz.Areas.Customer.ViewModels;
using PermissionBasedAuz.Areas.Vendor.ViewModels;

namespace PermissionBasedAuz.Shared.Services.Interfaces
{
    public interface IProductQueryService
    {
        Task<IEnumerable<ProductVariantCardVM>> GetProductsBestSellersAsync(int count);
        Task<EditProductVM?> GetProductForEditAsync(int productId);
        Task<IEnumerable<ProductWithHotOffer>> GetHotOfferVariantAsync(int count);
        Task<IEnumerable<ProductVariantCardVM>> GetProductsByCategoryIdAsync(int id);
        Task<ProductDetailsVM> GetProductDetailes(int id,int? variantId);
       // EditProductVM? GetProductByIdWithVariants(int id);
    }
}

using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Areas.Vendor.ViewModels;

namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface IProductQueryService
    {
        Task<IEnumerable<ProductVariantCardVM>> LiveSearchProductsAsync(string term);
        Task<(IEnumerable<ProductVariantCardVM>, int TotalCount)> GetProductsBySearchTermAsync
                    (string searchTerm, string sortBy ,int pageSize = 10, int pageNumber = 1);
            Task<IEnumerable<ProductVariantCardVM>> GetProductsBestSellersAsync(int count);
        Task<EditProductVM?> GetProductForEditAsync(int productId);
        Task<IEnumerable<ProductWithHotOffer>> GetHotOfferVariantAsync(int count);
        Task<IEnumerable<ProductVariantCardVM>> GetProductsByCategoryIdAsync(int id);
        Task<ProductDetailsVM> GetProductDetailes(int id,int? variantId);
       // EditProductVM? GetProductByIdWithVariants(int id);
    }
}

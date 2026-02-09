using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IProductVariantRepository
    {
        Task<ProductVariant?> GetProductByIdAsync(int id);
        Task<IEnumerable<ProductVariant>> GetProductsByVendorIdAsync(int id);
        Task<ProductVariant?> GetByIdAsync(int id);
        IQueryable<ProductVariant> GetAll();
        Task<IReadOnlyList<ProductVariant>> GetPagedAsync(
            int page,
            int pageSize);

        Task<IReadOnlyList<ProductVariant>> GetByCategoryAsync(
            int categoryId,
            int page,
            int pageSize);

        Task<IReadOnlyList<ProductVariant>> GetByVendorAsync(
            int vendorId,
            int page,
            int pageSize);

        Task<IReadOnlyList<ProductVariant>> GetTopBySalesAsync(int count);
        Task AddProductAsync(ProductVariant product);
        Task<bool> DeleteProductAsync(ProductVariant product);
        Task SaveAsync();
        Task<bool> IsProductNameExistWithVendorIdAsync(string pName, int vId, int? pId = null);
    }

}

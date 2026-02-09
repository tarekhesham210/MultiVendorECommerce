using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product ?> GetProductByIdAsync(int id);
        Task<IEnumerable<Product>> GetProductsByVendorIdAsync(int id);
        Task<Product?> GetByIdAsync(int id);
        IQueryable<Product> GetAll();
        Task<IReadOnlyList<Product>> GetPagedAsync(
            int page,
            int pageSize);

        Task<IReadOnlyList<Product>> GetByCategoryAsync(
            int categoryId,
            int page,
            int pageSize);

        Task<IReadOnlyList<Product>> GetByVendorAsync(
            int vendorId,
            int page,
            int pageSize);

        Task<IReadOnlyList<Product>> GetTopBySalesAsync(int count); Task AddProductAsync(Product product);
        Task<bool> DeleteProductAsync(Product product);
        Task SaveAsync();
        Task<bool> IsProductNameExistWithVendorIdAsync(string pName, int vId,int? pId=null);
    }
}

using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class ProductVariantRepo : IProductVariantRepository
    {
        private readonly ApplicationDb _context;

        public ProductVariantRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddProductAsync(ProductVariant product)
        {
           await _context.ProductVariants.AddAsync(product);
        }

        public async Task<bool> DeleteProductAsync(ProductVariant product)
        {
            _context.ProductVariants.Remove(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public IQueryable<ProductVariant> GetAll()
        {
            return _context.ProductVariants.AsNoTracking();
        }

        public Task<IReadOnlyList<ProductVariant>> GetByCategoryAsync(int categoryId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ProductVariant?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ProductVariant>> GetByVendorAsync(int vendorId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ProductVariant>> GetPagedAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductVariant?> GetProductByIdAsync(int id)
        {
           return await _context.ProductVariants.Include(v=>v.Product).FirstOrDefaultAsync(v=>v.Id==id);
        }

        public Task<IEnumerable<ProductVariant>> GetProductsByVendorIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ProductVariant>> GetTopBySalesAsync(int count)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsProductNameExistWithVendorIdAsync(string pName, int vId, int? pId = null)
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}

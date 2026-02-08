using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class ProductRepo : IProductRepository
    {
        private readonly ApplicationDb _context;

        public ProductRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
           await _context.Products.AddAsync(product);
         
        }

        public async Task<bool> DeleteProductAsync(Product product)
        {
            _context.Products.Remove(product);
            return (await _context.SaveChangesAsync()) >0;
        }

       
        public async Task<bool> IsProductNameExistWithVendorIdAsync(string pName,int vId,int? pId=null)
        {
            return await _context.Products
                .AnyAsync(p => p.VendorId == vId && p.Name == pName &&(!pId.HasValue||p.Id!=pId.Value));
                
        }
        public async Task<IEnumerable<Product>> GetProductsByVendorIdAsync(int id)
        {
            return await _context.Products.Include(p=>p.Category)
                .Where(p => p.VendorId == id)
                .ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var result= await _context.Products
                .Include(p => p.Images)             
                .Include(p=>p.AttributeValues)
                .Include(p=>p.Variants).ThenInclude(v=>v.CurrentOffer)
                .Include(p=>p.Variants).ThenInclude(v=>v.VariantValues)
                .FirstOrDefaultAsync(p => p.Id == id);
            return result;
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.Images).SingleOrDefaultAsync(p=>p.Id==id);
        }

        public Task<IReadOnlyList<Product>> GetPagedAsync(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetByCategoryAsync(int categoryId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetByVendorAsync(int vendorId, int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Product>> GetTopBySalesAsync(int count)
        {
            return await _context.Products
                .AsNoTracking()
                .OrderByDescending(p => p.TotalSoldCount)
                .Take(count)
                .Include(p => p.Images)
                .Include(p=>p.Category)
                .Include(p=>p.Vendor)
                .ToListAsync();
        }
    
        public IQueryable<Product> GetAll()
        {
            return _context.Products.AsNoTracking();
        }
    }
}

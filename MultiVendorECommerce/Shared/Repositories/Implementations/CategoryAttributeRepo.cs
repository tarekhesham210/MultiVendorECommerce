using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
{
    public class CategoryAttributeRepo: ICategoryAttributeRepository
    {
        private readonly ApplicationDb _context;

        public CategoryAttributeRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddAsync(CategoryAttribute categoryAttribute) 
        {
           await _context.CategoryAttributes.AddAsync(categoryAttribute);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync(); 
        }

        public void Delete(CategoryAttribute entity)
        {
            _context.CategoryAttributes.Remove(entity);
        }

        public async Task<IEnumerable<CategoryAttribute>> GetAttributesByCategoryId(int id)
        {
           return await _context.CategoryAttributes
                .Where(att => att.CategoryId == id)
                .Include(attr => attr.Options).ToListAsync();
        }

    }
}

using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class CategoryAttributeOptionRepo : ICategoryAttributeOptionsRepository
    {
        private readonly ApplicationDb _context;

        public CategoryAttributeOptionRepo(ApplicationDb context)
        {
            _context = context;
        }

        public  IEnumerable<CategoryAttributeOption> GetOptionsByCatetegoryId(int id)
        {
           return _context.CategoryAttributeOptions.Where(o=>o.CategoryAttribute.CategoryId==id).ToList();
        }
        public async Task AddAsync(CategoryAttributeOption categoryAttributeOption)
        {
           await _context.CategoryAttributeOptions.AddAsync(categoryAttributeOption);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

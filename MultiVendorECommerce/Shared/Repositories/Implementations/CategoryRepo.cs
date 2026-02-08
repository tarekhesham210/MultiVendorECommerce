using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Areas.Admin.ViewModels;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class CategoryRepo : ICategoryRepository
    {
        private readonly ApplicationDb _context;

        public CategoryRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task AddCategoryAsync(Category category)
        {
           await _context.Categories.AddAsync(category); 
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync(); 
            
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking().Where(c=>!c.Products.Any()).ToListAsync();
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesWithSubCategoriesAsync()
        {
            return await _context.Categories.Where(c=>c.ParentCategoryId==null).Include(c => c.SubCategories).AsNoTracking().ToListAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
          return await _context.Categories
                .Include(c=>c.SubCategories)
                .Include(c=>c.Attributes).ThenInclude(attr=>attr.Options)
                .SingleOrDefaultAsync(c=>c.Id==id);
        }

       
        public async Task<bool> UpdateCategoryAsync(int id, Action<Category> Update)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category is null)
            {
                return false;
            }
            Update(category);
           return await _context.SaveChangesAsync() > 0;
             
        }
        public async Task<bool> CategoryNameExistsAsync(string name,int? currentId=null)
        {
            return await _context.Categories
                .AnyAsync(c =>
            c.Name == name &&
            (!currentId.HasValue || c.Id != currentId.Value));
        }
        public async Task<bool> CategoryIdExistsAsync(int id)
        {
            return await _context.Categories
                .AnyAsync(c => c.Id == id );
        }
        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public IQueryable<Category> GetAllCategories()
        {
            return  _context.Categories.AsNoTracking();
        }

        public async Task<bool> IsCategoryHasProducts(int id)
        {
            return (await _context.Categories.FindAsync(id)).Products.Any();
        }
    }
}

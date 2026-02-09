
using MultiVendorECommerce.Models;



namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetParentCategoriesAsync();
        Task<IEnumerable<Category>> GetAllCategoriesWithSubCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(Category category);
        Task<bool> UpdateCategoryAsync(int id, Action<Category> Update);
        Task SaveAsync();
        Task DeleteCategoryAsync(Category category);
        Task<bool> CategoryNameExistsAsync(string name, int? currentId = null);
        Task<bool> CategoryIdExistsAsync(int id);
        IQueryable<Category> GetAllCategories();
        Task<bool> IsCategoryHasProducts(int id);
    }
}

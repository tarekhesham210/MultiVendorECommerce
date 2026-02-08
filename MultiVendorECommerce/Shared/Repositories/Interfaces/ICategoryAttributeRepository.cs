using PermissionBasedAuz.Models;

namespace PermissionBasedAuz.Shared.Repositories.Interfaces
{
    public interface ICategoryAttributeRepository
    {
        Task SaveAsync();
        Task AddAsync(CategoryAttribute categoryAttribute);
        void Delete(CategoryAttribute entity);
        Task<IEnumerable<CategoryAttribute>> GetAttributesByCategoryId(int id);
    }
}
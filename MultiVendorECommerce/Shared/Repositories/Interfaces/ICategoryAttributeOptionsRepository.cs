using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface ICategoryAttributeOptionsRepository
    {
        IEnumerable<CategoryAttributeOption> GetOptionsByCatetegoryId(int id);
        Task AddAsync(CategoryAttributeOption categoryAttributeOption);
        Task SaveAsync();
    }
}

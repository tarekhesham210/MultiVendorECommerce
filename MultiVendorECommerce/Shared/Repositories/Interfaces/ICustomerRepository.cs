using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface ICustomerRepository
    {
        IQueryable<Customer> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> GetCustomerByUserIdAsync(string userId);

        Task<bool> AddCustomerAsync(Customer customer);
        Task SaveAsync();

    }
}

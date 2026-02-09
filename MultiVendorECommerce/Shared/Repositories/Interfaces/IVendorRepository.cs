using MultiVendorECommerce.Models;

namespace MultiVendorECommerce.Shared.Repositories.Interfaces
{
    public interface IVendorRepository
    {
        Task<IEnumerable<Vendor>> GetAllVendorsAsync();
        Task<Vendor?> GetVendorByIdAsync(int id);
        Task<bool> AddVendorAsync(Vendor vendor);
        Task SaveAsync();
        Task RejectVendorAsync(Vendor vendor);
        Task<Vendor?> GetVendorByUserIdAsync(string userId);

    }

}

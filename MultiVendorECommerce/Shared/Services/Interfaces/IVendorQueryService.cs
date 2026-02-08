namespace PermissionBasedAuz.Shared.Services.Interfaces
{
    public interface IVendorQueryService
    {
        Task<int> GetVendorByUserIdAsync(string userId);
    }
}

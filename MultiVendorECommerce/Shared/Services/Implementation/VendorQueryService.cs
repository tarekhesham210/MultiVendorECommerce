using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Repositories.Interfaces;
using PermissionBasedAuz.Shared.Services.Interfaces;

namespace PermissionBasedAuz.Shared.Services.Implementation
{
    public class VendorQueryService: IVendorQueryService
    {
        private readonly IVendorRepository _vendorRepo;

        public VendorQueryService(IVendorRepository vendorRepo)
        {
            _vendorRepo = vendorRepo;
        }

        public async Task<int> GetVendorByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("UserId cannot be null or empty.");
            var vendor = await _vendorRepo.GetVendorByUserIdAsync(userId);
            if (vendor == null)
                throw new NotFoundException("Vendor profile not found.");
            if (vendor.VendorStatus != VendorStatus.Approved)
                throw new BusinessRuleException("Invalid Vendor.");

            return vendor.Id;
        }

    }
}

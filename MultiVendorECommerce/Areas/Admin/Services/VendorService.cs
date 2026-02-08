using PermissionBasedAuz.Areas.Admin.ViewModels;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Areas.Admin.Services
{
    public class VendorService
    {
        private readonly IVendorRepository _vendorRepo;
        private readonly UserService _userService;
        public VendorService(IVendorRepository vendorRepo, UserService userService)
        {
            _vendorRepo = vendorRepo;
            _userService = userService;
        }
        public async Task RejectVendor(int Id)
        {
            if (Id <= 0)
                throw new ValidationException("Invalide Vendor Id.");

            Models.Vendor? vendor = await _vendorRepo.GetVendorByIdAsync(Id);
            if (vendor == null)
                throw new NotFoundException("Vendor not found.");
            await _vendorRepo.RejectVendorAsync(vendor);
        }

        public async Task<IEnumerable<VendorUserVM>> GetPendingVendorsAsync()
        {
            return (await _vendorRepo.GetAllVendorsAsync()).Where(v => v.VendorStatus == VendorStatus.Pending)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt

                }).ToList();
        }
        public async Task<IEnumerable<VendorUserVM>> GetApprovedVendorsAsync()
        {
            return (await _vendorRepo.GetAllVendorsAsync()).Where(v => v.VendorStatus == VendorStatus.Approved)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt

                }).ToList();
        }

        public async Task ApproveVendor(int Id)
        {
            if (Id <= 0)
                throw new ValidationException("Invalide Vendor Id.");

            var vendor = await _vendorRepo.GetVendorByIdAsync(Id);
            if (vendor == null)
                throw new NotFoundException("Vendor not found.");
            vendor.VendorStatus = VendorStatus.Approved;
            await _vendorRepo.SaveAsync();
            await _userService.ActivateUser(vendor.UserId);
        }
        public async Task SuspendVendor(int Id)
        {
            if (Id <= 0)
                throw new ValidationException("Invalide Vendor Id.");

            var vendor = await _vendorRepo.GetVendorByIdAsync(Id);
            if (vendor == null)
                throw new NotFoundException("Vendor not found.");
            vendor.VendorStatus = VendorStatus.Suspended;
            await _vendorRepo.SaveAsync();
            await _userService.SuspendUser(vendor.UserId);
        }


        internal async Task<List<VendorUserVM>> GetRejectedVendorsAsync()
        {
            var rejectedVendors = (await _vendorRepo.GetAllVendorsAsync()).Where(v => v.VendorStatus == VendorStatus.Rejected)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt
                }).ToList();
            return rejectedVendors;
        }
    }

}

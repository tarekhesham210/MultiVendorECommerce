using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.ViewModels;

namespace MultiVendorECommerce.Areas.Admin.Services
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

        public async Task<PagedResult<VendorUserVM>> GetPendingVendorsAsync(int pageNumber=1,int pageSize=10)
        {
            return await _vendorRepo.GetAllVendorsAsync().Where(v => v.VendorStatus == VendorStatus.Pending)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt

                }).ToPagedListAsync(pageNumber,pageSize);
        }
        public async Task<PagedResult<VendorUserVM>> GetApprovedVendorsAsync(int pageNumber=1,int pageSize=10)
        {
            return await  _vendorRepo.GetAllVendorsAsync()
                .Where(v => v.VendorStatus == VendorStatus.Approved)
                .OrderByDescending(v=>v.CreatedAt)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt

                }).ToPagedListAsync(pageNumber,pageSize);
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


        internal async Task<PagedResult<VendorUserVM>> GetRejectedVendorsAsync(int pageNumber=1,int pageSize=10)
        {
            var rejectedVendors = await _vendorRepo.GetAllVendorsAsync()
                .Where(v => v.VendorStatus == VendorStatus.Rejected)
                .OrderByDescending(v=>v.CreatedAt)
                .Select(v => new VendorUserVM
                {
                    Id = v.Id,
                    userId = v.User.Id,
                    Email = v.User.Email,
                    StoreName = v.StoreName,
                    Name = v.User.UserName,
                    vendorStatus = v.VendorStatus,
                    CreatedAt = v.CreatedAt
                }).ToPagedListAsync(pageNumber,pageSize);
            return rejectedVendors;
        }

        public async Task<VendorDetailsVM> GetVendorDetails(int vendorId)
        {
            if(vendorId <= 0) throw new ValidationException("Invalid vendor");
            var vendor=await _vendorRepo.GetVendorByIdAsync(vendorId)??throw new NotFoundException("Vendor not found");

            var vendorDetails = new VendorDetailsVM
            {
                Email = vendor.User.Email,
                StoreName = vendor.StoreName,
                StoreDescription = vendor.StoreDescription,
                Phone = vendor.User.PhoneNumber,
                Id = vendor.Id
            };
            return vendorDetails;
        }
    }

}

using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.ViewModels;
using MultiVendorECommerce.Exceptions;
using Microsoft.AspNetCore.Identity;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Services
{
    public class VendorService
    {
        private readonly IVendorRepository _vendorRepo;
        public VendorService(IVendorRepository vendorRepo, UserManager<ApplicationUser> userManager)
        {
            _vendorRepo = vendorRepo;
        }
        public async Task<bool> CreateVendorProfile(VendorRegisterVM vendorVm, string userId)

        {
            if(string.IsNullOrEmpty(userId)) 
                throw new ValidationException("UserId cannot be null or empty.");
            
            var vendor = new Vendor
            {
                UserId = userId,
                StoreName = vendorVm.StoreName,
                StoreDescription = vendorVm.StoreDescription,
                VendorStatus = VendorStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
           bool isAdded= await _vendorRepo.AddVendorAsync(vendor);
              return isAdded;
        }
        public async Task RejectVendor(int Id)
        {
            if(Id<=0) 
                throw new ValidationException("Invalide Vendor Id.");
            
            Vendor? vendor= await _vendorRepo.GetVendorByIdAsync(Id);
            if(vendor==null) 
                throw new NotFoundException("Vendor not found.");
            await _vendorRepo.RejectVendorAsync(vendor);
        }


        public async Task ApproveVendor(int Id)
        {
            if(Id<=0) 
                throw new ValidationException("Invalide Vendor Id.");
            
            Vendor? vendor= await _vendorRepo.GetVendorByIdAsync(Id);
            if(vendor==null) 
                throw new NotFoundException("Vendor not found.");
            vendor.VendorStatus=VendorStatus.Approved;
            await _vendorRepo.SaveAsync();
        }
        public async Task SuspendVendor(int Id)
        {
            if (Id <= 0)
                throw new ValidationException("Invalide Vendor Id.");

            Vendor? vendor = await _vendorRepo.GetVendorByIdAsync(Id);
            if (vendor == null)
                throw new NotFoundException("Vendor not found.");
            vendor.VendorStatus = VendorStatus.Suspended;
            await _vendorRepo.SaveAsync();
        }

        public async Task<int> GetVendorByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ValidationException("UserId cannot be null or empty.");
            var vendor= await _vendorRepo.GetVendorByUserIdAsync(userId);
            if (vendor==null)
                throw new NotFoundException("Vendor profile not found.");
            if(vendor.VendorStatus!=VendorStatus.Approved)
                throw new BusinessRuleException("Invalid Vendor.");

            return vendor.Id;
        }
    }
}

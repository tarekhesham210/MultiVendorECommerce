using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class VendorRepo : IVendorRepository
    {
        private readonly ApplicationDb _context;

        public VendorRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task<bool> AddVendorAsync(Vendor vendor)
        {
            await _context.Vendors.AddAsync(vendor);
            int row = await _context.SaveChangesAsync();
            return row > 0;
        }

        public async Task RejectVendorAsync(Vendor vendor)
        {
             vendor.VendorStatus=VendorStatus.Rejected;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vendor>> GetAllVendorsAsync()
        {
           return await _context.Vendors.Include(v=>v.User).AsNoTracking().ToListAsync();
        }

        public async Task<Vendor?> GetVendorByIdAsync(int id)
        {
            return await _context.Vendors.Include(v=>v.User).SingleOrDefaultAsync(v=>v.Id==id);
        }
        public async Task<Vendor?> GetVendorByUserIdAsync(string userId)
        {
            return await _context.Vendors.Where(v=>v.UserId==userId).SingleOrDefaultAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

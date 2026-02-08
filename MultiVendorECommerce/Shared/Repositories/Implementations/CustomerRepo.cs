using Microsoft.EntityFrameworkCore;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Shared.Repositories.Implementations
{
    public class CustomerRepo : ICustomerRepository
    {
        private readonly ApplicationDb _context;

        public CustomerRepo(ApplicationDb context)
        {
            _context = context;
        }

        public async Task<bool> AddCustomerAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            int rows =await _context.SaveChangesAsync();
            return rows > 0;

        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.Include(c=>c.User).AsNoTracking().ToListAsync(); 
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync( id);
        }

        public async Task<Customer?> GetCustomerByUserIdAsync(string userId)
        {
            return await _context.Customers.Include(c => c.User)
                .SingleOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

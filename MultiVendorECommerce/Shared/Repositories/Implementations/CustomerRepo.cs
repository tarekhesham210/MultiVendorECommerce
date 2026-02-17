using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Repositories.Interfaces;

namespace MultiVendorECommerce.Shared.Repositories.Implementations
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

        public  IQueryable<Customer> GetAllCustomersAsync()
        {
            return  _context.Customers.Include(c=>c.User).AsNoTracking(); 
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

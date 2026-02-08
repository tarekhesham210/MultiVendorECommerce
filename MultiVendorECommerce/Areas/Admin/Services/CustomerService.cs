using PermissionBasedAuz.Areas.Admin.ViewModels;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Repositories.Interfaces;

namespace PermissionBasedAuz.Areas.Admin.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly UserService _userService;

        public CustomerService(ICustomerRepository customerRepository, UserService userService)
        {
            _customerRepository = customerRepository;
            _userService = userService;
        }

        public async Task<List<CustomerVM>> GetAllCustomers()
        {
            var customers= await _customerRepository.GetAllCustomersAsync();
            return customers.Select(c => new CustomerVM
            {
                Id = c.Id,
                FullName = c.FirstName +" "+c.LastName ,
                Email = c.User.Email,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                Status = c.Status

            }).ToList();
          
        }

        public async Task BlockCustomer(int id)
        {          
            if (id <= 0)
            {
                throw new ValidationException("Invalid Customer Id");
            }
            var customer=await _customerRepository. GetCustomerByIdAsync(id); 
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }
            customer.Status = CustomerStatus.Blocked;
            await _customerRepository.SaveAsync();
            await _userService.SuspendUser(customer.UserId);

        }

        internal async Task ActivateCustomer(int id)
        {
            if (id <= 0)
            {
                throw new ValidationException("Invalid Customer Id");
            }
            var customer = await _customerRepository.GetCustomerByIdAsync(id);
            if (customer == null)
            {
                throw new NotFoundException("Customer not found");
            }
            customer.Status = CustomerStatus.Active;
            await _customerRepository.SaveAsync();
            await _userService.ActivateUser(customer.UserId);
        }
    }
}

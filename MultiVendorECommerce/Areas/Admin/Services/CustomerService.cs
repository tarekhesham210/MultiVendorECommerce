using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.ViewModels;

namespace MultiVendorECommerce.Areas.Admin.Services
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

        public async Task<PagedResult<CustomerVM>> GetAllCustomers(int pageNumber=1,int pageSize=10)
        {
            var customers = _customerRepository.GetAllCustomersAsync();
            return await customers.OrderByDescending(c=>c.CreatedAt)
                  .Select(c => new CustomerVM
            {
                Id = c.Id,
                FullName = c.FirstName + " " + c.LastName,
                Email = c.User.Email,
                Phone = c.Phone,
                CreatedAt = c.CreatedAt,
                UserId = c.UserId,
                Status = c.Status

            }).ToPagedListAsync<CustomerVM>(pageNumber,pageSize);
          
        }

        public async Task BlockCustomer(int id)
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

        public async Task<CustomerDetailsVM> GetCustomerDetails(int id)
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
            var user = await _userService.GetUserById(customer.UserId);
            return new CustomerDetailsVM
            {
                Id = customer.Id,
                FullName = customer.FirstName + " " + customer.LastName,
                Email = customer.User.Email,
                Phone = customer.Phone,
                Status = customer.Status.ToString(),
                Image = user.Image
            };

        }
    }
}

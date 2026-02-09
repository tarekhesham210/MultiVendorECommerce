using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Services;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Services.Interfaces;
using MultiVendorECommerce.ViewModels;
using System.Data;
using System.Security.Claims;

namespace MultiVendorECommerce.Areas.Auth.Services
{
    public class AccountService
    {
        private readonly ApplicationDb _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly VendorService _vendorService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IImageService _imageService;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            ApplicationDb context,
            VendorService vendorService,
            IImageService imageService)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
            _vendorService = vendorService;
            _imageService = imageService;
        }

        public async Task<ApplicationUser> GetUserByEmail(string Email)
        {
            return await _userManager.FindByEmailAsync(Email);
        }
        public async Task<ApplicationUser> GetUserById(string Id)
        {
            return await _userManager.FindByIdAsync(Id);
        }
        public string getAreaNameFromClaimUserType(UserType userType)
        {
            var areaName = string.Empty;
            switch (userType)
                {
                case UserType.Internal:         
                    areaName = "Admin";
                    break;
                case UserType.Vendor:
                    areaName = "Vendor";
                    break;
                case UserType.Customer:
                    areaName = "Customer";
                    break;
            }
            return areaName;
        }

        public async Task<ApplicationUser> SignIn(LoginFormVM loginUser,bool isper)
        {
            var user = await GetUserByEmail(loginUser.Email);
            if (user is null)
                throw new NotFoundException("User not found.");
            if (!await _userManager.CheckPasswordAsync(user, loginUser.Password))
                throw new OperationFailedException("Invalid login attempt.");
            var Claims=new List<Claim> { new Claim("UserType", user.UserType.ToString()) };

              await _signInManager.SignInWithClaimsAsync(user, loginUser.RememberMe, Claims);
          
              return user;
        }

        public async Task<string> CreateUserAsync<TRegisterVM>(TRegisterVM registerUser)
            where TRegisterVM : RegisterVM
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerUser.Email.Split("@")[0],
                Email = registerUser.Email,
                UserType=registerUser.userType,
                Image = await _imageService.SaveImageAsync(registerUser.Image,ImageSettings.UsersImagesFolder)
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                throw new OperationFailedException("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            if (registerUser.userType == UserType.Vendor)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
            if (registerUser.userType== UserType.Internal)
            {
                throw new BusinessRuleException("Invalid user type for this registration.");
            }
            result = await _userManager.AddToRoleAsync(user, registerUser.userType.ToString());
            if (!result.Succeeded)
            {
                _imageService.DeleteImage(user.Image, ImageSettings.UsersImagesFolder);
                await _userManager.DeleteAsync(user);
                throw new OperationFailedException("Failed to add role to user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            return user.Id;
        }
          
        public async Task<Models.Customer> CreateCustomerProfile(CustomerRegisterVM customerVm,string userId)
        {
            
            var customer = new Models.Customer
            {
                UserId = userId,
                FirstName = customerVm.FirstName,
                LastName = customerVm.LastName,
                CreatedAt = DateTime.UtcNow
            };
             await _context.Customers.AddAsync(customer);
            int row=  await _context.SaveChangesAsync();
            if(row<=0)
            {
                throw new InvalidOperationException("Failed to create customer profile.");
            }
            return customer;
        }

    
        public async Task CreateUserWithCustomerProfile(CustomerRegisterVM customerVM)
        {
           await using var transaction = await _context.Database.BeginTransactionAsync();
            var userId= string.Empty;
            try
            {
                 userId = await CreateUserAsync(customerVM);
                await CreateCustomerProfile(customerVM, userId);
                await transaction.CommitAsync();
            }
            catch
            {
                if (!string.IsNullOrEmpty(userId))
                    await DeleteUserByIdAsync(userId);
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CreateUserWithVendorProfile(VendorRegisterVM vendorVM)
        {
           await using var transaction = await _context.Database.BeginTransactionAsync();
            var userId= string.Empty;
            try
            {
                userId = await CreateUserAsync(vendorVM);
                bool isVendorCreated = await _vendorService.CreateVendorProfile(vendorVM, userId);
                if (!isVendorCreated)
                {
                    await DeleteUserByIdAsync(userId);
                    throw new OperationFailedException("Failed to create vendor profile.");
                }
                await transaction.CommitAsync();
            }
            finally
            {
                if (transaction.GetDbTransaction().Connection != null)
                {
                    await transaction.RollbackAsync();

                    if (!string.IsNullOrEmpty(userId))
                        await DeleteUserByIdAsync(userId);
                }
            }
        }

        public async Task DeleteUserByIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ValidationException("UserId cannot be null or empty.");
            }
            var user= await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                throw new NotFoundException("User not found.");
            }
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new OperationFailedException("Failed to delete user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            
             
        }
        public async Task ActivateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            await _userManager.SetLockoutEndDateAsync(user, null);
        }
        public async Task SuspendUser(string id)
        {
            if(string.IsNullOrEmpty(id))
            {
                throw new ValidationException("UserId cannot be null or empty.");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
        }
    }
}

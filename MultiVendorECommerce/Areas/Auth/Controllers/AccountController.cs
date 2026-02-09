using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Auth.Services;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Auth.Controllers
{
    [AllowAnonymous]
    [Area("Auth")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AccountService _accountService;

        public AccountController(UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            AccountService accountService)
        {
            this.userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity != null && User.Identity.IsAuthenticated)
            {
                var claim=User.FindFirstValue("UserType");
                return RedirectToAction("Index", "Home", new {
              area= _accountService.getAreaNameFromClaimUserType(Enum.Parse<UserType>(claim))});
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginFormVM loginUser)
        {
            if (ModelState.IsValid)
            {
               var user= await _accountService.SignIn( loginUser, false); 
                var areaName= _accountService.getAreaNameFromClaimUserType(user.UserType);

                return RedirectToAction("Index", "Home", new { area=areaName });               
            }

            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterCustomerPartial()
       => PartialView("_CustomerRegisterPartial", new CustomerRegisterVM());

        [HttpGet]
        public IActionResult RegisterVendorPartial()
            => PartialView("_VendorRegisterPartial", new VendorRegisterVM());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  async Task<IActionResult> RegisterCustomer(CustomerRegisterVM registerCustomer)
        {
            
            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            try
            {
                
                await _accountService.CreateUserWithCustomerProfile(registerCustomer);
              
                return RedirectToAction("Login");
                    
            }
            catch (InvalidOperationException ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View("Register");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("",$"{ex.Message}/n {ex.InnerException?.Message}" );
                return View("Register");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterVendor(VendorRegisterVM registerVendor)
        {

            if (!ModelState.IsValid)
            {
                return View("Register");
            }
            try
            {

                await _accountService.CreateUserWithVendorProfile(registerVendor);
               
                return View("VendorRegisterMessage");
                
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Register");
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Register");
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Register");
            }
        }
      
#region 
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterVM registerUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid Register Attempt");
        //        return View(registerUser);

        //        //var user = new ApplicationUser
        //        //{
        //        //    UserName = registerUser.Email.Split("@")[0],
        //        //    Email = registerUser.Email,
        //        //   // Image=registerUser.Image
        //        //};
        //        //var result = await userManager.CreateAsync(user, registerUser.Password);
        //        //if (result.Succeeded)
        //        //{
        //        //    // Assign "User" role to the newly registered user

        //        //    await userManager.AddToRoleAsync(user, Roles.User.ToString());
        //        //    return RedirectToAction("Login", "Account");
        //        //}
        //        //foreach (var error in result.Errors)
        //        //{
        //        //    ModelState.AddModelError(string.Empty, error.Description);
        //        //}
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var isCreated = await _accountService.CreateUserAsync(registerUser);
        //            return RedirectToAction("Login", "Account");

        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError(string.Empty, ex.Message);
        //            return View(registerUser);
        //        }
        //    }
        //}
        #endregion 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home",new {area="Customer"});
        }

    }
}

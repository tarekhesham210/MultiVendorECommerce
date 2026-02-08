using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PermissionBasedAuz.Areas.Admin.Controllers;
using PermissionBasedAuz.Data;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Services;
using PermissionBasedAuz.ViewModels;
using System.Security.Claims;

namespace PermissionBasedAuz.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly VendorService _vendorService;
        private readonly ApplicationDb _context;

        public ProductController(ProductService productService, VendorService vendorService, ApplicationDb context)
        {
            _productService = productService;
            _vendorService = vendorService;
            _context = context;
        }

        public IActionResult Index()
        {
            var image= _context.ProductImages.FirstOrDefault(p=>p.ProductId==2 && p.IsMain==true);
            return View(image);
        }
        [HttpGet]
        public async Task<IActionResult> AddProductAsync()
        {
            var productVM = await _productService.CreateProductAsync();
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductVM productvm)
        {
            if(!ModelState.IsValid)
            {
                return View(productvm);
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
              var vendorId= await _vendorService.GetVendorByUserIdAsync(userId);
              //  await _productService.AddProductAsync(productvm, vendorId);
                return RedirectToAction(actionName:nameof(Index),controllerName:nameof(HomeController));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(productvm);
            }
            catch (NotFoundException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(productvm);
            }
            catch (DomainException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(productvm);
            }
          

        }

    }
}

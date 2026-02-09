using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Vendor.Services;
using MultiVendorECommerce.Areas.Vendor.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Vendor.Controllers
{
    [Area("Vendor")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> VendorProduct()
        {
            var products = await _productService.GetProductsByVendorUserdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return View(products);
        }
        [HttpGet]
        public async Task<IActionResult> SelectCategoryForProduct( )
        {
            var categories = await _productService.GetCategoriesForSelection();
            return View(categories);
        }
        [HttpPost]
        public async Task<IActionResult> SelectCategoryForProduct(SelectCategoryVM selectcategory)
        {
            if (!ModelState.IsValid)
            {
                var select=await _productService.GetCategoriesForSelection();
                return View(select);

            }

            return RedirectToAction(nameof(AddProduct),selectcategory);
         
        }
      
        [HttpGet]
        public async Task<IActionResult> AddProduct(SelectCategoryVM selectCategory)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(SelectCategoryForProduct), selectCategory);
            var productForm = await _productService.CreateProductAsync(selectCategory);
            return View(productForm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _productService.AddProductAsync(productVM, currentUserId);

            return RedirectToAction("VendorProduct");
            ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProduct(int Id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("VendorProduct");
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _productService.DeleteProductAsync(Id, currentUserId);
            return RedirectToAction("VendorProduct");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int Id)
        {
            var productVM = await _productService.GetProductForEditAsync(Id);
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductVM productVM)
        {
            if (!ModelState.IsValid)
            {
                return View(productVM);
            }
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _productService.EditProductAsync(productVM, currentUserId);
            return RedirectToAction("VendorProduct");
        }
    }
}

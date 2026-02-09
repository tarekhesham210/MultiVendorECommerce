using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiVendorECommerce.Areas.Customer.Services;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Shared.Services.Interfaces;
using System.Threading.Tasks;

namespace MultiVendorECommerce.Areas.Customer.Controllers
{
    [AllowAnonymous]

    [Area("Customer")]

    public class HomeController : Controller
    {
        private readonly ICategoryQueryService _categoryQueryService;
        private readonly HomeService _homeService;

        public HomeController(ICategoryQueryService categoryQueryService, HomeService homeService)
        {
            _categoryQueryService = categoryQueryService;
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var home=await _homeService.GetHomeData();
            return View(home);
        }
        //public async Task<IActionResult> Index()
        //{
        //    var categoris = await _homeService.GetAllCategoriesForDropdownList();
        //    var model= new CategoriesHeaderVM
        //    {
        //        Categories = await _homeService.GetAllCategoriesForDropdownList(),

        //    };
        //    return View(model);
        //}
        //public async Task<IActionResult> Categoris()
        //{
        //    var categoris= await _homeService.GetAllCategoriesForDropdownList();
        //    return View(categoris);
        //}
        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts(int id)
        {
            if(!ModelState.IsValid)
                return View(nameof(Index));
            var categoryProducts=await _homeService.GetCategoryProducts(id);
            return View(categoryProducts);

        }
        [HttpGet]
        public async Task<IActionResult> ProductDetails(int productId,int? variantId)
        { 
            var product=await _homeService.GetProductDetailes(productId,variantId);
            return View(product);
        }

        public IActionResult cart()
        {
            return View();
        }
    }
}

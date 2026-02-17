using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Areas.Vendor.ViewModels;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Implementation;
using MultiVendorECommerce.Shared.Services.Interfaces;
using System.Drawing.Printing;
using System.Globalization;

namespace MultiVendorECommerce.Areas.Customer.Services
{
    public class HomeService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IProductQueryService _productQueryService;
        private readonly IProductVariantRepository _variantRepository;

        public HomeService(ICategoryRepository categoryRepository, IProductRepository productRepository, IProductVariantRepository variantRepository, IProductQueryService productQueryService)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _variantRepository = variantRepository;
            _productQueryService = productQueryService;
        }

        // private readonly IProductQueryService _productQueryService;


        public async Task<HomeVM> GetHomeData()
        {
            var homeVM = new HomeVM
            {
                Categories = await GetCategories(),
                BestSellers = await GetBestSellersProducts(8),
            };
            return homeVM;
        }
        public async Task<IEnumerable<CategoriesForHomeVM>> GetAllCategoriesForDropdownList()
        {
            var categories = await _categoryRepository.GetAllCategoriesWithSubCategoriesAsync();
            var categoriesForHome = categories.Select(c => new CategoriesForHomeVM
            {
                Id = c.Id,
                Name = c.Name,
                SubCategories = c.SubCategories.Select(sc => new SelectListItem
                {
                    Value = sc.Id.ToString(),
                    Text = sc.Name
                }).ToList()
            });
            return categoriesForHome;
        }

        public async Task<IEnumerable<ProductWithHotOffer>> GetHotOffers(int count)
        {
            var products= await _productQueryService.GetHotOfferVariantAsync(count);
            return products;
        }
    
        public async Task<CategoryProductsVM> GetCategoryProducts(int id)
        {
            if (id == 0)
                throw new ValidationException("Invalid Category id");
            var category=await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category Not Found");

            var categoryProducts = await _productQueryService.GetProductsByCategoryIdAsync(category.Id);

            return new CategoryProductsVM
            {
                CategoryId = category.Id,
                CategoryName = category.Name,

                Products = categoryProducts
            };            
        }

        public async Task<ProductDetailsVM> GetProductDetailes(int id,int? variantId)
        {
            if (id <= 0)
                throw new ValidationException("invalid id");
            var product=await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                throw new NotFoundException("product not found");

            var productDetails =await _productQueryService.GetProductDetailes(id,variantId);

            return productDetails;
        }





        private async Task<IEnumerable<ProductVariantCardVM>> GetBestSellersProducts(int count)
        {
            var bestSellers = await _productQueryService.GetProductsBestSellersAsync(count);
            return bestSellers;
                  
        }
    

        internal async Task<IEnumerable<ProductVariantCardVM>> GetLiveSearchResultsAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term)) return new List<ProductVariantCardVM>();

            return await _productQueryService.LiveSearchProductsAsync(term);
        }
        public async Task<SearchResultVM> GetProductsBySearchTermAsync(string searchTerm, string sortBy,int? categoryId,int pageNumber=1,int pageSize=9)
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return new SearchResultVM();

            var (products,totalCount) = await _productQueryService.GetProductsBySearchTermAsync(searchTerm,sortBy,pageSize, pageNumber);
            var result = new SearchResultVM
            {
                Products = products.ToList(),
                CurrentPage = pageNumber,
                CurrentSortBy = sortBy,
                CurrentSearchTerm = searchTerm,
                SelectedCategoryId = categoryId,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),

            };
            return result;
        }
        private async Task<IEnumerable<CategoryVM>> GetCategories()
        {
            return await _categoryRepository.GetAllCategories()
                .Where(c => !c.SubCategories.Any())
                .Select(c => new CategoryVM
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl,
                    ProductsCount = c.Products.Count
                }).ToListAsync();
        }
    }
}

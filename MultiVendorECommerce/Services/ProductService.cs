using Microsoft.AspNetCore.Mvc.Rendering;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;
using MultiVendorECommerce.ViewModels;

namespace MultiVendorECommerce.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IImageService _imageService;
        public ProductService(IProductRepository productRepo, IImageService imageService, ICategoryRepository categoryRepo)
        {
            _productRepo = productRepo;
            _imageService = imageService;
            _categoryRepo = categoryRepo;
        }

        //public async Task AddProductAsync(AddProductVM productvm,int vendorId)
        //{
        //    if(!await _categoryRepo.CategoryIdExistsAsync(productvm.CaegoryId))
        //    {
        //        throw new ValidationException("Invalide category.");
        //    }
            
        //    var product = new Product
        //    {
        //        Name = productvm.Name,
        //        Description = productvm.Description,
        //        Price = productvm.Price,
        //        StockQuantity = productvm.StockQuantity,
        //        VendorId = vendorId,
        //        CreatedAt = DateTime.UtcNow,
        //        CategoryId = productvm.CaegoryId,
        //    };
        //    if(product.StockQuantity<1)
        //        product.ProductStatus=ProductStatus.OutOfStock;
        //    bool isFirst=true;
        //    foreach (var image in productvm.ProductImages)
        //    {
        //        if (image == null || image.Length == 0)
        //            throw new ValidationException("Invalid image file");

        //        var imageName = await _imageService.SaveImageAsync(image, ImageSettings.ProudctsImagesFolder);
        //        product.Images.Add(new ProductImage
        //        {

        //            ImageUrl = imageName,

        //            IsMain = isFirst,
        //        }); 
               
        //        isFirst =false;
        //    }
        //    bool isAdded =await _productRepo.AddProductAsync(product);

        //}

        //public async Task<IEnumerable<Product>> GetAllProductsAsync()
        //{
        //    return await _productRepo.GetAllProductsAsync();
        //}

        //public async Task<Product?> GetProductByIdAsync(int id)
        //{
        //    return await _productRepo.GetProductByIdAsync(id);
        //}
        //public async Task<IEnumerable<Product>>> GetProductsByVendorIdAsync(int vendorId)
        //{
        //    return await _productRepo.GetProductsByVendorIdAsync(vendorId);
        //}
        //public async Task<bool> DeleteProductAsync(int id)
        //{
        //    return await _productRepo.DeleteProductAsync(id);
        //}
            
        public async Task<AddProductVM> CreateProductAsync()
        {
            var categories = await _categoryRepo.GetParentCategoriesAsync();
            var productVM = new AddProductVM
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList()
            };
            return productVM;
        }
    }
}

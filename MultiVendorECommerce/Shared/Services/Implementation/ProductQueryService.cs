using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Customer.ViewModels;
using MultiVendorECommerce.Areas.Vendor.ViewModels;
using MultiVendorECommerce.Data;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;
using System.Collections;

namespace MultiVendorECommerce.Shared.Services.Implementation
{
    public class ProductQueryService:IProductQueryService
    {
        private readonly ApplicationDb _context;
       
        public ProductQueryService(ApplicationDb context)
        {
            _context = context;
        }

        public async Task<EditProductVM?> GetProductForEditAsync(int productId)
        {
            var product = await _context.Products
                .Include(p=>p.Images)
           .Include(p => p.AttributeValues)
           .Include(p => p.Variants).ThenInclude(v => v.VariantValues)
           .Include(p => p.Variants).ThenInclude(v => v.CurrentOffer)
           .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null) return null;

            var allCategoryAttributes = await _context.CategoryAttributes
                .Where(ca => ca.CategoryId == product.CategoryId)
                .Include(ca => ca.Options)
                .ToListAsync();

            // Identify active dimensions in existing variants
            var activeDimIds = product.Variants
                .SelectMany(v => v.VariantValues.Select(vv => vv.CategoryAttributeOption.CategoryAttributeId))
                .Distinct().ToList();

            var model = new EditProductVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                CategoryId = product.CategoryId,
                ExistingImages=product.Images.Select(i=>new Areas.Vendor.ViewModels.ProductImageVM
                {
                    Id=i.Id,
                    ImageUrl = i.ImageUrl,
                    IsMain=i.IsMain,
                }).ToList(),
                // 1. Fixed Attributes (IsVariant = false)
                FixedAttributesValues = allCategoryAttributes.Where(a => !a.IsVariant).Select(a => new ProductAttributeValueVM
                {
                    AttributeId = a.Id,
                    AttributeName = a.Name,
                    SelectedOptionId = product.AttributeValues.FirstOrDefault(av => av.CategoryAttributeId == a.Id)?.CategoryAttributeOptionId ??0,
                    Options = a.Options.Select(o => new CategoryAttributeOptionVM { Id = o.Id, Value = o.Value }).ToList()
                }).ToList(),

                // 2. Dimensions (IsVariant = true)
                Dimensions = allCategoryAttributes.Where(a => a.IsVariant).Select(a => new ProductAttributeSchemaVM
                {
                    AttributeId = a.Id,
                    AttributeName = a.Name,
                    IsActive = activeDimIds.Contains(a.Id),
                    AllowableOptions = a.Options.Select(o => new CategoryAttributeOptionVM { Id = o.Id, Value = o.Value }).ToList()
                }).ToList(),

                // 3. Existing Variants
                Variants = product.Variants.Select(v => new ProductVariantVM
                {
                    Id = v.Id,
                    SKU = v.SKU,
                    Price = v.Price,
                    StockQuantity = v.StockQuantity,
                    SelectedOptionIds = v.VariantValues.Select(vv => vv.CategoryAttributeOptionId).ToList(),
                    HasOffer=v.CurrentOffer!=null,
                    DiscountPercentage=v.CurrentOffer?.DiscountPercentage,
                    StartDate=v.CurrentOffer?.StartDate,
                    EndDate=v.CurrentOffer?.EndDate,
                    IsActive=v.CurrentOffer?.IsActive ?? false,
                }).ToList()
            };

            return model;
        }

        public async Task<IEnumerable<ProductWithHotOffer>> GetHotOfferVariantAsync(int count)
        {
            var products = await _context.ProductVariants
                .Where(v => v.ProductStatus == ProductStatus.Available

                 && v.CurrentOffer != null
                )
                 .OrderByDescending(p => p.CurrentOffer!.DiscountPercentage)
                .Take(count)
                .Select(p => new ProductWithHotOffer
                {
                    Id = p.Id,
                    Name = p.Product.Name,
                    ProductId=p.ProductId,
                    MainImageUrl = p.Product.Images.FirstOrDefault(i => i.IsMain)!.ImageUrl,
                    OldPrice = p.Price,
                    NewPrice = p.FinalPrice,
                    Discount = (int)p.CurrentOffer!.DiscountPercentage,

                }).ToListAsync();

            return products;

        }
        
        public async Task<IEnumerable<ProductVariantCardVM>> GetProductsByCategoryIdAsync(int id)
        {
            var categoryProducts = await _context.ProductVariants
          .Where(c => c.Product.CategoryId == id)
          .Select(c => new ProductVariantCardVM
          {
              Id = c.Id,
              Name = c.Product.Name,
              ProductId = c.ProductId,
              HasOffer = c.CurrentOffer != null,
              Price = c.Price,
              Discount=c.CurrentOffer!=null? (int)c.CurrentOffer.DiscountPercentage:0,
              MainImageUrl = c.Image.ImageUrl ?? c.Product.Images.First().ImageUrl, 
          }).ToListAsync();
            return categoryProducts;
        }
        public async Task<IEnumerable<ProductVariantCardVM>> GetProductsBestSellersAsync(int count)
        {
            var categoryProducts = await _context.ProductVariants
          .OrderByDescending(v => v.SoldCount).Take(count)
          .Select(c => new ProductVariantCardVM
          {
              Id = c.Id,
              Name = c.Product.Name,
              ProductId = c.ProductId,
              HasOffer = c.CurrentOffer != null,
              Price = c.Price,
              Discount=c.CurrentOffer!=null? (int)c.CurrentOffer.DiscountPercentage:0,
              MainImageUrl = c.Image.ImageUrl ?? c.Product.Images.First().ImageUrl,
              
          }).ToListAsync();
            return categoryProducts;
        }

        public async Task<ProductDetailsVM> GetProductDetailes(int id,int? variantId)
        {
           var product= await _context.Products.Where(p => p.Id == id)
                .Select(p => new ProductDetailsVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryName=p.Category.Name,
                    CategoryId=p.Category.Id,
                    GalleryImages=p.Images.Select(i=>i.ImageUrl).ToList(),
                    SpecificationAttributes=p.AttributeValues
                           .Select(attr=>new FixedAttributeVM
                           {
                               Name=attr.CategoryAttribute.Name,
                               Value=attr.CategoryAttributeOption.Value,

                           }).ToList(),
                    Variants=p.Variants.Select(v=>new VariantDetailsVM
                    {
                        Id=v.Id,
                        SKU=v.SKU,
                        Price=v.Price,
                        Stock=v.StockQuantity,
                        ImageUrl=v.Image.ImageUrl,
                        OfferPrice=v.FinalPrice,
                        OptionIds=v.VariantValues.Select(vv=>vv.CategoryAttributeOptionId).ToList(),
                        
                    }).ToList(),
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            var existVariavtOptions=product.Variants.SelectMany(v=>v.OptionIds).Distinct().ToList();
            var selection =await _context.CategoryAttributes
                .Where(ca => ca.CategoryId == product.CategoryId&&ca.IsVariant)
                .Select(ca => new ProductAttributeVM
                {
                    AttributeId = ca.Id,
                    Name = ca.Name,
                    Values = ca.Options.Where(o=>existVariavtOptions.Contains(o.Id))
                    .Select(o => new ProductAttributeOptionVM
                    {
                        Id = o.Id,
                        Value = o.Value,
                    }).ToList()
                }).ToListAsync();

            product.SelectionAttributes = selection;
            if (variantId.HasValue)
            {
                var selectedVariant = product.Variants.FirstOrDefault(v => v.Id == variantId);
                if (selectedVariant != null)
                {
                    product.InitialSelectedIds = selectedVariant.OptionIds;
                    product.RequestedVariantId = variantId;
                }
            }

            return product;
        }
    }
}

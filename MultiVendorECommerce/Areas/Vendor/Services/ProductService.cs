using Microsoft.AspNetCore.Mvc.Rendering;
using PermissionBasedAuz.Constants;
using PermissionBasedAuz.Models;
using PermissionBasedAuz.Shared.Enums;
using PermissionBasedAuz.Shared.Services.Interfaces;
using PermissionBasedAuz.Areas.Vendor.ViewModels;
using PermissionBasedAuz.Shared.Repositories.Interfaces;
using PermissionBasedAuz.Exceptions;
using PermissionBasedAuz.Data;

using Microsoft.EntityFrameworkCore;


namespace PermissionBasedAuz.Areas.Vendor.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductVariantRepository _productVariantRepo;
        private readonly IOfferRepository _offerRepo;
        private readonly ApplicationDb _context;
        private readonly ICategoryQueryService _categoryQueryService;
        private readonly IProductQueryService _productQueryService;
        private readonly ICategoryRepository _categoryRepo;
        private readonly ICategoryAttributeRepository _categoryAttributeRepo;
        private readonly ICategoryAttributeOptionsRepository _categoryAttributeOptionsRepo;
        private readonly IVendorQueryService _vendorQueryService;
        private readonly IImageService _imageService;
        public ProductService(IProductRepository productRepo,
            IImageService imageService, ICategoryQueryService categoryQueryService,
            IVendorQueryService vendorQueryService, ApplicationDb context,
            IOfferRepository offerRepo, ICategoryRepository categoryRepo,
            ICategoryAttributeRepository categoryAttributeRepo, IProductVariantRepository productVariantRepo, ICategoryAttributeOptionsRepository categoryAttributeOptionsRepo, IProductQueryService productQueryService)
        {
            _productRepo = productRepo;
            _imageService = imageService;
            _categoryQueryService = categoryQueryService;
            _vendorQueryService = vendorQueryService;
            _context = context;
            _offerRepo = offerRepo;
            _categoryRepo = categoryRepo;
            _categoryAttributeRepo = categoryAttributeRepo;
            _productVariantRepo = productVariantRepo;
            _categoryAttributeOptionsRepo = categoryAttributeOptionsRepo;
            _productQueryService = productQueryService;
        }

        //TODO: Fix this after migration
        public async Task<IEnumerable<ProductVM>> GetProductsByVendorUserdAsync(string vendorUserId)
        {
            var vendorId = await _vendorQueryService.GetVendorByUserIdAsync(vendorUserId);
            if (vendorId <= 0)
            {
                throw new ValidationException("Invalide vendor id.");
            }
            var products = await _productRepo.GetProductsByVendorIdAsync(vendorId);
            var productVMs = products.Select(p => new ProductVM
            {
                Id = p.Id,
                VendorId = p.VendorId,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                Name = p.Name,
                Description = p.Description,
                //TODO: Fix this after migration
                // Price = p.Price,
                //StockQuantity = p.StockQuantity,
             //   ProductStatus = p.ProductStatus,
                CreatedAt = p.CreatedAt,
                Images = p.Images
            }).ToList();
            return productVMs;
        }
        //post :Add Product
        public async Task AddProductAsync(AddProductVM productvm, string userId)
        {
            var vendorId = await ValidateVendorAsync(userId);
            await ValidateCommonProductRulesAsync(productvm, vendorId);
            await ValidateProductAttributesAsync(productvm.CategoryId, productvm);

            var product = new Product
            {
                Name = productvm.Name,
                Description = productvm.Description,
                VendorId = vendorId,
                CategoryId = productvm.CategoryId,
                AttributeValues = productvm.FixedAttributesValues.Select(attr => new ProductAttributeValue
                {
                    CategoryAttributeId = attr.AttributeId,
                    CategoryAttributeOptionId = attr.SelectedOptionId
                }).ToList()
            };
            var mapaattributesToOptions = _categoryAttributeOptionsRepo
                .GetOptionsByCatetegoryId(productvm.CategoryId)
                .ToDictionary(a => a.Id, a => a.CategoryAttributeId);
            foreach (var variantVM in productvm.CreatedVariants)
            {
                var newVariant = new ProductVariant
                {
                    SKU = variantVM.SKU,
                    Price = variantVM.Price,
                    VariantValues = variantVM.SelectedOptionIds.Select(v => new ProductVariantValue
                    {
                        CategoryAttributeId = mapaattributesToOptions[v],
                        CategoryAttributeOptionId = v
                    }).ToList()
                };
                newVariant.SetStockQuantity(variantVM.StockQuantity);

                await HandleVariantOfferAsync(newVariant, variantVM);

                product.Variants.Add(newVariant);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            List<ProductImage> images = new List<ProductImage>(); 

            try
            {
                await _productRepo.AddProductAsync(product);
                images = await SaveProductImages(productvm.ProductImages);
                product.Images = images;
                await _context.SaveChangesAsync();

                
                for (int i = 0; i < productvm.CreatedVariants.Count; i++)
                {
                    var v = productvm.CreatedVariants[i];
                    if (v.SelectedImageId.HasValue)
                    {
                        var variant = product.Variants.FirstOrDefault(va => va.SKU == v.SKU);
                       var url = images[v.SelectedImageId.Value].ImageUrl;
                        variant.VariantImageId = product.Images.First(i=>i.ImageUrl==url).Id;
                    }
                }
                    await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                DeleteProductImages(images.Select(i => i.ImageUrl).ToList());
                throw;
            }
        }

        public async Task<SelectCategoryVM> GetCategoriesForSelection()
        {
            var categories = await _categoryRepo.GetAllCategories()
            .Where(c => !c.SubCategories.Any()).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToListAsync();

            var select = new SelectCategoryVM
            {
                Categories = categories

            };
            return select;
        }

        //Get : Create Product
        public  async Task<AddProductVM> CreateProductAsync(SelectCategoryVM selectCategory)

        {
            await ValidateCategory(selectCategory.CategoryId);
            var attributes=await _categoryAttributeRepo.GetAttributesByCategoryId(selectCategory.CategoryId);
            var productVM = new AddProductVM
            {
                CategoryId = selectCategory.CategoryId,                
                AvailableAttributes = attributes.Select(att => new CategoryAttributeVM
                {
                    Id = att.Id,
                    Name = att.Name,
                    IsVariant = att.IsVariant,
                    IsRequired = att.IsRequired,                    
                    Options = att.Options.Select(o => new CategoryAttributeOptionVM 
                    {
                        Id = o.Id,
                        Value = o.Value
                    }).ToList()
                    

                }).ToList()

            };
            
            return productVM;
        }
        //Get : Edit Product
        internal async Task<EditProductVM?> GetProductForEditAsync(int id)
        {
            var product = await _productRepo.GetProductByIdAsync(id);
            if (product == null)
            {
                throw new NotFoundException("Product not found.");
            }
            var categories = await _categoryRepo.GetAllCategories()
                .Where(c => !c.SubCategories.Any()).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToListAsync();
            var categoryAttribute = await _categoryAttributeRepo.GetAttributesByCategoryId(product.CategoryId);
            var productVm =await _productQueryService.GetProductForEditAsync(product.Id);

            //var attributes = categoryAttribute.Select(att => new CategoryAttributeVM
            //{
            //    Id = att.Id,
            //    Name = att.Name,
            //   IsRequired = att.IsRequired,
            //   IsVariant=att.IsVariant,
            //    Options = att.Options.Select(o => new CategoryAttributeOptionVM
            //    {
            //        Id = o.Id,
            //        Value = o.Value
            //    }).ToList(),

            //    }).ToList();
            //    productVm.AvailableAttributes= attributes;
                //var productVM = new EditProductVM
                //{
                //    Id = product.Id,
                //    Name = product.Name,
                //    Description = product.Description,           
                //    CategoryId = product.CategoryId,
                //    Categories = categories,
                //    ExistingImages = product.Images.Select(i => new ProductImageVM
                //    {
                //        Id = i.Id,
                //        ImageUrl = i.ImageUrl,
                //        IsMain = i.IsMain
                //    }).ToList(),
                //    Attributes=attributes
                //};

            return productVm;

        }

        //post : Edit Product
        internal async Task EditProductAsync(EditProductVM productVM, string currentUserId)
        {
            if(productVM.Id == 0)
                throw new ValidationException("Invalid product id.");
            var vendorId = await ValidateVendorAsync(currentUserId);

            await ValidateCommonProductRulesAsync(productVM,vendorId);
            var product = await GetAndValidateProductForEditAsync((int)productVM.Id, vendorId);
            ValidateImages(product, productVM);
            await ApplyProductUpdates(product, productVM);
           
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var imegesToBeDeleted = await HandleImagesAsync(product, productVM);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                DeleteProductImages(imegesToBeDeleted);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

        }

        internal async Task DeleteProductAsync(int productId, string currentUserId)
        {   
            if (productId <= 0 ||string.IsNullOrEmpty(currentUserId))
            {
                throw new ValidationException("Invalid Data.");
            }
            var vendorId = await _vendorQueryService.GetVendorByUserIdAsync(currentUserId);
            if (vendorId<=0)
            {
                throw new NotFoundException("Vendor not found.");
            }
            var product = await _productRepo.GetProductByIdAsync(productId);
            if(product == null)
            {
                throw new NotFoundException("Product not found.");
            }
            DeleteProductImages(product.Images.Select(i=>i.ImageUrl).ToList());

            bool isDeleted = await _productRepo.DeleteProductAsync(product);
            if (!isDeleted)
            {
                throw new Exception("Failed to delete the product.");
            }

        }

        private void ValidateOffer(DateTime startDateUtc,DateTime endDateUtc, decimal DiscountPercentage )
        {
           
            if (
               DiscountPercentage < 1 || DiscountPercentage > 90
               || startDateUtc >= endDateUtc
               || endDateUtc < DateTime.UtcNow)
            {
                throw new ValidationException("Invalid Offer data.");
            }
        }
        private async Task <List<ProductImage>> SaveProductImages(List<IFormFile> images)
        {
            bool isFirst = true;
            var productImages = new List<ProductImage>();

            foreach (var image in images)
            {
                if (image == null || image.Length == 0)
                    throw new ValidationException("Invalid image file");

                var imageName = await _imageService.SaveImageAsync(image, ImageSettings.ProudctsImagesFolder);
                productImages.Add(new ProductImage
                {

                    ImageUrl = imageName,

                    IsMain = isFirst,
                });

                isFirst = false;
            }
            return productImages;
        }
        private  void DeleteProductImages(List<string> imagesUrl)
        {
            foreach (var image in imagesUrl)
            {
                if (image == null || image.Length == 0)
                    throw new ValidationException("Invalid image file");

                _imageService.DeleteImage(image, ImageSettings.ProudctsImagesFolder);               
            }
        }



        private async Task HandleVariantOfferAsync(ProductVariant variant, ProductVariantVM vm)
        {
            if (!vm.HasOffer)
            {
                if (variant.OfferId != null)
                {
                    var oldOffer = await _offerRepo.GetById(variant.OfferId.Value);
                    if (oldOffer != null)
                    {
                        _offerRepo.DeleteOffer(oldOffer);
                    }

                    variant.OfferId = null;
                    variant.CurrentOffer = null;
                }
                return;
            }

            if (vm.StartDate == null || vm.EndDate == null || vm.DiscountPercentage == null)
                throw new ValidationException($"Invalid offer data {vm.SKU}");

            var startUtc = DateTime.SpecifyKind(vm.StartDate.Value, DateTimeKind.Local).ToUniversalTime();
            var endUtc = DateTime.SpecifyKind(vm.EndDate.Value, DateTimeKind.Local).ToUniversalTime();

            ValidateOffer(startUtc, endUtc, vm.DiscountPercentage.Value);

            if (variant.OfferId != null)
            {
                var existingOffer = await _offerRepo.GetById(variant.OfferId.Value);
                if (existingOffer != null)
                {
                    existingOffer.DiscountPercentage = vm.DiscountPercentage.Value;
                    existingOffer.StartDate = startUtc;
                    existingOffer.EndDate = endUtc;
                    return;
                }
            }

            var newOffer = new Offer
            {
                DiscountPercentage = vm.DiscountPercentage.Value,
                StartDate = startUtc,
                EndDate = endUtc,
            };

            await _offerRepo.AddOfferAsync(newOffer);
            variant.CurrentOffer = newOffer;
        }
        private async Task<List<string>> HandleImagesAsync(Product product, EditProductVM vm)
        {
            var imagesToBeDeleted = new List<string>();

            foreach (var existingImage in vm.ExistingImages)
            {
                var image = product.Images.FirstOrDefault(i => i.Id == existingImage.Id);
                if (image == null) continue;

                if (existingImage.MarkForDelete)
                {
                    imagesToBeDeleted.Add(image.ImageUrl);
                    _context.ProductImages.Remove(image);
                    continue;
                }

                if (existingImage.NewImage != null)
                {
                    imagesToBeDeleted.Add(image.ImageUrl);
                    image.ImageUrl = await _imageService
                        .SaveImageAsync(existingImage.NewImage, ImageSettings.ProudctsImagesFolder);
                }
            }

            if (vm.NewImages?.Any() == true)
            {
                var newImages = await SaveProductImages(vm.NewImages);
                foreach (var img in newImages)
                    product.Images.Add(img);
            }

            return imagesToBeDeleted;
        }

        private void ValidateImages(Product product, EditProductVM vm)
        {
            var remainingImages =
                product.Images.Count - vm.ExistingImages.Count(i => i.MarkForDelete);

            if (remainingImages <= 0 && (vm.NewImages == null || !vm.NewImages.Any()))
                throw new BusinessRuleException("Product must have at least one image.");
        }


        private async Task<int> ValidateVendorAsync(string currentUserId)
        {
            if (string.IsNullOrEmpty(currentUserId))
                throw new ValidationException("Invalid data.");

            var vendorId = await _vendorQueryService.GetVendorByUserIdAsync(currentUserId);
            if (vendorId == 0)
                throw new NotFoundException("Vendor not found.");

            return vendorId;
        }

        private async Task ApplyProductUpdates(Product product, EditProductVM vm)
        {
            
            product.Name = vm.Name;
            product.Description = vm.Description;
            product.AttributeValues.Clear();
           var cOptions= _categoryAttributeOptionsRepo.GetOptionsByCatetegoryId(product.CategoryId);
            foreach(var attr in vm.FixedAttributesValues.Where(a=>a.SelectedOptionId != 0))
            {
                product.AttributeValues.Add(new ProductAttributeValue
                {
                    ProductId = product.Id,
                    CategoryAttributeId = attr.AttributeId,
                    CategoryAttributeOptionId = attr.SelectedOptionId

                });
            }
            var modelVariantIds = vm.Variants.Where(v => v.Id > 0).Select(v => v.Id.Value).ToList();
            var variantsToRemove = product.Variants.Where(v => !modelVariantIds.Contains(v.Id)).ToList();
            //var oldVariantsIds=product.Variants.ToDictionary(v=>v.Id);
            foreach (var variant in variantsToRemove)
            {
                product.Variants.Remove(variant);
            }
            foreach (var variantvm in vm.Variants)
            {
                ProductVariant variant;

                if ((variantvm.Id.HasValue && variantvm.Id > 0))
                {
                    variant = product.Variants.First(p => p.Id == variantvm.Id);
                    variant.Price = variantvm.Price;
                    variant.SKU = variantvm.SKU;
                    variant.SetStockQuantity(variantvm.StockQuantity);
                    variant.VariantValues.Clear();
                }

                else
                {
                    variant = new ProductVariant
                    {
                        ProductId = product.Id,
                        Price = variantvm.Price,
                        SKU = variantvm.SKU

                    };
                    variant.SetStockQuantity(variantvm.StockQuantity);
                    product.Variants.Add(variant);
                }
                foreach (var optionId in variantvm.SelectedOptionIds)
                {
                    variant.VariantValues.Add(
                        new ProductVariantValue
                    {   CategoryAttributeId=cOptions.First(o=>o.Id==optionId).CategoryAttributeId,
                        CategoryAttributeOptionId = optionId
                    });
                }
                await HandleVariantOfferAsync(variant, variantvm);
            }
        }

        

        private async Task ValidateCategory(int id)
        {
            if (id <= 0 || !await _categoryQueryService.CategoryIdExistsAsync(id))
                throw new ValidationException("Invalid Category Data");
     
            var category = await _categoryRepo.GetCategoryByIdAsync(id);
            if (category.SubCategories.Any())
            {
                throw new BusinessRuleException("Can not add product to category has subcategories");
            }
        }
        private async Task ValidateCommonProductRulesAsync<TBaseProductVM>(TBaseProductVM vm, int vendorId)
            where TBaseProductVM : BaseProductVM
        {
             await ValidateCategory(vm.CategoryId);

           
            if (await _productRepo.IsProductNameExistWithVendorIdAsync(
                    vm.Name,
                    vendorId,
                    vm.Id)) // Id = null في Add
            {
                throw new ValidationException("Product name already exists.");
            }
        }
        private async Task<Product> GetAndValidateProductForEditAsync(int productId, int vendorId)
        {
            var product = await _productRepo.GetProductByIdAsync(productId)
                ?? throw new NotFoundException("Product not found.");

            if (product.VendorId != vendorId)
                throw new UnauthorizedAccessException("You are not authorized to edit this product.");

            return product;
        }

        private async Task ValidateProductAttributesAsync(int categoryId, AddProductVM model)
        {
            var dbAttributes = await _categoryAttributeRepo.GetAttributesByCategoryId(categoryId);

            var selectedFixedOptions = model.FixedAttributesValues.Select(f => f.SelectedOptionId).ToList();

            var selectedVariantOptions = model.CreatedVariants
                .SelectMany(v => v.SelectedOptionIds)
                .Distinct()
                .ToList();

            foreach (var dbAttr in dbAttributes)
            {
                if (dbAttr.IsRequired)
                {
                    if (dbAttr.IsVariant)
                    {
                        var hasValue = dbAttr.Options.Any(opt => selectedVariantOptions.Contains(opt.Id));
                        if (!hasValue) throw new ValidationException($"Please select at least one value for {dbAttr.Name}.");
                    }
                    else
                    {
                        var submittedFixed = model.FixedAttributesValues.FirstOrDefault(f => f.AttributeId == dbAttr.Id);
                        if (submittedFixed == null || submittedFixed.SelectedOptionId <= 0)
                        {
                            throw new ValidationException($"{dbAttr.Name} is required.");
                        }
                    }
                }

                if (!dbAttr.IsVariant)
                {
                    var fixedVal = model.FixedAttributesValues.FirstOrDefault(f => f.AttributeId == dbAttr.Id);
                    if (fixedVal != null && !dbAttr.Options.Any(o => o.Id == fixedVal.SelectedOptionId))
                    {
                        throw new ValidationException($"Invalid value for attribute {dbAttr.Name}.");
                    }
                }
               
            }

            if (dbAttributes.Any(a => a.IsVariant) && !model.CreatedVariants.Any())
            {
                throw new ValidationException("You must generate at least one product variation.");
            }
        }
        private List<ProductAttributeValue> MapAttributeValues(int productId, List<CategoryAttributeVM> submittedAttributes)
        {
            var result = new List<ProductAttributeValue>();

            foreach (var attr in submittedAttributes)
            {
                //foreach (var val in attr.Values.Where(v => !string.IsNullOrWhiteSpace(v)))
                //{
                //    result.Add(new ProductAttributeValue
                //    {
                //        ProductId = productId,
                //        CategoryAttributeId = attr.Id,
                //        //TODO:after migration

                //        //Value = val.Trim()
                //    });
                //}
            }

            return result;
        }



    }

}

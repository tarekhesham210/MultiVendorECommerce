
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Constants;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Models;
using MultiVendorECommerce.Shared.Enums;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Areas.Admin.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryQueryService _categoryQueryService;
        private readonly ICategoryAttributeRepository _categoryAttributeRepository;
        private readonly ICategoryAttributeOptionsRepository _categoryAttributeOptionsRepository;
        private readonly IImageService _imageService;


        public CategoryService(ICategoryRepository categoryRepository, ICategoryQueryService categoryQueryService, IImageService imageService, ICategoryAttributeRepository categoryAttributeRepository, ICategoryAttributeOptionsRepository categoryAttributeOptionsRepository)
        {
            _categoryRepository = categoryRepository;
            _categoryQueryService = categoryQueryService;
            _imageService = imageService;
            _categoryAttributeRepository = categoryAttributeRepository;
            _categoryAttributeOptionsRepository = categoryAttributeOptionsRepository;
        }
        public async Task<CategoryWithAttributeVM> CreateCategory()
        {

            var parentCategories = await _categoryQueryService.GetAllCategoriesforSelection();
            CategoryWithAttributeVM categoryvm = new CategoryWithAttributeVM
            {
                ParentCategories = parentCategories
            };
            return categoryvm;
        }
        public async Task AddCategoryAsync(CategoryWithAttributeVM categoryvm)
        {
            if (await _categoryQueryService.IsCategoryNameExistAsync(categoryvm.Name))
                throw new BusinessRuleException("Category name already exists.");

            if (categoryvm.ParentCategoryId != null)
            {
                var parentCategory = await _categoryRepository.GetCategoryByIdAsync((int)categoryvm.ParentCategoryId);
                if (parentCategory == null)
                    throw new NotFoundException("Parent category not found");

                if (await _categoryRepository.IsCategoryHasProducts(parentCategory.Id))
                    throw new BusinessRuleException("Cannot add subcategory to category that has products");
            }

            var category = new Category
            {
                Name = categoryvm.Name,
                Description = categoryvm.Description,
                ParentCategoryId = categoryvm.ParentCategoryId
            };

            await _categoryRepository.AddCategoryAsync(category);

            if (categoryvm.NewImage != null)
                category.ImageUrl = await _imageService.SaveImageAsync(categoryvm.NewImage, ImageSettings.CategoriesImagesFolder);

            await _categoryRepository.SaveAsync();

            foreach (var attr in categoryvm.Attributes)
            {
                var categoryAttribute = new CategoryAttribute
                {
                    CategoryId = category.Id,
                    Name = attr.Name,            
                    IsRequired = attr.IsRequired,
                    IsVariant = attr.IsVariant,
                    
                };

                await _categoryAttributeRepository.AddAsync(categoryAttribute);
                await _categoryAttributeRepository.SaveAsync();
                var options = attr.Options
                    .Where(o => !string.IsNullOrWhiteSpace(o.Value))
                    .Select(o => new CategoryAttributeOption
                    {
                        CategoryAttributeId = categoryAttribute.Id,
                        Value = o.Value.Trim()
                    })
                    .ToList();

                    if (!options.Any())
                        throw new ValidationException($"Attribute '{attr.Name}' must have at least one option.");

                    foreach (var option in options)
                        await _categoryAttributeOptionsRepository.AddAsync(option);

                    await _categoryAttributeOptionsRepository.SaveAsync();
                
            }
        }
        public async Task<EditCategoryVM?> GetCategoryByIdForEditAsync(int id)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category is null)
            {
                throw new NotFoundException($"Category not found.");
            }
            EditCategoryVM categoryvm = new EditCategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                ParentCategoryName = category.ParentCategory?.Name,
                ParentCategories = await _categoryQueryService.GetAllCategoriesforSelection(),
                OldImage = category?.ImageUrl,
                Attributes = category.Attributes.Select(a => new CategoryAttributeVM
                {
                    Id = a.Id,
                    Name = a.Name,
                    IsVariant = a.IsVariant,                 
                    IsRequired = a.IsRequired,
                    Options = a.Options.Select(o => new CategoryAttributeOptionVM { Id = o.Id, Value = o.Value }).ToList()
                }).ToList()
            };
            return categoryvm;
        }

        public async Task UpdateCategoryAsync(int id, EditCategoryVM categoryvm)
        {
            if (await _categoryQueryService.IsCategoryNameExistAsync(categoryvm.Name, id))
                throw new BusinessRuleException("Category name already exists.");

            var category = await _categoryRepository.GetCategoryByIdAsync(id)
                ?? throw new NotFoundException("Category not found");

            if (await IsCyclicAsync(id, categoryvm.ParentCategoryId))
                throw new BusinessRuleException("Cannot set a parent that creates a cycle");

            category.Name = categoryvm.Name;
            category.Description = categoryvm.Description;
            category.ParentCategoryId = categoryvm.ParentCategoryId;

            if (categoryvm.NewImage != null)
            {
                if (!string.IsNullOrWhiteSpace(category.ImageUrl))
                    _imageService.DeleteImage(category.ImageUrl, ImageSettings.CategoriesImagesFolder);

                category.ImageUrl = await _imageService.SaveImageAsync(categoryvm.NewImage, ImageSettings.CategoriesImagesFolder);
            }

            foreach (var attr in categoryvm.Attributes)
            {
                // Delete Attribute
                if (attr.IsDeleted)
                {
                    var entity = category.Attributes.First(a => a.Id == attr.Id);
                    _categoryAttributeRepository.Delete(entity);
                    continue;
                }

                // Add New Attribute
                if ((attr.Id == null || attr.Id == 0))
                {
                    var newAttr = new CategoryAttribute
                    {
                        Name = attr.Name,
                        IsVariant = attr.IsVariant,
                        IsRequired = attr.IsRequired
                    };

                    var options = attr.Options
                        .Where(o => !string.IsNullOrWhiteSpace(o.Value))
                        .Select(o => new CategoryAttributeOption { Value = o.Value.Trim() })
                        .ToList();

                    if (!options.Any())
                        throw new ValidationException($"Attribute '{attr.Name}' must have at least one option.");

                    newAttr.Options = options;             
                    category.Attributes.Add(newAttr);
                    continue;
                }

                // Update Existing Attribute
                if (attr.Id > 0)
                {
                    var entity = category.Attributes.First(a => a.Id == attr.Id);
                    entity.Options.Clear(); // Type changed → remove old options

                    entity.Name = attr.Name;
                    entity.IsRequired = attr.IsRequired;

                        var options = attr.Options
                            .Where(o => !string.IsNullOrWhiteSpace(o.Value))
                            .Select(o => new CategoryAttributeOption
                            {
                                Id = o.Id,
                                Value = o.Value.Trim()
                            })
                            .ToList();

                        if (!options.Any())
                            throw new ValidationException($"Attribute '{attr.Name}' must have at least one option.");

                        entity.Options = options;
                    
                }
            }

            await _categoryRepository.SaveAsync();
        }

        public async Task<IEnumerable<CategoryVM>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategories()
            .Select(c => new CategoryVM
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ParentCategoryId = (int)c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory != null ? c.ParentCategory.Name : "N/A"

            }).ToListAsync();

        }

        public async Task DeleteCategoryAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalid id");

            var category = await _categoryRepository.GetCategoryByIdAsync(id);
            if (category == null)
                throw new NotFoundException("Category not found");
            var subcat = category.SubCategories;
            foreach (var sub in subcat)
            {
                sub.ParentCategoryId = null;
            }
            await _categoryRepository.SaveAsync();
            await _categoryRepository.DeleteCategoryAsync(category);

        }

        private async Task<bool> IsCyclicAsync(int categoryId, int? newParentId)
        {
            if (!newParentId.HasValue)
                return false;

            if (categoryId == newParentId.Value)
                return true; // direct cycle

            var parent = await _categoryRepository.GetCategoryByIdAsync(newParentId.Value);

            while (parent != null)
            {
                if (parent.Id == categoryId)
                    return true; // found a cycle

                if (!parent.ParentCategoryId.HasValue)
                    break;

                parent = await _categoryRepository.GetCategoryByIdAsync(parent.ParentCategoryId.Value);
            }

            return false;
        }


    }
}

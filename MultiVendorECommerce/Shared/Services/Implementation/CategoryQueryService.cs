
using Microsoft.AspNetCore.Mvc.Rendering;
using MultiVendorECommerce.Areas.Admin.ViewModels;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.Shared.Repositories.Interfaces;
using MultiVendorECommerce.Shared.Services.Interfaces;

namespace MultiVendorECommerce.Shared.Services.Implementation
{
    public class CategoryQueryService : ICategoryQueryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryQueryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<bool> CategoryIdExistsAsync(int id)
        {
            if (id <= 0)
                throw new ValidationException("Invalide category id");
          return await _categoryRepository.CategoryIdExistsAsync(id);
        }

        public async Task<IEnumerable<SelectListItem>> GetAllCategoriesforSelection()
        {
            return (await _categoryRepository.GetParentCategoriesAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

        }

        public async Task<bool> IsCategoryNameExistAsync(string name,int?currentId=null)
        {
            if (string.IsNullOrWhiteSpace(name))
               throw new ValidationException("inavalid name");
            return await _categoryRepository.CategoryNameExistsAsync(name,currentId);

        }
    }
}

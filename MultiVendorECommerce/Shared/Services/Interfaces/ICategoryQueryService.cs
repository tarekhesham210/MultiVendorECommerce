using Microsoft.AspNetCore.Mvc.Rendering;
using MultiVendorECommerce.Areas.Admin.ViewModels;

namespace MultiVendorECommerce.Shared.Services.Interfaces
{
    public interface ICategoryQueryService
    {
        Task<IEnumerable<SelectListItem>> GetAllCategoriesforSelection();
        Task<bool> IsCategoryNameExistAsync(string name, int? currentId = null);
        Task<bool> CategoryIdExistsAsync(int id);
        //Task<IEnumerable<CategoryVM>> GetParentCategoriesAsync();



    }
}

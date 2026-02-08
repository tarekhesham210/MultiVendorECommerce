using Microsoft.AspNetCore.Mvc.Rendering;
using PermissionBasedAuz.Areas.Admin.ViewModels;

namespace PermissionBasedAuz.Shared.Services.Interfaces
{
    public interface ICategoryQueryService
    {
        Task<IEnumerable<SelectListItem>> GetAllCategoriesforSelection();
        Task<bool> IsCategoryNameExistAsync(string name, int? currentId = null);
        Task<bool> CategoryIdExistsAsync(int id);
        //Task<IEnumerable<CategoryVM>> GetParentCategoriesAsync();



    }
}

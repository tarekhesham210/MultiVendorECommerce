using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuz.Areas.Customer.Services;
using PermissionBasedAuz.Areas.Customer.ViewModels;

namespace PermissionBasedAuz.Areas.Customer.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly HomeService _homeService;

        public HeaderViewComponent(HomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var action = ViewContext.RouteData.Values["action"]?.ToString();

            var isHome = string.Equals(action, "Index", StringComparison.OrdinalIgnoreCase);

            var model = new CategoriesHeaderVM
            {
                Categories = await _homeService.GetAllCategoriesForDropdownList(),
                 
            };
            //return isHome? View("_HeroSectionPartial", model):View("_NavbarPartial",model);
            return View("_NavbarPartial", model);
            
        }
    }
}

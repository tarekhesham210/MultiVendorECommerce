namespace PermissionBasedAuz.Areas.Customer.Components
{
    using global::PermissionBasedAuz.Areas.Customer.Services;
    using global::PermissionBasedAuz.Areas.Customer.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml.Linq;

    namespace PermissionBasedAuz.Areas.Customer.Components
    {
        public class SliderViewComponent : ViewComponent
        {
            private readonly HomeService _homeService;

            public SliderViewComponent(HomeService homeService)
            {
                _homeService = homeService;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                
                    var model = await _homeService.GetHotOffers(8);
                    return View("_SliderPartial", model);

            }
        }
    }

}

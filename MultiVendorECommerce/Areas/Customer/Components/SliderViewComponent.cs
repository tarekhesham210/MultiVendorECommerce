namespace MultiVendorECommerce.Areas.Customer.Components
{
    using global::MultiVendorECommerce.Areas.Customer.Services;
    using global::MultiVendorECommerce.Areas.Customer.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using System.Xml.Linq;

    namespace MultiVendorECommerce.Areas.Customer.Components
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

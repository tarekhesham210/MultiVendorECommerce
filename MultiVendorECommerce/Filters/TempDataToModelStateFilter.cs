using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace PermissionBasedAuz.Filters
{
    public class TempDataToModelStateFilter : IAsyncActionFilter
    {
        private readonly ITempDataDictionaryFactory _tempDataFactory;

        public TempDataToModelStateFilter(ITempDataDictionaryFactory tempDataFactory)
        {
            _tempDataFactory = tempDataFactory;
        }



        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);

            if (context.Controller is Controller controller)
            {
                if (tempData.TryGetValue("Error", out var error))
                {
                    controller.ModelState.AddModelError(string.Empty, error!.ToString()!);
                }

                if (tempData.TryGetValue("Success", out var success))
                {
                    controller.ViewBag.Success = success;
                }
            }

            await next();
        }
    }
}

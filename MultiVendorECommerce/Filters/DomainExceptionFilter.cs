using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using MultiVendorECommerce.Exceptions;
using MultiVendorECommerce.ViewModels;
using System.Diagnostics.Eventing.Reader;

namespace MultiVendorECommerce.Filters
{
    public class DomainExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<DomainExceptionFilter> _logger;
        private readonly ITempDataDictionaryFactory _tempDataFactory ;

        public DomainExceptionFilter(ILogger<DomainExceptionFilter> logger, ITempDataDictionaryFactory tempDataFactory)
        {
            _logger = logger;
            _tempDataFactory = tempDataFactory;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is not DomainException ex)
                return;

            _logger.LogWarning(ex, "Domain exception caught by filter: {Message}", ex.Message);

            if (IsAjaxRequest(context.HttpContext.Request))
            {
                context.Result = new JsonResult(new { success = false, message = ex.Message });
                context.HttpContext.Response.StatusCode = 200; 
            }
            else
            {
                var tempData = _tempDataFactory.GetTempData(context.HttpContext);
                tempData["Error"] = ex.Message;

                if (context.HttpContext.Request.Method == "POST")
                {
                    var returnUrl = context.HttpContext.Request.Headers["Referer"].ToString();
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        context.Result = new RedirectResult(returnUrl);
                    }
                    else
                    {
                        context.Result = new RedirectToActionResult("Index", "Home", null);
                    }
                }
                else
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }

            context.ExceptionHandled = true;
        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}

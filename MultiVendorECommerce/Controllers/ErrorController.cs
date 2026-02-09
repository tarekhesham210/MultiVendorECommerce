using Microsoft.AspNetCore.Mvc;

namespace MultiVendorECommerce.Controllers
{
    public class ErrorController : Controller
    {
        [Route("404")]
        public IActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("Error");
        }
    }
}

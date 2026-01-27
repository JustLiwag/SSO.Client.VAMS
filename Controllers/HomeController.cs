using Microsoft.AspNetCore.Mvc;

namespace SSO.Auth.Api.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/home/error")]
        public IActionResult Error(string errorId)
        {
            // Temporary error display for debugging
            return Content($"IdentityServer Error. ErrorId: {errorId}");
        }
    }
}

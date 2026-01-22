using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Controllers
{
    [Authorize] // Protects all actions unless overridden
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/Logout
        // Signs the user out of both the cookie and IdentityServer
        [HttpGet]
        public IActionResult Logout()
        {
            // This triggers the OIDC sign-out process
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("Index", "Home") // redirect after logout
                },
                CookieAuthenticationDefaults.AuthenticationScheme,
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        // GET: /Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}

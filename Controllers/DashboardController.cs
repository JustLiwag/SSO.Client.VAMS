using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;
using SSO.Client.VAMS.Services;
using System.Text.Json;
using SSO.Client.VAMS.Filters;

namespace SSO.Client.VAMS.Controllers
{
    [RequireSsoLogin]
    public class DashboardController : Controller
    {
        private readonly SsoAuthService _sso;

        public DashboardController(SsoAuthService sso)
        {
            _sso = sso;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("access_token");
            if (token == null)
                return RedirectToAction("Login", "Account");

            // Retrieve user info from session
            var userJson = HttpContext.Session.GetString("user_info");
            var userInfo = JsonSerializer.Deserialize<LoginResponseDto>(userJson ?? "{}");

            // Optionally, get claims from SSO API
            var claims = await _sso.GetUserClaimsAsync(token);

            ViewBag.UserInfo = userInfo;
            ViewBag.Claims = claims;

            return View();
        }
    }
}

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            // Triggers OIDC challenge
            return Challenge();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("oidc");

            return RedirectToAction("Index", "Home");
        }
    }
}

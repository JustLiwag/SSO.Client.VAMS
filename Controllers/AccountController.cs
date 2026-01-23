using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            // Challenge OIDC
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            }, "OpenIdConnect");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync("OpenIdConnect");
            return RedirectToAction("Login");
        }
    }
}

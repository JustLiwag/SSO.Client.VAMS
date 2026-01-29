using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers
{
    public class AccountController : Controller
    {
        // =========================
        // LOGIN
        // =========================
        public IActionResult Login(string returnUrl = "/Dashboard")
        {
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = returnUrl
            }, "oidc");
        }

        // =========================
        // LOGOUT (CRITICAL)
        // =========================
        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties
                {
                    RedirectUri = "/"
                },
                "vams_cookie",
                "oidc"
            );
        }
    }
}

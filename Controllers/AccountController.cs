using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SSO.Auth.Api.Models;

public class AccountController : Controller
{
    private readonly IIdentityServerInteractionService _interaction;

    public AccountController(IIdentityServerInteractionService interaction)
    {
        _interaction = interaction;
    }

    [HttpGet]
    public async Task<IActionResult> Logout(string logoutId)
    {
        var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

        return View(new LogoutViewModel
        {
            LogoutId = logoutId,
            PostLogoutRedirectUri = logoutContext?.PostLogoutRedirectUri
        });
    }

    public IActionResult Logout()
    {
        return SignOut(
            new AuthenticationProperties
            {
                RedirectUri = "/"
            },
            CookieAuthenticationDefaults.AuthenticationScheme,
            "oidc"
        );
    }

    [HttpPost]
    public async Task<IActionResult> LogoutConfirmed(string logoutId)
    {
        await HttpContext.SignOutAsync();

        var logoutContext = await _interaction.GetLogoutContextAsync(logoutId);

        return Redirect(logoutContext?.PostLogoutRedirectUri ?? "/");
    }


}

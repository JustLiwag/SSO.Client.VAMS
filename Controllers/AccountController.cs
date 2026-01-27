using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers;

public class AccountController : Controller
{
    public IActionResult Logout()
    {
        return SignOut("Cookies", "OpenIdConnect");
    }
}

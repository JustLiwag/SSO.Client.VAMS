using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers;

[ApiController]
public class DebugController : Controller
{
    [HttpPost("/signin-oidc")]
    public IActionResult SigninOidcHit()
    {
        return Content("SIGNIN-OIDC HIT CONTROLLER");
    }
}



using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SSO.Client.VAMS.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return Content("THIS IS THE DASHBOARD — LOGIN SUCCESSFUL");
    }
}

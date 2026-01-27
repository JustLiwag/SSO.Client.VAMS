using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var claims = User.Claims.Select(c => new UserClaimsDto
            {
                Type = c.Type,
                Value = c.Value
            }).ToList();

            return View(claims);
        }
    }
}

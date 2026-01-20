using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;
using System.Linq;

namespace SSO.Client.VAMS.Controllers
{
    [Authorize] // Only authenticated users can access
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Convert all claims to a strongly typed list
            var claims = User.Claims
                .Select(c => new ClaimViewModel
                {
                    Type = c.Type,
                    Value = c.Value
                })
                .ToList();

            // Map essential claims to view model
            var model = new DashboardViewModel
            {
                EmployeeId = User.FindFirst("employee_id")?.Value,
                FullName = User.FindFirst("full_name")?.Value,
                Division = User.FindFirst("division")?.Value,
                Claims = claims
            };

            return View(model);
        }
    }
}

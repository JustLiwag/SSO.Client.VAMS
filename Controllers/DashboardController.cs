namespace SSO.Client.VAMS.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var model = new
            {
                EmployeeNo = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                FullName = User.FindFirst(ClaimTypes.Name)?.Value,
                Division = User.FindFirst("division")?.Value,
                Claims = User.Claims.Select(c => new
                {
                    c.Type,
                    c.Value
                }).ToList()
            };

            return View(model);
        }
    }


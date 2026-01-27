using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SSO.Client.VAMS.Models;
using SSO.Client.VAMS.Services;
using SSO.Client.VAMS.Data;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace SSO.Client.VAMS.Controllers
{
    /// <summary>
    /// Dashboard controller that shows authenticated user's information.
    /// Requires an SSO login via the <see cref="RequireSsoLogin"/> filter.
    /// </summary>
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly SsoAuthService _sso;
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Creates a new <see cref="DashboardController"/>.
        /// </summary>
        /// <param name="sso">SSO service used to fetch claims.</param>
        /// <param name="db">EF Core database context used to query local user details view.</param>
        public DashboardController(SsoAuthService sso, ApplicationDbContext db)
        {
            _sso = sso;
            _db = db;
        }

        /// <summary>
        /// GET: /Dashboard/Index
        /// Loads user info from session, optionally refreshes claims from SSO API,
        /// and looks up local personnel details from the mapped view <c>vw_PersonnelDivisionDetails</c>.
        /// </summary>
        /// <returns>Dashboard view populated with SSO and local user data.</returns>
        public async Task<IActionResult> Index()
        {
            // Retrieve bearer token stored in session during login. If missing, redirect to login.
            var token = HttpContext.Session.GetString("access_token");
            if (token == null)
                return RedirectToAction("Login", "Account");

            // Retrieve and deserialize SSO-provided user info saved in session.
            var userJson = HttpContext.Session.GetString("user_info");
            var userInfo = JsonSerializer.Deserialize<LoginResponseDto>(userJson ?? "{}");

            // Optionally fetch claims from SSO API for more detailed authorization data.
            var claims = await _sso.GetUserClaimsAsync(token);

            // Query the local database view for personnel details using the EmployeeId stored in session.
            var employeeId = HttpContext.Session.GetString("EmployeeId");
            PersonnelDivisionDetail? localUser = null;
            if (!string.IsNullOrWhiteSpace(employeeId))
            {
                // Use AsNoTracking for read-only queries to improve performance.
                localUser = await _db.PersonnelDivisionDetails
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.EmployeeId == employeeId);
            }

            // Pass gathered information to the view using ViewBag to keep the view simple.
            ViewBag.UserInfo = userInfo;
            ViewBag.Claims = claims;
            ViewBag.LocalUser = localUser;

            return View();
        }
    }
}

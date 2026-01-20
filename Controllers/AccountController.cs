using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;
using SSO.Client.VAMS.Services;
using System.Text.Json;

namespace SSO.Client.VAMS.Controllers
{
    /// <summary>
    /// Controller responsible for local account interactions (login / logout).
    /// This controller delegates authentication to the external SSO API via <see cref="SsoAuthService"/>.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly SsoAuthService _sso;

        /// <summary>
        /// Creates a new instance of <see cref="AccountController"/>.
        /// </summary>
        /// <param name="sso">Injected SSO service used to call the SSO API.</param>
        public AccountController(SsoAuthService sso)
        {
            _sso = sso;
        }

        /// <summary>
        /// GET: /Account/Login
        /// Returns the login page.
        /// </summary>
        /// <returns>Login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// POST: /Account/Login
        /// Attempts to authenticate the user against the SSO API. On success stores token and user info in session.
        /// </summary>
        /// <param name="model">The login form model containing username and password.</param>
        /// <returns>Redirects to Dashboard on success or re-displays the login view on failure.</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Validate incoming model
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Call external SSO API. The service returns (responseDto, errorMessage).
            var (loginResponse, errorMessage) = await _sso.LoginAsync(model.Username, model.Password);

            if (loginResponse == null)
            {
                // Prefer API-provided error details when available.
                ViewBag.Error = string.IsNullOrWhiteSpace(errorMessage)
                    ? "Invalid credentials or inactive employee"
                    : errorMessage;

                return View(model);
            }

            // Persist minimal authenticated state in session for use by controllers/views.
            // Note: Session storage is a simple approach used here for demo purposes.
            HttpContext.Session.SetString("access_token", loginResponse.Token);
            HttpContext.Session.SetString("user_info", JsonSerializer.Serialize(loginResponse));
            HttpContext.Session.SetString("EmployeeId", loginResponse.EmployeeNo);

            // Redirect to the Dashboard page after successful login.
            return RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        /// POST: /Account/Logout
        /// Clears session and optional cookies then redirects back to the login page.
        /// </summary>
        /// <returns>Redirect to Login.</returns>
        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all in-memory session data for this user.
            HttpContext.Session.Clear();

            // If a cookie-based session or auth cookies are present, remove them as well.
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;
using SSO.Client.VAMS.Services;
using System.Text.Json;

namespace SSO.Client.VAMS.Controllers
{
    public class AccountController : Controller
    {
        private readonly SsoAuthService _sso;

        public AccountController(SsoAuthService sso)
        {
            _sso = sso;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // LoginAsync now returns a tuple with the response DTO and an error message (if any)
            var (loginResponse, errorMessage) = await _sso.LoginAsync(model.Username, model.Password);

            if (loginResponse == null)
            {
                // Prefer the API-provided error message when available; fall back to a generic message
                ViewBag.Error = string.IsNullOrWhiteSpace(errorMessage)
                    ? "Invalid credentials or inactive employee"
                    : errorMessage;

                return View(model);
            }

            // Store token and user info in session
            HttpContext.Session.SetString("access_token", loginResponse.Token);
            HttpContext.Session.SetString("user_info", JsonSerializer.Serialize(loginResponse));
            HttpContext.Session.SetString("EmployeeId", loginResponse.EmployeeNo);


            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Clear all session data
            HttpContext.Session.Clear();

            // Optional: clear authentication cookies if added later
            Response.Cookies.Delete(".AspNetCore.Session");

            return RedirectToAction("Login");
        }

    }
}

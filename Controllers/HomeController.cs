using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SSO.Client.VAMS.Models;

namespace SSO.Client.VAMS.Controllers
{
    /// <summary>
    /// Default home controller for basic pages like index and privacy.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Constructs a new instance of <see cref="HomeController"/>.
        /// </summary>
        /// <param name="logger">Logger instance provided by DI.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// GET: /Home/Index
        /// Returns the application's home page.
        /// </summary>
        /// <returns>Index view.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// GET: /Home/Privacy
        /// Returns the privacy page.
        /// </summary>
        /// <returns>Privacy view.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Error handler used by ASP.NET Core when an exception occurs and the pipeline forwards here.
        /// </summary>
        /// <returns>Error view populated with request id.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

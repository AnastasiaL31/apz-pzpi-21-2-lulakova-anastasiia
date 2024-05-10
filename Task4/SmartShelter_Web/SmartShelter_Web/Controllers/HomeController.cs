using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using System.Diagnostics;

namespace SmartShelter_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITokenService _tokenService;

        public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Backup()
        {
            return View();
        }

        public IActionResult DownloadFile()
        {

            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Backup";
            var response = client.GetAsync(fullUrl).Result;

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = response.Content.ReadAsByteArrayAsync().Result;

                var fileName = "backup.bak";

                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                return StatusCode((int)response.StatusCode, $"Error downloading file: {response.ReasonPhrase}");
            }
        }


        [HttpPost]
        public IActionResult CultureManagment(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
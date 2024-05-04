using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class AviaryController : Controller
    {
        private readonly ITokenService _tokenService;

        public AviaryController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<IActionResult> Aviaries()
        {
            var aviaries = await GetAllAviaries();
            return View( aviaries);
        }

        public async Task<List<AviaryDescription>> GetAllAviaries()
        {
            var allAviaries = new List<AviaryDescription>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/all";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                try
                {
                    allAviaries = JsonSerializer.Deserialize<List<AviaryDescription>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }

            return allAviaries;
        }

        [HttpPost]
        public async Task<IActionResult> GetAviariesToFeed(int[] selectedAviaries)
        {
            await RechargeAviaries(selectedAviaries);
            return RedirectToAction("Aviaries");
        }

        public async Task<bool> RechargeAviaries(int[] aviaries)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/aviaries/fill";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(aviaries, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}

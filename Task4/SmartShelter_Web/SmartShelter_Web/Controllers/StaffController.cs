using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Dtos;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class StaffController : Controller
    {
        private readonly ITokenService _tokenService;

        public StaffController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IActionResult> Index()
        {
            var staff = await GetAllStaff();
            return View(staff);
        }
        public async Task<IActionResult> StaffDetails(int staffId)
        {
            var staff = await GetStaffById(staffId);
            return View(staff);
        }
        public async Task<List<StaffDto>> GetAllStaff()
        {
            List<StaffDto> staff = new List<StaffDto>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/all";

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
                    staff = JsonSerializer.Deserialize<List<StaffDto>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
            return staff;
        }

        public async Task<StaffDto> GetStaffById(int staffId)
        {
            var staff = new StaffDto();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/all/{staffId}";

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
                    staff = JsonSerializer.Deserialize<StaffDto>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
            return staff;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaff(StaffDto staff)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/update";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(staff, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("StaffDetails", new { staffId = staff.Id });
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Dtos;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
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

        public async Task<StaffDetailsVM> GetStaffById(int staffId)
        {
            var staff = new StaffDetailsVM();
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
                    staff = JsonSerializer.Deserialize<StaffDetailsVM>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
            return staff;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStaff(StaffDetailsVM vm, string selectedRole)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/update";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.Staff, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                if(selectedRole != vm.Role)
                {
                    client = _tokenService.CreateHttpClient();
                    fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/addRole?roleName={selectedRole}&staffId={vm.Staff.Id}";
                    content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                    response = await client.PutAsync(fullUrl, null);
                    if(response.IsSuccessStatusCode)
                    {

                    }
                }
            }

            return RedirectToAction("StaffDetails", new { staffId = vm.Staff.Id });
        }

    }
}

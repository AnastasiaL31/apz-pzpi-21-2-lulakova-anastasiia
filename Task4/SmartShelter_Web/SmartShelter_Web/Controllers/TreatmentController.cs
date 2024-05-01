using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ITokenService _tokenService;
        public TreatmentController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
       
        public async Task<IActionResult> DiseaseTreatments(int diseaseId, bool isClosed)
        {
            var disease = await GetDisease(diseaseId);
            if (disease == null)
            {
                disease = new Disease();
            }
            var treatments = await GetDiseaseTreatments(diseaseId);
            return View( new DiseaseTreatmentsVM()
            {
                Treatments = treatments,
                Disease = disease,
                NewTreatment = new Treatment(){  
                    AnimalId = (int)disease.AnimalId, 
                    Date = DateTime.Now
                },
                isClosed = isClosed
            });
        }

        public async Task<Disease> GetDisease(int diseaseId)
        {
            var disease = new Disease();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/disease/{diseaseId}";

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
                    disease = JsonSerializer.Deserialize<Disease>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
           
            return disease;
        }

        public async Task<List<TreatmentWithStaff>> GetDiseaseTreatments(int diseaseId)
        {
            var treatments = new List<TreatmentWithStaff>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/treatment/disease/{diseaseId}";

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
                    treatments = JsonSerializer.Deserialize<List<TreatmentWithStaff>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }

            return treatments;
        }

        [HttpPost]
        public async Task<IActionResult> AddDiseaseTreatment(DiseaseTreatmentsVM vm)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/addTreatment?diseaseId={vm.Disease.Id}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.NewTreatment, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("DiseaseTreatments", new { diseaseId = vm.Disease.Id, isClosed = false });
        }
    }
}

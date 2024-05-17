using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using SmartShelter_Web.Dtos;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SmartShelter_Web.Controllers
{
    public class TreatmentController : Controller
    {
        private readonly ITokenService _tokenService;
        public TreatmentController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        
        public List<AddSupplyDto> SuppliesToAdd
        {
            get
            {
                var vm = new List<AddSupplyDto>();
                try
                {
                    string suppliesJson = TempData["addedSupplies"] as string;

                    if (!string.IsNullOrEmpty(suppliesJson))
                    {
                        vm = JsonConvert.DeserializeObject<List<AddSupplyDto>>(suppliesJson);
                    }
                }
                catch (Exception ex)
                {

                }
                return vm;
            }
            set
            {
                string suppliesJson = JsonConvert.SerializeObject(value);

                TempData["addedSupplies"] = suppliesJson;
            }
        }
        
        public async Task<IActionResult> Supplies(int treatmentId, bool isClosed)
        {
            var supplies = await GetTreatmentSupplies(treatmentId);
            return View(new SuppliesVM()
            {
                Supplies = supplies,
                TreatmentId = treatmentId,
                IsClosed = isClosed
            });
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
                isClosed = isClosed
            });
        }

        public async Task<IActionResult> CreateTreatment(int? diseaseId, int animalId)
        {
            var vm = new AddTreatmentVM()
            {
                NewTreatment = new Treatment()
                {
                    AnimalId = animalId,
                    Date = DateTime.Now
                },
                SuppliesToAdd = new List<AddSupplyDto>(),
                NewSupply = new AddSupplyDto()
            };

            vm.SuppliesToAdd = SuppliesToAdd;
            SuppliesToAdd = vm.SuppliesToAdd;
            if (diseaseId != null)
            {
                var disease = await GetDisease((int)diseaseId);
                if (disease == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                vm.Disease = disease;
            }
            return View(vm);
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
        public async Task<IActionResult> AddDiseaseTreatment(AddTreatmentVM vm)
        {
            var suppliesToAdd = SuppliesToAdd;
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/addTreatment";
            if(vm.Disease != null)
            {
                fullUrl += $"?diseaseId={vm.Disease.Id}";
            }
            if (String.IsNullOrEmpty(vm.NewTreatment.Notes))
            {
                vm.NewTreatment.Notes = " ";
            }
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.NewTreatment, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                int id = Int32.Parse(await response.Content.ReadAsStringAsync());
                if(suppliesToAdd.Count > 0)
                {
                    var res = await AddTreatmentSupplies(suppliesToAdd, id);
                }
            }

            return RedirectToAction("Details", "Animal", new { animalId = vm.NewTreatment.AnimalId });
        }

        public async Task<bool> AddTreatmentSupplies(List<AddSupplyDto> list, int treatmentId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/addSupplies?treatmentId={treatmentId}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(list, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public IActionResult AddSupply(AddTreatmentVM vm, int? diseaseId)
        {
            var addedSupplies = SuppliesToAdd;
            if (vm.NewSupply != null && addedSupplies != null)
            {
                addedSupplies.Add(vm.NewSupply);
            }
            SuppliesToAdd = addedSupplies;
            
            return RedirectToAction("CreateTreatment", new
            {
                animalId = vm.NewTreatment.AnimalId,
                diseaseId = (vm.Disease != null ? vm.Disease?.Id : null )
            });
        }

        [HttpPost]
        public IActionResult DeleteSupply(AddTreatmentVM vm)
        {
            var suppliesToAdd = SuppliesToAdd;
            suppliesToAdd.RemoveAt(vm.indexToDelete);
            SuppliesToAdd = suppliesToAdd;

            return RedirectToAction("CreateTreatment", new
            {
                animalId = vm.NewTreatment.AnimalId,
                diseaseId = (vm.Disease != null ? vm.Disease?.Id : null)
            });
        }

        public async Task<List<Supply>> GetTreatmentSupplies(int treatmentId)
        {
            var supplies = new List<Supply>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/treatment/{treatmentId}/supplies";

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
                    supplies = JsonSerializer.Deserialize<List<Supply>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }

            return supplies;
        }

        public async Task<IActionResult> DeleteTreatment(int treatmentId, int animalId) {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/treatment/delete/{treatmentId}";
            HttpResponseMessage response = await client.DeleteAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Details", "Animal", new { animalId = animalId });

        }

        public async Task<IActionResult> DeleteInsertedSupply(int supplyId, int treatmentId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/supply/delete/{supplyId}";
            HttpResponseMessage response = await client.DeleteAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {

            }
            return RedirectToAction("Supplies", new { treatmentId = treatmentId, isClosed = false });
        }
    }
}

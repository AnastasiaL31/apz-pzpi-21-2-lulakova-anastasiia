using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
using System.Collections.Generic;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class AnimalController : Controller
    {
        private readonly ITokenService _tokenService;
        public AnimalController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IActionResult> Index()
        {
            var animals = await GetAnimals();
            return View(animals);
        }


        public async Task<IActionResult> Details(int animalId)
        {
            if(animalId == 0)
            {
                return RedirectToAction("Index");
            }
            var animal = await GetAnimal(animalId);
            var meals = await GetAnimalMealPlan(animalId);
            var aviary = await GetAviary(animalId);
            var freeAviaries = await GetFreeAviaries();
            var diseases = await GetAnimalDiseases(animalId);
            var treatments = await GetTreatments(animalId);
            return View(new AnimalDetailsVM
            {
                Animal = animal,
                Meals = meals,
                NewMealPlan = new MealPlan { AnimalId = animalId },
                Aviary = aviary,
                FreeAviaries = freeAviaries,
                Diseases = diseases,
                Treatments = treatments,
                NewDisease = new Disease() { AnimalId = animalId, Symptoms="", StartDate=DateTime.Now }
            });
        }


        public async Task<Animal> GetAnimal(int animalId)
        {
            var animal = new Animal();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Animals/{animalId}";

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
                    animal = JsonSerializer.Deserialize<Animal>(result, options);
                }
                catch (Exception ex)
                {
                   
                }
            }

            return animal;
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAnimal(Animal animal)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/updateAnimal";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(animal, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("Details", new { animalId = animal.Id });
        }
        public async Task<List<Animal>> GetAnimals()
        {
            var animals = new List<Animal>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Animals";

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
                    animals = JsonSerializer.Deserialize<List<Animal>>(result, options);
                }
                catch (Exception ex)
                {
                    return animals;
                }
            }
            
            return animals;
        }

        public async Task<List<MealPlan>> GetAnimalMealPlan(int animalId)
        {
            var meals = new List<MealPlan>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/animalMeal/{animalId}";

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
                    meals = JsonSerializer.Deserialize<List<MealPlan>>(result, options);
                }
                catch (Exception ex)
                {
                    return meals;
                }
            }
            if(meals.Count > 0)
            {
                var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
                var isUtc = currentCulture.Name == "en-US";
                if (!isUtc) {
                    TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                    for (int i = 0; i < meals.Count; i++)
                    {

                        meals[i].Time = TimeZoneInfo.ConvertTimeFromUtc(meals[i].Time, localTimeZone);
                    }
                }
            }
            return meals;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMealplan(AnimalDetailsVM vm)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            var isUtc = currentCulture.Name == "en-US";
            if (!isUtc)
            {
                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                for (int i = 0; i < vm.Meals.Count; i++)
                {
                    vm.Meals[i].Time = TimeZoneInfo.ConvertTimeToUtc(vm.Meals[i].Time, localTimeZone);
                }
            }
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/updateMealPlan/group";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(
                vm.Meals, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("Details", new { animalId = vm.Animal.Id });
        }

        public async Task<IActionResult> DeleteMeal(int mealId, int animalId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Meals/?id={mealId}";
            HttpResponseMessage response = await client.DeleteAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Details", new { animalId = animalId });
        }

        [HttpPost]
        public async Task<IActionResult> AddMeal(AnimalDetailsVM vm)
        {
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            var isUtc = currentCulture.Name == "en-US";
            if (!isUtc)
            {
                TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
                vm.NewMealPlan.Time = TimeZoneInfo.ConvertTimeToUtc(vm.NewMealPlan.Time, localTimeZone);
            }
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/addMealPlan";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.NewMealPlan, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Details", new { animalId = vm.NewMealPlan.AnimalId });
            
        }


        public async Task<Aviary> GetAviary(int animalId)
        {
            var aviary = new Aviary();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/aviary/{animalId}";

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
                    aviary = JsonSerializer.Deserialize<Aviary>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
            aviary.AnimalId = animalId;
            if (String.IsNullOrEmpty(aviary.Description))
            {
                aviary.Description = " ";
            }
            return aviary;
        }

        public async Task<List<int>> GetFreeAviaries()
        {
            var freeAviaries = new List<int>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/aviary/free";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
             
                try
                {
                    freeAviaries = JsonSerializer.Deserialize<List<int>>(result);
                }
                catch (Exception ex)
                {

                }
            }

            return freeAviaries;
        }

        [HttpPost]
        public async Task<IActionResult> ChangeAviary(string selectedAviary, int animalId)
        {

            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/change?animalId={animalId}&newAviaryId={selectedAviary}";
            HttpResponseMessage response = await client.PutAsync(fullUrl, null);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Details", new { animalId = animalId });

        }

        [HttpPost]
        public async Task<IActionResult> UpdateAviary(AnimalDetailsVM vm)
        {
            if(vm.Aviary.AviaryConditionId == 0)
            {
               vm.Aviary.AviaryConditionId = await AddAviaryCodition(vm.Aviary.AviaryCondition, vm.Aviary.Id);
            }
            else
            {
                vm.Aviary.AviaryCondition.Id = (int)vm.Aviary.AviaryConditionId;

            }
            if(String.IsNullOrEmpty(vm.Aviary.Description)) {
                vm.Aviary.Description = " ";
            }
            await UpdateAviary(vm.Aviary);
            return RedirectToAction("Details", new { animalId = vm.Aviary.AnimalId });
        }

        public async Task<int> AddAviaryCodition(AviaryCondition condition, int aviaryId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/add/condition?aviaryId={aviaryId}";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(condition, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return Int32.Parse(await response.Content.ReadAsStringAsync());
            }
            return 0;
        }

        public async Task<IActionResult> UpdateAviary(Aviary aviary)
        {
            aviary.AviaryCondition = null;
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/update";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(aviary, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("Details", new { animalId = aviary.AnimalId });
        }

        public async Task<List<Disease>> GetAnimalDiseases(int animalId)
        {
            var diseases = new List<Disease>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/animal/{animalId}/diseases";

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
                    diseases = JsonSerializer.Deserialize<List<Disease>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }
            
            return diseases;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDiseases(AnimalDetailsVM vm)
        {
            vm.Diseases.RemoveAll(x => x.Name == null);
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/updateDisease/group";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(
                vm.Diseases, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("Details", new { animalId = vm.Animal.Id });
        }

        [HttpPost]
        public async Task<IActionResult> AddDisease(AnimalDetailsVM vm)
        {

            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/addDisease";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.NewDisease, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Details", new { animalId = vm.NewDisease.AnimalId });

        }


        public async Task<List<TreatmentWithStaff>> GetTreatments(int animalId)
        {
            var treatments = new List<TreatmentWithStaff>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/treatments/{animalId}";

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
    }
}

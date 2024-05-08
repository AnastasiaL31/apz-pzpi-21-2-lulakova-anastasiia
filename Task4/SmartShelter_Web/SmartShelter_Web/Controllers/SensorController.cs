using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Dtos;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class SensorController : Controller
    {
        private readonly ITokenService _tokenService;
        public SensorController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IActionResult> Index(int aviaryId, int animalId)
        {
            SensorWithDataVM vm = new SensorWithDataVM();
            vm.Sensor = await GetSensor(aviaryId);
            if(vm.Sensor.Id != 0)
            {
                vm.SensorData = await GetSensorData(vm.Sensor.Id);
            }
            else
            {
                vm.SensorData = new List<SensorData>();
            }
            vm.AnimalId = animalId;
            return View(vm);
        }

        public async Task<Sensor> GetSensor(int aviaryId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/sensor/{aviaryId}";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                Sensor sensor = JsonSerializer.Deserialize<Sensor>(result, options);
                if (sensor != null)
                {
                    return sensor;
                }
            }

            return new Sensor() { Id = 0, AviaryId = aviaryId};
        }
        public async Task<IActionResult> AddSensor(int aviaryId, int animalId)
        {
            AddSensorDto sensor = new AddSensorDto() { AviaryId = aviaryId}; 
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/add/sensor";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(sensor, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Index", new { animalId = animalId, aviaryId = aviaryId });

        }

        public async Task<List<SensorData>> GetSensorData(int sensorId)
        {
            List<SensorData> list = new List<SensorData>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/sensordata/{sensorId}";

            HttpResponseMessage response = await client.GetAsync(fullUrl);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                list = JsonSerializer.Deserialize<List<SensorData>>(result, options);
            }

            return list;
        }


        public async Task<IActionResult> UpdateSensorSettings(SensorWithDataVM vm)
        {
            var client = _tokenService.CreateHttpClient();
            vm.Sensor.Frequency *= 60000;
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Aviary/update/sensor";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(
               vm.Sensor, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(fullUrl, content);

            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }

            return RedirectToAction("Index", new { animalId = vm.AnimalId, aviaryId = vm.Sensor.AviaryId });
        
        }

        public async Task<IActionResult> DeleteSensor(int aviaryId, int sensorId, int animalId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/sensor/{sensorId}";
            HttpResponseMessage response = await client.DeleteAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Index", new { animalId = animalId, aviaryId = aviaryId });

        }
    }
}

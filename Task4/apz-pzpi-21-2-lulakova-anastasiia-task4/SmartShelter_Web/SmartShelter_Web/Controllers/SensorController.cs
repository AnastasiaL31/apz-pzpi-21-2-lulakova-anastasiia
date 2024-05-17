using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using PlantCare_Web.Models.ViewModels;
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


        public async Task<IActionResult> Diagrams(int sensorId)
        {
            if (sensorId != 0)
            {
                TempData["SId"] = sensorId;
            }
            else
            {
                sensorId = (int)TempData["SId"];
            }
            TempData["SId"] = sensorId;


            Sensor sensor = new Sensor() { Id = sensorId };
            var sensorData = await GetSensorData(sensorId);
            SensorDataViewModel model = new SensorDataViewModel()
            {
                Temperatures = new List<float>(),
                Humidities = new List<float>(),
                DewPoints = new List<float>(),
                AbsHumidities = new List<float>(),
                Dates = new List<DateTime>()
            };
            foreach (var data in sensorData)
            {
                model.Temperatures.Add(data.Temperature);
                model.Humidities.Add(data.Humidity);
                //model.DewPoints.Add(data.DewPoint);
                //model.AbsHumidities.Add(data.AbsoluteHumidity);
                model.Dates.Add(data.Date);
            }
            var currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            var isCelsius = currentCulture.Name != "en-US";
            List<LineSeriesData> tempData = new List<LineSeriesData>();
            List<LineSeriesData> humidData = new List<LineSeriesData>();
            //List<LineSeriesData> dewpointData = new List<LineSeriesData>();
            //List<LineSeriesData> absHumData = new List<LineSeriesData>();
            if (!isCelsius)
            {
                model.Temperatures.ForEach(p => tempData.Add(new LineSeriesData
                {
                    Y = Math.Round(p * 1.8 + 32, 2)
                }));

                //model.DewPoints.ForEach(p => dewpointData.Add(new LineSeriesData
                //{
                //    Y = Math.Round(p * 1.8 + 32, 2)
                //}));
            }
            else
            {
                model.Temperatures.ForEach(p => tempData.Add(new LineSeriesData
                {
                    Y = p
                }));

                //model.DewPoints.ForEach(p => dewpointData.Add(new LineSeriesData
                //{
                //    Y = p
                //}));
            }

            model.Humidities.ForEach(p => humidData.Add(new LineSeriesData
            {
                Y = p
            }));
            //model.AbsHumidities.ForEach(p => absHumData.Add(new LineSeriesData
            //{
            //    Y = p
            //}));
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
            List<string> chDates = new List<string>();
            if (isCelsius)
            {
                for (int i = 0; i < model.Dates.Count(); i++)
                {
                    model.Dates[i] = TimeZoneInfo.ConvertTimeFromUtc(model.Dates[i], localTimeZone);
                }
                chDates.Add(model.Dates[0].ToString("dd.MM.y HH:mm"));
            }
            else
            {
                chDates.Add(model.Dates[0].ToString("y.MM.dd HH:mm"));
            }


            for (int i = 1; i < model.Dates.Count(); i++)
            {
                if (model.Dates[i].DayOfYear == model.Dates[i - 1].DayOfYear)
                {
                    chDates.Add(model.Dates[i].ToString("t"));
                }
                else
                {
                    if (!isCelsius)
                    {
                        chDates.Add(model.Dates[i].ToString("y.MM.dd HH:mm"));
                    }
                    else
                    {
                        chDates.Add(model.Dates[i].ToString("dd.MM.y HH:mm"));
                    }
                }
            }

            ViewData["tempData"] = tempData;
            ViewData["humidData"] = humidData;
            //ViewData["dewpointData"] = dewpointData;
            //ViewData["absHumData"] = absHumData;
            ViewData["dates"] = chDates;
            ViewData["sensorId"] = sensorId;
            //ViewData["plantName"] = plantName;
            return View();
        }

    }
}

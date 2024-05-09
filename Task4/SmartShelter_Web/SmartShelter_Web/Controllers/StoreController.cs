using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShelter_Web.Middleware;
using SmartShelter_Web.Models;
using SmartShelter_Web.Models.ViewModel;
using System.Text.Json;

namespace SmartShelter_Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly ITokenService _tokenService;
        public StoreController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await GetFullStorage();
            var groupedList = GetGroupedStorage(list);
            return View(new StorageVM (){ FullList = list, GroupedList = groupedList, NewOrder = new Dtos.AddOrderDto() });
        }

        public async Task<List<Storage>> GetFullStorage()
        {
            var list = new List<Storage>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Storage/all";

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
                    list = JsonSerializer.Deserialize<List<Storage>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }

            return list;

        }


        public List<Storage> GetGroupedStorage(List<Storage> list)
        {
            List<Storage> storage = new List<Storage>();
            foreach (var item in list)
            {
                if (storage.FirstOrDefault(x => x.Name == item.Name) != null)
                {
                    int ind = storage.FindIndex(x => x.Name == item.Name);
                    if (ind != -1)
                    {
                        storage[ind].Amount += item.Amount;
                        storage[ind].Price += item.Price;
                    }
                }
                else { storage.Add(item); }
            }
            return storage;
        }


        public async Task<IActionResult> CreateOrder(StorageVM vm)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Storage/order/add";
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(vm.NewOrder, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(fullUrl, content);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Index");
        }
    }
}

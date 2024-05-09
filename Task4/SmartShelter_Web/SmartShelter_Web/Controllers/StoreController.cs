using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShelter_Web.Dtos;
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
            var orders = await GetAllOrders();
            orders.Sort(
                (a, b) =>
                {
                    return a.IsApproved != b.IsApproved ? 1 : 0;
                });
            return View(new StorageVM (){ 
                FullList = list, 
                GroupedList = groupedList, 
                NewOrder = new AddOrderDto(),
                Orders = orders
            });
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
            for (int i = 0; i < list.Count; i++)
            {
                if (storage.FirstOrDefault(x => x.Name.ToLower() == list[i].Name.ToLower()) != null)
                {
                    int ind = storage.FindIndex(x => x.Name.ToLower() == list[i].Name.ToLower());
                    if (ind != -1)
                    {
                        storage[ind].Amount += list[i].Amount;
                        storage[ind].Price += list[i].Price;
                    }
                }
                else { 
                    storage.Add(new Storage()
                    {
                        Id = list[i].Id, Amount = list[i].Amount, Price = list[i].Price, 
                        Date = list[i].Date,  Name = list[i].Name, StaffId = list[i].StaffId, 
                        Type = list[i].Type, UnitOfMeasure = list[i].UnitOfMeasure

                    }); 
                }
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

        public async Task<List<OrderDto>> GetAllOrders()
        {
            var list = new List<OrderDto>();
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Storage/orders/all";

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
                    list = JsonSerializer.Deserialize<List<OrderDto>>(result, options);
                }
                catch (Exception ex)
                {

                }
            }

            return list;

        }

        public async Task<IActionResult> ApproveOrder(int orderId)
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Storage/approve/{orderId}";
            HttpResponseMessage response = await client.PostAsync(fullUrl, null);
            if (response.IsSuccessStatusCode)
            {
                //return RedirectToAction("Details", animal.Id);
            }
            return RedirectToAction("Index");
        }
    }
}

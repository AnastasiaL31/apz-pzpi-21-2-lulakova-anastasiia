using Microsoft.AspNetCore.Mvc;
using SmartShelter_Web.Models.ViewModel;
using LoginModel = SmartShelter_Web.Models.ViewModel.LoginModel;

using SmartShelter_Web.Models.ViewModel;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using SmartShelter_Web.Models;
using Microsoft.Extensions.Options;
using NuGet.Common;
using SmartShelter_Web.Middleware;
using Microsoft.IdentityModel.Tokens;
using SmartShelter_Web.Dtos;
using Microsoft.AspNetCore.Http;

namespace SmartShelter_Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly ITokenService _tokenService;

        public AccountController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        public IActionResult Register()
        {
            return View(new LoginModel());
        }

        public IActionResult UserData(string username)
        {
            return View(new LoginModel() { Username = username, NewStaff = new AddStaffDto() });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            string res = await GetUser(model);
            if (!String.IsNullOrEmpty(res))
            {
                return View(model);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginModel model)
        {
           
            var res = await RegisterUser(model);
            if (!String.IsNullOrEmpty(res))
            {
                return View(model);
            }
             
            return RedirectToAction("UserData", new {username = model.Username});
        }

        [HttpPost]
        public async Task<IActionResult> AddUserData(LoginModel model)
        {
            var result = await AddStaff(model.NewStaff, model.Username);
            if (!result)
            {
                return RedirectToAction("UserData", model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<string> GetUser(LoginModel model)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = String.Empty;
            using (HttpClient client = new HttpClient())
            {
                string fullUrl = $"{GlobalVariables.backendAddress}/api/Auth/Login";

                HttpResponseMessage response = await client.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    try
                    {
                        UserData user = JsonSerializer.Deserialize<UserData>(result, options);
                        if (user != null)
                        {
                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true
                            };

                            Response.Cookies.Append("token", user.token);
                            Response.Cookies.Append("role", user.role, cookieOptions);
                            GlobalVariables.role = user.role;
                        }
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            error = "BadRequest";
                            break;
                        case HttpStatusCode.InternalServerError:
                            error = "InternalServerError";
                            break;
                        case HttpStatusCode.NotFound:
                            error = "NoObject";
                            break;
                        case HttpStatusCode.UnprocessableEntity:
                            error = "NoUser";
                            break;
                        default:
                            error = "Default";
                            break;
                    }
                }

                return error;
            }
        }

        public async Task<string> RegisterUser(LoginModel model)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(model, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = String.Empty;
            using (HttpClient client = new HttpClient())
            {
                string fullUrl = $"{GlobalVariables.backendAddress}/api/Auth/Register";
        
                HttpResponseMessage response = await client.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    if(result == "true")
                    {
                        error = await GetUser(model);
                    }
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.BadRequest:
                            error = "BadRequest";
                            break;
                        case HttpStatusCode.InternalServerError:
                            error = "InternalServerError";
                            break;
                        case HttpStatusCode.NotFound:
                            error = "NoObject";
                            break;
                        case HttpStatusCode.UnprocessableEntity:
                            error = "NoUser";
                            break;
                        default:
                            error = "Default";
                            break;
                    }
                }
                return error;
            }
        }

        public async Task<bool> AddStaff(AddStaffDto staff, string email)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            string json = JsonSerializer.Serialize(staff, options);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            string error = String.Empty;
            using (HttpClient client = new HttpClient())
            {
                string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/add?email={email}";

                HttpResponseMessage response = await client.PostAsync(fullUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        public IActionResult Logout()
        {
            Response.Cookies.Append("token", "");
            Response.Cookies.Append("role", "");
            GlobalVariables.role = String.Empty;
            return RedirectToAction("Login", "Account");
        }
        public async void GetStaff()
        {
            var client = _tokenService.CreateHttpClient();
            string fullUrl = $"{GlobalVariables.backendAddress}/api/Staff/all";
            HttpResponseMessage response = await client.GetAsync(fullUrl);
            if (response.IsSuccessStatusCode)
            {

            }
        }

    }
}

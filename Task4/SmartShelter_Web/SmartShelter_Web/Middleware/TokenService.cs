using System.Net.Http.Headers;
using Azure;
using Azure.Core;

namespace SmartShelter_Web.Middleware
{
    public class TokenService:ITokenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetToken()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                return context.Request.Cookies["token"];
            }
            
            return null;
            
        }

        public HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient();
            var token = GetToken();
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer", token);
            }

            return client;

        }

    }
}

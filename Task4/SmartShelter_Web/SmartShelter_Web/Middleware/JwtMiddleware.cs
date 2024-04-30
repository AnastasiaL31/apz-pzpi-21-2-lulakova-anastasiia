namespace SmartShelter_Web.Middleware
{
    public class JwtMiddleware:IMiddleware
    {
        
        private readonly ITokenService _tokenService; 

        public JwtMiddleware(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = _tokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {

                context.Request.Headers.Add("Authorization", "Bearer " + token);
            }

            await next(context);
            context.Request.Headers.Add("Authorization", "Bearer " + token);
        }
    }
}

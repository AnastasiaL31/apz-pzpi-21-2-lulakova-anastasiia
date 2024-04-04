using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmartShelter_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<bool> Register(LoginUser user)
        {
            return await _authService.RegisterUser(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUser user)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(await _authService.LoginUser(user))
            {
                var tokenString = await _authService.GenerateToken(user);
                return Ok(tokenString);
            }

            return BadRequest();
        }
    }
}

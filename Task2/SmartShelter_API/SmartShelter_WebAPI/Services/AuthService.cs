using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace SmartShelter_WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<bool> RegisterUser(LoginUser user)
        {
            var identityUser = new IdentityUser
            {
                UserName = user.Username,
                Email = user.Username
            };
            
           var result = await  _userManager.CreateAsync(identityUser, user.Password);
           //var res = await _roleManager.CreateAsync(new IdentityRole("Admin"));
           
           return result.Succeeded;
        }

        public async Task<bool> LoginUser(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return false;
            }
            //var res = await _userManager.AddToRoleAsync(identityUser, "Admin");
            return await _userManager.CheckPasswordAsync(identityUser, user.Password);
        }


        private async Task<List<string>> GetUserRole(LoginUser user)
        {
            var identityUser = await _userManager.FindByEmailAsync(user.Username);
            if (identityUser == null)
            {
                return new List<string>();
            }

            var roles = await _userManager.GetRolesAsync(identityUser);
            return roles as List<string>;
        }

        public async Task<string> GenerateToken(LoginUser user)
        {
            var roles = await GetUserRole(user);
            if (!roles.Any())
            {
                roles.Add("Guest");
            }
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Username),
                new Claim(ClaimTypes.Role, roles[0]),
            };

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            SigningCredentials signCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            var securityToken = new JwtSecurityToken(
                claims:claims,
                expires:DateTime.Now.AddHours(1),
                issuer:_configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials:signCred
                );
            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return token;
        }
    }
}

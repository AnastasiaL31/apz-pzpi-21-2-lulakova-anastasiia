namespace SmartShelter_WebAPI.Interfaces
{
    public interface IAuthService
    {
         Task<bool> RegisterUser(LoginUser user);
         Task<bool> LoginUser(LoginUser user);
         Task<string> GenerateToken(LoginUser user);
    }
}
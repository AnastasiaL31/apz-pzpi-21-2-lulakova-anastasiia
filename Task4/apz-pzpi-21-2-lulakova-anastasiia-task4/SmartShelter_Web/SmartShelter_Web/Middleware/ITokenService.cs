namespace SmartShelter_Web.Middleware
{
    public interface ITokenService
    {
        string? GetToken();
        HttpClient CreateHttpClient();
    }
}
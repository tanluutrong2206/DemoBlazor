namespace DemoBlazor.Server.Services.Authorize
{
    public interface IJwtTokenService
    {
        string BuildToken(string email);
    }
}

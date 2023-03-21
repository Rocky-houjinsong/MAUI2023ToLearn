namespace HelloWorld.Services;

public interface ITokenService
{
    Task<string> GetTokenAsync();
}
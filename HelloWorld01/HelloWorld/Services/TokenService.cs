namespace HelloWorld.Services
{
    public class TokenService : ITokenService
    {
        public async Task<string> GetTokenAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://v2.jinrishici.com/token");
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }
    }
}
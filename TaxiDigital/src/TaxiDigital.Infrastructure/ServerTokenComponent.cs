using IdentityModel.Client;

namespace TaxiDigital.Infrastructure;

internal static class ServerTokenComponent
{
    private static TokenResponse Token { get; set; }
    private static DateTime ExpiryTime { get; set; }
    private static string URLAuthServer { get; set; }

    public static async Task<TokenResponse> GetTokenAsync()
    {
        //use token if it exists and is still fresh
        if (Token != null)
        {
            if (ExpiryTime > DateTime.UtcNow)
            {
                return Token;
            }
        }
        URLAuthServer = Environment.GetEnvironmentVariable("authserver");

        var client = new HttpClient();
        var disco = await client.GetDiscoveryDocumentAsync(URLAuthServer);

        if (disco.IsError)
        {
            throw new Exception($"Could not retrieve token.");
        }

        // request token
        var tokenRequest = new ClientCredentialsTokenRequest
        {
            Address = disco.TokenEndpoint,
            ClientId = "wMobileAPI",
            ClientSecret = "secret",
            Scope = "wDriverDataAPI wExpoDataAPI wBankService"
        };
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(tokenRequest);

        if (tokenResponse.IsError)
        {
            throw new Exception($"Could not retrieve token.");
        }

        //set Token to the new token and set the expiry time to the new expiry time
        Token = tokenResponse;
        ExpiryTime = DateTime.UtcNow.AddSeconds(Token.ExpiresIn);

        //return fresh token
        return Token;
    }
}

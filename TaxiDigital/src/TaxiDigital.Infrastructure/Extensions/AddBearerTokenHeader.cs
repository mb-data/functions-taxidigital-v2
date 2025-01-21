using System.Net.Http.Headers;

namespace TaxiDigital.Infrastructure.Extensions;

public sealed class AddBearerTokenHeader : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await ServerTokenComponent.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}

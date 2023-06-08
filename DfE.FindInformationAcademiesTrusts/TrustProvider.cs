using System.Text.Json;
using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<string>> GetTrustsAsync();
}

public class TrustProvider : ITrustProvider
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<AcademiesApiOptions> _academiesApiOptions;

    public TrustProvider(IHttpClientFactory httpClientFactory, IOptions<AcademiesApiOptions> academiesApiOptions)
    {
        _httpClientFactory = httpClientFactory;
        _academiesApiOptions = academiesApiOptions;
    }

    public async Task<IEnumerable<string>> GetTrustsAsync()
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            _academiesApiOptions.Value.Endpoint! + "/v2/trusts")
        {
            Headers = { { "ApiKey", _academiesApiOptions.Value.Key } }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<IEnumerable<string>>(contentStream);
            if (json != null) return json;
            throw new Exception();
        }

        throw new NotImplementedException();
    }
}

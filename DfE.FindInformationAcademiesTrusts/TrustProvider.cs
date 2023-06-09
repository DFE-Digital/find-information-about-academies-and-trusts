using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;
using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<Trust>> GetTrustsAsync();
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

    public async Task<IEnumerable<Trust>> GetTrustsAsync()
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"{_academiesApiOptions.Value.Endpoint!}/v2/trusts/bulk")
        {
            Headers = { { "ApiKey", _academiesApiOptions.Value.Key } }
        };

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<ApiResponseV2<TrustResponse>>(contentStream);
            if (json?.Data != null)
            {
                var transformedData = json.Data.Where(t => t.GiasData?.GroupName != null)
                    .Select(t => new Trust(t.GiasData!.GroupName!));
                return transformedData;
            }

            throw new Exception();
        }

        throw new ApplicationException("Problem communicating with Academies API");
    }
}

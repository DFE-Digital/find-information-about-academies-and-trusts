using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<Trust>> GetTrustsAsync();
}

public class TrustProvider : ITrustProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TrustProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<Trust>> GetTrustsAsync()
    {
        var httpClient = _httpClientFactory.CreateClient("AcademiesApi");

        var httpResponseMessage = await httpClient.GetAsync("v2/trusts");
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<ApiResponseV2<TrustSummaryResponse>>(contentStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (json?.Data != null)
            {
                var transformedData = json.Data.Where(t => t.GroupName != null)
                    .Select(t => new Trust(t.GroupName!));
                return transformedData;
            }

            throw new Exception();
        }

        throw new ApplicationException("Problem communicating with Academies API");
    }
}

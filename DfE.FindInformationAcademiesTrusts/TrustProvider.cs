using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<Trust>> GetTrustsAsync();

    public Task<IEnumerable<Trust>> GetTrustsByNameAsync(string name);
    public Task<Trust> GetTrustByUkprnAsync(string ukprn);
}

public class TrustProvider : ITrustProvider
{
    private readonly ILogger<ITrustProvider> _logger;
    private readonly HttpClient _httpClient;

    public TrustProvider(IHttpClientFactory httpClientFactory,
        ILogger<ITrustProvider> logger)

    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("AcademiesApi");
    }

    public async Task<IEnumerable<Trust>> GetTrustsAsync()
    {
        var requestUri = "v3/trusts";

        return await FetchTrustsAsync(requestUri);
    }

    public async Task<IEnumerable<Trust>> GetTrustsByNameAsync(string name)
    {
        var requestUri = $"v3/trusts?groupName={name}";

        return await FetchTrustsAsync(requestUri);
    }

    private async Task<IEnumerable<Trust>> FetchTrustsAsync(string requestUri)
    {
        var httpResponseMessage = await _httpClient.GetAsync(requestUri);
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<ApiResponseV3<TrustSummaryResponse>>(contentStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (json?.Data == null) throw new JsonException();
            var transformedData = json.Data
                .Where(t => t.GroupName != null)
                .Select(t => new Trust(
                    t.GroupName!,
                    TrustAddressAsString(t.TrustAddress),
                    t.Ukprn!,
                    t.Establishments != null ? t.Establishments!.Count() : 0
                ));
            return transformedData;
        }

        var errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
        LogHttpError(httpResponseMessage, errorMessage);
        throw new HttpRequestException("Problem communicating with Academies API");
    }

    public async Task<Trust> GetTrustByUkprnAsync(string ukprn)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"v3/trust/{ukprn}");
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<ApiSingleResponseV3<TrustResponse>>(contentStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (json?.Data == null || json.Data?.GiasData?.GroupName == null) throw new JsonException();

            if (json.Data?.GiasData?.GroupName != null)
            {
                var trust = new Trust(
                    json.Data.GiasData.GroupName,
                    TrustAddressAsString(json.Data.GiasData.GroupContactAddress),
                    ukprn,
                    json.Data.Establishments != null ? json.Data.Establishments!.Count() : 0);

                return trust;
            }
        }

        var errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
        LogHttpError(httpResponseMessage, errorMessage);
        throw new HttpRequestException("Problem communicating with Academies API");
    }

    private void LogHttpError(HttpResponseMessage httpResponseMessage, string errorMessage)
    {
        _logger.LogError(
            "Received {statusCode} from Academies API, \r\nendpoint: {endpoint}, \r\ncontent: {errorMessage}, \r\nheaders: {headers}",
            httpResponseMessage.StatusCode,
            httpResponseMessage.RequestMessage?.RequestUri,
            errorMessage,
            httpResponseMessage.Headers
        );
    }


    private string TrustAddressAsString(AddressResponse? addressResponse)
    {
        var address = "";

        if (addressResponse != null)
        {
            address += !string.IsNullOrEmpty(addressResponse.Street) ? $"{addressResponse.Street}, " : "";
            address += !string.IsNullOrEmpty(addressResponse.Locality) ? $"{addressResponse.Locality}, " : "";
            address += !string.IsNullOrEmpty(addressResponse.AdditionalLine)
                ? $"{addressResponse.AdditionalLine}, "
                : "";
            address += !string.IsNullOrEmpty(addressResponse.Town) ? $"{addressResponse.Town}, " : "";
            address += !string.IsNullOrEmpty(addressResponse.Postcode) ? $"{addressResponse.Postcode}" : "";
        }

        return address.TrimEnd().TrimEnd(',');
    }
}

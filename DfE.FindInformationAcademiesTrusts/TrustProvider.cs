using System.Net;
using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;
using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<TrustSearchEntry>> GetTrustsAsync();
    public Task<IEnumerable<TrustSearchEntry>> GetTrustsByNameAsync(string name);
    public Task<Trust?> GetTrustByUkprnAsync(string ukprn);
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

    public async Task<IEnumerable<TrustSearchEntry>> GetTrustsAsync()
    {
        var requestUri = "v3/trusts";

        return await FetchTrustsAsync(requestUri);
    }

    public async Task<IEnumerable<TrustSearchEntry>> GetTrustsByNameAsync(string name)
    {
        var requestUri = $"v3/trusts?groupName={name}";

        return await FetchTrustsAsync(requestUri);
    }

    private async Task<IEnumerable<TrustSearchEntry>> FetchTrustsAsync(string requestUri)
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
                .Select(t => new TrustSearchEntry(
                    t.GroupName!,
                    TrustAddressAsString(t.TrustAddress),
                    t.Ukprn,
                    t.Establishments?.Count ?? 0
                ));
            return transformedData.OrderBy(t => t.Name);
        }

        var errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();
        LogHttpError(httpResponseMessage, errorMessage);
        throw new HttpRequestException("Problem communicating with Academies API");
    }

    public async Task<Trust?> GetTrustByUkprnAsync(string ukprn)
    {
        var httpResponseMessage = await _httpClient.GetAsync($"v3/trust/{ukprn}");
        if (httpResponseMessage.IsSuccessStatusCode)
        {
            await using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
            var json = await JsonSerializer.DeserializeAsync<ApiSingleResponseV3<TrustResponse>>(contentStream,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (json?.Data?.GiasData?.GroupName == null) throw new JsonException();

            var trust = new Trust(
                json.Data.GiasData.GroupName,
                json.Data.GiasData.Ukprn,
                json.Data.GiasData.GroupType ?? string.Empty
            );

            return trust;
        }

        if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
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

    private static string TrustAddressAsString(AddressResponse? addressResponse)
    {
        if (addressResponse == null) return string.Empty;
        return string.Join(", ", new[]
        {
            addressResponse.Street,
            addressResponse.Locality,
            addressResponse.AdditionalLine,
            addressResponse.Town,
            addressResponse.Postcode
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}

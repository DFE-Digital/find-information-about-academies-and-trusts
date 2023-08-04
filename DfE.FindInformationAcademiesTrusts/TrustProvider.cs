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
    private readonly ILogger<ITrustProvider> _logger;

    public TrustProvider(IHttpClientFactory httpClientFactory,
        ILogger<ITrustProvider> logger)

    {
        _logger = logger;
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

            if (json?.Data == null) throw new JsonException();
            var transformedData = json.Data
                .Where(t => t.GroupName != null)
                .Select(t => new Trust(
                    t.GroupName!,
                    TrustAddressAsString(t.TrustAddress)
                ));
            return transformedData;
        }

        var errorMessage = await httpResponseMessage.Content.ReadAsStringAsync();

        _logger.LogError(
            "Received {statusCode} from Academies API, \r\nendpoint: {endpoint}, \r\ncontent: {errorMessage}, \r\nheaders: {headers}",
            httpResponseMessage.StatusCode,
            httpResponseMessage.RequestMessage?.RequestUri,
            errorMessage,
            httpResponseMessage.Headers
        );
        throw new HttpRequestException("Problem communicating with Academies API");
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

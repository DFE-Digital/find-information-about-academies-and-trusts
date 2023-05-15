using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts;

public class AcademiesApi
{
    private readonly IOptions<AcademiesApiOptions> _academiesApiOptions;
    private readonly HttpClient _httpClient;

    public AcademiesApi(IOptions<AcademiesApiOptions> academiesApiOptions)
    {
        _academiesApiOptions = academiesApiOptions;
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(_academiesApiOptions.Value.Endpoint!);
        _httpClient.DefaultRequestHeaders.Add("ApiKey", _academiesApiOptions.Value.Key);
    }

    public async Task<string> GetTrusts()
    {
        var response = await _httpClient.GetAsync("/v2/trusts");
        var responseMessage = await response.Content.ReadAsStringAsync();
        return responseMessage;
    }
}

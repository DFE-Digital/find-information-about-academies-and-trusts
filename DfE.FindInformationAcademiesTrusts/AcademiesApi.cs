using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts;

public interface IAcademiesApi
{
    Task<string> GetTrusts();
}

public class AcademiesApi : IAcademiesApi
{
    private readonly HttpClient _httpClient;

    public AcademiesApi(IOptions<AcademiesApiOptions> academiesApiOptions)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(academiesApiOptions.Value.Endpoint!);
        _httpClient.DefaultRequestHeaders.Add("ApiKey", academiesApiOptions.Value.Key);
    }

    public async Task<string> GetTrusts()
    {
        var response = await _httpClient.GetAsync("/v2/trusts");
        var responseMessage = await response.Content.ReadAsStringAsync();
        return responseMessage;
    }
}

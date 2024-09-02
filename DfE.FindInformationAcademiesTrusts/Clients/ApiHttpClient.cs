using DfE.FindInformationAcademiesTrusts.Options;
using Microsoft.Extensions.Options;
using DfE.FindInformationAcademiesTrusts.Services.Api;
using Serilog;

namespace DfE.FindInformationAcademiesTrusts.Clients;

public class ApiHttpClient : IApiService
{
    private readonly HttpClient client;

    public ApiHttpClient(HttpClient httpClient, IOptions<FindInformationAcademiesTrustsApiOptions> options)
    {
        client = httpClient;

        try {
            if (string.IsNullOrEmpty(options.Value.BaseUri)) {
                Log.Error("FIAT API: Base Uri parameter is null or empty");
                throw new ArgumentNullException(nameof(options));
            }

            client.BaseAddress = new Uri(options.Value.BaseUri);
        } catch (Exception)
        {
            Log.Fatal($"FIAT API: Base Uri value '{options.Value.BaseUri}' is not suitable as a Uri");
        }

        client.DefaultRequestHeaders.Add("Accept", "application/json");

        // TODO: Bearer value must be fetched from Microsoft Identity first
        client.DefaultRequestHeaders.Add("Bearer", "xxx");
    }

    public async Task Get(string path)
    {
        var result = await client.GetAsync(path);

        if (!result.IsSuccessStatusCode)
        {
            Log.Error($"GET: {path}. Result: {result.StatusCode}");
        } else {
            Log.Information($"GET: {path}. Result: {result.StatusCode}");
        }
    }
}

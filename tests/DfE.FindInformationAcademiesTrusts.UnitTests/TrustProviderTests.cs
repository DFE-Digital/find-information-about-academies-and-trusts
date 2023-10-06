using System.Net;
using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using static FluentAssertions.FluentActions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    private readonly MockHttpClientFactory _mockHttpClientFactory;
    private readonly MockLogger<ITrustProvider> _mockLogger;
    private const string TrustsEndpoint = "v3/trusts";
    private const string TrustEndpoint = "v3/trust/1234";

    public TrustProviderTests()
    {
        _mockLogger = new MockLogger<ITrustProvider>();
        _mockHttpClientFactory = new MockHttpClientFactory("AcademiesApi");
    }

    [Fact]
    public async Task GetTrustsByUkprnAsync_should_return_a_trust_if_success_status()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                "{\"Data\": {\"GiasData\": {\"GroupName\": \"trust 1\", \"GroupType\": \"Multi-academy trust\", \"GroupContactAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}, \"ukprn\": \"1234\"}, \"Establishments\": []}}"
            )
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustByUkprnAsync("1234");
        result.Should().BeEquivalentTo(new Trust("trust 1",
            "1234",
            "Multi-academy trust"));
    }

    [Theory]
    [InlineData(
        "{\"Data\": {\"GiasData\": {\"GroupName\": \"trust 1\", \"GroupContactAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}, \"ukprn\": \"1234\"}, \"Establishments\": []}}",
        ""
    )]
    [InlineData(
        "{\"Data\": {\"GiasData\": {\"GroupName\": \"trust 1\", \"GroupType\": \"Single-academy trust\", \"GroupContactAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}, \"ukprn\": \"1234\"}, \"Establishments\": [ {\"Urn\": \"123\"}]}}",
        "Single-academy trust"
    )]
    [InlineData(
        "{\"Data\": {\"GiasData\": {\"GroupName\": \"trust 1\", \"GroupType\": \"Multi-academy trust\", \"GroupContactAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}, \"ukprn\": \"1234\"}, \"Establishments\": [{\"Urn\": \"123\"}, {\"Urn\": \"123\"}, {\"Urn\": \"123\"}]}}",
        "Multi-academy trust"
    )]
    public async Task GetTrustByUkprnAsync_should_include_Trust_Type(string data, string expected)
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(data)
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustByUkprnAsync("1234");
        result?.Type.Should().Be(expected);
    }

    [Fact]
    public async Task GetTrustsByUkprnAsync_should_throw_exception_on_http_response_error_code()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            { Content = new StringContent("") };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        await Invoking(() => sut.GetTrustByUkprnAsync("1234")).Should().ThrowAsync<HttpRequestException>()
            .WithMessage("Problem communicating with Academies API");
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task GetTrustsByUkprnAsync_should_log_any_exception(HttpStatusCode statusCode)
    {
        var responseMessage = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent("")
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);
        try
        {
            await sut.GetTrustByUkprnAsync("1234");
        }
        catch
        {
            _mockLogger.VerifyLogError(
                $"Received {statusCode} from Academies API, \r\nendpoint: https://apiendpoint.dev/v3/trust/1234");
        }
    }

    [Theory]
    [InlineData("{\"Data\": null }")]
    [InlineData("{\"Data\": {\"GiasData\": null } }")]
    [InlineData(null)]
    public async Task GetTrustsByUkprnAsync_should_throw_exception_on_null_data_response(string? content)
    {
        var stringContent = content != null ? new StringContent(content) : null;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = stringContent };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        await Invoking(() => sut.GetTrustByUkprnAsync("1234")).Should().ThrowAsync<JsonException>();
    }

    [Fact]
    public async Task GetTrustsByUkprnAsync_should_return_null_on_Not_Found_Result()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = new StringContent("")
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);
        var result = await sut.GetTrustByUkprnAsync("1234");
        result.Should().BeNull();
    }
}

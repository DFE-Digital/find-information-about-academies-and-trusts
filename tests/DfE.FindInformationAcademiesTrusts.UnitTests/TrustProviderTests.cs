using System.Net;
using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;
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
    public async Task GetTrustsAsync_should_return_trusts_if_success_status()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}, {\"GroupName\": \"trust 2\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}, {\"GroupName\": \"trust 3\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}]}"
            )
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }

    [Theory]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}}, {\"GroupName\": \"trust 2\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}, {\"GroupName\": \"trust 3\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}]}")]
    [InlineData(
        "{\"data\": [{\"groupName\": \"trust 1\", \"trustAddress\": {\"street\":\"Dorthy Inlet\", \"locality\": null, \"additionalLine\": null, \"town\": \"Kingston upon Hull\", \"county\": null, \"postcode\": \"JY36 9VC\"}}, {\"groupName\": \"trust 2\", \"trustAddress\": {\"street\":null, \"locality\": null, \"additionalLine\": null, \"town\": null, \"county\": null, \"postcode\": null}}, {\"groupName\": \"trust 3\", \"trustAddress\": {\"street\":null, \"locality\": null, \"additionalLine\": null, \"town\": null, \"county\": null, \"postcode\": null}}]}")]
    public async Task GetTrustsAsync_should_handle_json_case_insensitively(string data)
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(data)
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.Should().HaveCount(3);
    }

    [Theory]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Street\":\"12 Abbey Road\", \"Locality\": \"Dorthy Inlet\", \"AdditionalLine\": \"East Park\", \"Town\": \"Kingston upon Hull\", \"County\": \"East Riding of Yorkshire\", \"Postcode\": \"JY36 9VC\"}}]}",
        "12 Abbey Road, Dorthy Inlet, East Park, Kingston upon Hull, JY36 9VC"
    )]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": null}]}",
        ""
    )]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Street\":\"Dorthy Inlet\"}}]}",
        "Dorthy Inlet"
    )]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Locality\":\"Dorthy Inlet\"}}]}",
        "Dorthy Inlet"
    )]
    public async Task GetTrustsAsync_should_return_trust_address_formatted_as_string(string data, string expected)
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(data)
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.First().Address.Should().Be(expected);
    }

    [Theory]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"Establishments\": []}]}",
        0
    )]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"Establishments\": [{\"urn\": \"12345\"}]}]}",
        1
    )]
    [InlineData(
        "{\"Data\": [{\"GroupName\": \"trust 1\", \"Establishments\": [{\"urn\": \"12346\"}, {\"urn\": \"12347\"}, {\"urn\": \"12348\"}]}]}",
        3
    )]
    public async Task GetTrustsAsync_should_include_Academies_count(string data, int expected)
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(data)
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.First().AcademyCount.Should().Be(expected);
    }

    [Fact]
    public async Task GetTrustsAsync_should_order_trusts_alphabetically_by_name()
    {
        var fakeUnorderedTrustResponse = new TrustSummaryResponse[]
        {
            new() { Ukprn = "1235", GroupName = "trust 1" },
            new() { Ukprn = "1234", GroupName = "a trust" },
            new() { Ukprn = "1236", GroupName = "another trust" }
        };

        var expectedOrderedTrusts = new TrustSearchEntry[]
        {
            new("a trust", "", "1234", 0),
            new("another trust", "", "1236", 0),
            new("trust 1", "", "1235", 0)
        };

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"Data\":" + JsonSerializer.Serialize(fakeUnorderedTrustResponse) + "}")
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.Should().BeEquivalentTo(expectedOrderedTrusts);
    }

    [Fact]
    public async Task GetTrustsAsync_should_throw_exception_on_http_response_error_code()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            { Content = new StringContent("") };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        await Invoking(() => sut.GetTrustsAsync()).Should().ThrowAsync<HttpRequestException>()
            .WithMessage("Problem communicating with Academies API");
    }

    [Theory]
    [InlineData(HttpStatusCode.InternalServerError)]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.NotFound)]
    public async Task GetTrustsAsync_should_log_any_exception(HttpStatusCode statusCode)
    {
        var responseMessage = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent("")
        };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);
        try
        {
            await sut.GetTrustsAsync();
        }
        catch
        {
            _mockLogger.VerifyLogError(
                $"Received {statusCode} from Academies API, \r\nendpoint: https://apiendpoint.dev/v3/trusts");
        }
    }

    [Theory]
    [InlineData("{\"Data\": null }")]
    [InlineData(null)]
    public async Task GetTrustsAsync_should_throw_exception_on_null_data_response(string? content)
    {
        var stringContent = content != null ? new StringContent(content) : null;

        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = stringContent };

        _mockHttpClientFactory.SetUpHttpGetResponse(TrustsEndpoint, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        await Invoking(() => sut.GetTrustsAsync()).Should().ThrowAsync<JsonException>();
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
        result.Type.Should().Be(expected);
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
    public async Task GetTrustsByNameAsync_should_return_trusts_if_success_status()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(
                "{\"Data\": [{\"GroupName\": \"trust 1\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}, {\"GroupName\": \"trust 2\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}, {\"GroupName\": \"trust 3\", \"TrustAddress\": {\"Street\":null, \"Locality\": null, \"AdditionalLine\": null, \"Town\": null, \"County\": null, \"Postcode\": null}}]}"
            )
        };

        _mockHttpClientFactory.SetUpHttpGetResponse($"{TrustsEndpoint}?groupName=trust", responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsByNameAsync("trust");
        result.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }
}

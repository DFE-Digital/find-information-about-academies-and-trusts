using System.Net;
using static FluentAssertions.FluentActions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    private readonly MockHttpClientFactory _mockHttpClientFactory;
    private readonly MockLogger<ITrustProvider> _mockLogger;

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
                "{\"Data\": [{\"GroupName\": \"trust 1\"}, {\"GroupName\": \"trust 2\"}, {\"GroupName\": \"trust 3\"}]}")
        };

        _mockHttpClientFactory.SetupRequestResponse(_ =>
                _.Method == HttpMethod.Get &&
                _.RequestUri != null &&
                _.RequestUri.AbsoluteUri.Contains("v2/trusts")
            , responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        var result = await sut.GetTrustsAsync();
        result.Should().HaveCount(3).And.OnlyHaveUniqueItems();
    }

    [Fact]
    public async Task GetTrustsAsync_should_throw_exception_on_http_response_error_code()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            { Content = new StringContent("") };

        _mockHttpClientFactory.SetupRequestResponse(_ => _.Method == HttpMethod.Get, responseMessage);

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

        _mockHttpClientFactory.SetupRequestResponse(
            _ => _.Method == HttpMethod.Get,
            responseMessage
        );
        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);
        try
        {
            await sut.GetTrustsAsync();
        }
        catch
        {
            _mockLogger.VerifyLogError($"Received {statusCode} from Academies API");
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

        _mockHttpClientFactory.SetupRequestResponse(_ => _.Method == HttpMethod.Get, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockLogger.Object);

        await Invoking(() => sut.GetTrustsAsync()).Should().ThrowAsync<JsonException>();
    }
}

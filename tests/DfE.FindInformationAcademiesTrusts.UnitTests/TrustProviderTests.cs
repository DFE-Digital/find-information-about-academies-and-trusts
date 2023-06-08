using Microsoft.Extensions.Options;
using Moq.Protected;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    [Fact]
    public Task GetTrustsAsync_should_do_something()
    {
        // Setup HttpClientFactory Mock
        var mockMessageHandler = new Mock<HttpMessageHandler>();

        var resultMessage = new HttpResponseMessage();
        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(resultMessage)
            .Verifiable();

        var httpClient = new HttpClient(mockMessageHandler.Object)
        {
            BaseAddress = new Uri("https://doesthismatter.com/")
        };

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(f => f.CreateClient()).Returns(httpClient);

        // Setup AcademiesApiOptionsMock
        var apiOptions = new AcademiesApiOptions { Endpoint = "thgdfgh", Key = "yyyyy" };
        var mockAcademiesOptions = new Mock<IOptions<AcademiesApiOptions>>();

        mockAcademiesOptions.Setup(o => o.Value).Returns(apiOptions);

        var sut = new TrustProvider(mockHttpClientFactory.Object, mockAcademiesOptions.Object);

        /// write the test
    }
}

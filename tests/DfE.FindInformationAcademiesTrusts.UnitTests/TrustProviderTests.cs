using System.Net;
using Microsoft.Extensions.Options;
using Moq.Protected;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    [Fact]
    public async Task GetTrustsAsync_should_return_trusts_if_success_status()
    {
        // Setup HttpClientFactory Mock
        var mockMessageHandler = new Mock<HttpMessageHandler>();

        var resultMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent("[\"trust 1\",\"trust 2\",\"trust 3\"]") };

        mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(resultMessage)
            .Verifiable();

        var httpClient = new HttpClient(mockMessageHandler.Object);

        var mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

        // Setup AcademiesApiOptionsMock
        var apiOptions = new AcademiesApiOptions { Endpoint = "https://apiendpoint.dev/", Key = "yyyyy" };
        var mockAcademiesOptions = new Mock<IOptions<AcademiesApiOptions>>();
        mockAcademiesOptions.Setup(o => o.Value).Returns(apiOptions);

        var sut = new TrustProvider(mockHttpClientFactory.Object, mockAcademiesOptions.Object);

        // write the test
        var result = await sut.GetTrustsAsync();

        result.Should().BeEquivalentTo("trust 1", "trust 2", "trust 3");
    }
}

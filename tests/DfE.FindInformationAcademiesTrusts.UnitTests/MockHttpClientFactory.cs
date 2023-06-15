using System.Linq.Expressions;
using Moq.Protected;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class MockHttpClientFactory : Mock<IHttpClientFactory>
{
    private readonly Mock<HttpMessageHandler> _mockMessageHandler;

    public MockHttpClientFactory()
    {
        _mockMessageHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockMessageHandler.Object);
        httpClient.BaseAddress = new Uri("https://apiendpoint.dev/");
        httpClient.DefaultRequestHeaders.Add("ApiKey", "yyyyy");

        Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);
    }

    public MockHttpClientFactory SetupRequestResponse(Expression<Func<HttpRequestMessage, bool>> requestMatcher,
        HttpResponseMessage resultMessage)
    {
        _mockMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is(requestMatcher),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(resultMessage)
            .Verifiable();

        return this;
    }
}

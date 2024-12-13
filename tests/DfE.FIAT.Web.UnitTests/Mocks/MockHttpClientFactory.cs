using System.Linq.Expressions;
using Moq.Protected;

namespace DfE.FIAT.Web.UnitTests.Mocks;

public interface IRevealHttpMessageHandlerProtectedMethods
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
}

public class MockHttpClientFactory : Mock<IHttpClientFactory>
{
    private readonly Mock<HttpMessageHandler> _mockMessageHandler;
    private readonly Uri _httpClientBaseAddress;
    private const string FakeBaseAddress = "https://apiendpoint.dev/";

    public MockHttpClientFactory(string clientName)
    {
        _mockMessageHandler = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_mockMessageHandler.Object);
        _httpClientBaseAddress = new Uri(FakeBaseAddress);
        httpClient.BaseAddress = _httpClientBaseAddress;
        httpClient.DefaultRequestHeaders.Add("ApiKey", "yyyyy");

        Setup(f => f.CreateClient(It.Is<string>(n => n == clientName))).Returns(httpClient).Verifiable();
    }

    private MockHttpClientFactory SetupRequestResponse(Expression<Func<HttpRequestMessage, bool>> requestMatcher,
        HttpResponseMessage resultMessage)
    {
        _mockMessageHandler
            .Protected().As<IRevealHttpMessageHandlerProtectedMethods>()
            .Setup(m => m.SendAsync(It.Is(requestMatcher), It.IsAny<CancellationToken>()).Result)
            .Returns<HttpRequestMessage, CancellationToken>((request, _) =>
            {
                resultMessage.RequestMessage = request;
                return resultMessage;
            })
            .Verifiable();

        return this;
    }

    public MockHttpClientFactory SetUpHttpGetResponse(string endpoint, HttpResponseMessage resultMessage)
    {
        var expectedUri = new Uri(_httpClientBaseAddress, endpoint);

        return SetupRequestResponse(requestMessage =>
                requestMessage.Method == HttpMethod.Get &&
                requestMessage.RequestUri == expectedUri
            , resultMessage);
    }
}

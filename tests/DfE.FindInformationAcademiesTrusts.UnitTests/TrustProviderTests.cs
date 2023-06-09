using System.Net;
using Microsoft.Extensions.Options;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    [Fact]
    public async Task GetTrustsAsync_should_return_trusts_if_success_status()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent("[\"trust 1\",\"trust 2\",\"trust 3\"]") };
        var mockHttpClientFactory = new MockHttpClientFactory();
        mockHttpClientFactory.SetupRequestResponse(_ => true, responseMessage);

        var apiOptions = new AcademiesApiOptions { Endpoint = "https://apiendpoint.dev/", Key = "yyyyy" };
        var mockAcademiesOptions = new Mock<IOptions<AcademiesApiOptions>>();
        mockAcademiesOptions.Setup(o => o.Value).Returns(apiOptions);

        var sut = new TrustProvider(mockHttpClientFactory.Object, mockAcademiesOptions.Object);

        // Act
        var result = await sut.GetTrustsAsync();

        // Assert
        result.Should().BeEquivalentTo("trust 1", "trust 2", "trust 3");
    }
}

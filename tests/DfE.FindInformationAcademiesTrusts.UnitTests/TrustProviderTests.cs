using System.Net;
using Microsoft.Extensions.Options;
using static FluentAssertions.FluentActions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class TrustProviderTests
{
    private readonly Mock<IOptions<AcademiesApiOptions>> _mockAcademiesOptions;
    private readonly MockHttpClientFactory _mockHttpClientFactory;

    public TrustProviderTests()
    {
        _mockHttpClientFactory = new MockHttpClientFactory();
        var apiOptions = new AcademiesApiOptions { Endpoint = "https://apiendpoint.dev/", Key = "yyyyy" };
        _mockAcademiesOptions = new Mock<IOptions<AcademiesApiOptions>>();
        _mockAcademiesOptions.Setup(o => o.Value).Returns(apiOptions);
    }

    [Fact]
    public async Task GetTrustsAsync_should_return_trusts_if_success_status()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            { Content = new StringContent("[\"trust 1\",\"trust 2\",\"trust 3\"]") };

        _mockHttpClientFactory.SetupRequestResponse(_ => _.Method == HttpMethod.Get, responseMessage);
        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockAcademiesOptions.Object);

        var result = await sut.GetTrustsAsync();

        result.Should().BeEquivalentTo("trust 1", "trust 2", "trust 3");
    }

    [Fact]
    public async Task GetTrustsAsync_should_throw_exception_on_http_response_error_code()
    {
        var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            { Content = new StringContent("[\"trust 1\",\"trust 2\",\"trust 3\"]") };

        _mockHttpClientFactory.SetupRequestResponse(_ => true, responseMessage);

        var sut = new TrustProvider(_mockHttpClientFactory.Object, _mockAcademiesOptions.Object);


        await Invoking(() => sut.GetTrustsAsync()).Should().ThrowAsync<ApplicationException>()
            .WithMessage("Problem communicating with Academies API");
    }
}

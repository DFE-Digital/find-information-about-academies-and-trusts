using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class IndexModelTests
{
    [Fact]
    public async Task OnGetAsync_should_log_trusts_from_api()
    {
        var mockLogger = new MockLogger<IndexModel>();
        var mockAcademiesApi = new Mock<IAcademiesApi>();

        const string data = "this is data about trusts in the api";
        mockAcademiesApi.Setup(a => a.GetTrusts().Result).Returns(data);

        var sut = new IndexModel(mockLogger.Object, mockAcademiesApi.Object);

        await sut.OnGetAsync();

        mockLogger.VerifyLogInformation(data);
    }
}

using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class IndexModelTests
{
    [Fact]
    public async Task OnGetAsync_should_log_trusts_from_api()
    {
        var loggerMock = new MockLogger<IndexModel>();
        var academiesApiMock = new Mock<IAcademiesApi>();

        const string data = "this is data about trusts in the api";
        academiesApiMock.Setup(a => a.GetTrusts().Result).Returns(data);

        var sut = new IndexModel(loggerMock.Object, academiesApiMock.Object);

        await sut.OnGetAsync();

        loggerMock.VerifyLogInformation(data);
    }
}

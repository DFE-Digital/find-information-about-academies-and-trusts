using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class DetailsModelTests
{
    private readonly DetailsModel _sut;

    public DetailsModelTests()
    {
        Mock<ITrustProvider> mockTrustProvider = new();
        _sut = new DetailsModel(mockTrustProvider.Object);
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        _sut.PageName.Should().Be("Details");
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class AcademiesDetailsModelTests
{
    private readonly AcademiesDetailsModel _sut;

    public AcademiesDetailsModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new AcademiesDetailsModel(mockTrustProvider.Object);
    }

    [Fact]
    public void PageTitle_should_be_AcademiesDetails()
    {
        _sut.PageTitle.Should().Be("Academies details");
    }

    [Fact]
    public void TabName_should_be_Details()
    {
        _sut.TabName.Should().Be("Details are");
    }
}

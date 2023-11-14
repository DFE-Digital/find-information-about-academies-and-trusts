using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingsModelTests
{
    private readonly OfstedRatingsModel _sut;

    public OfstedRatingsModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new OfstedRatingsModel(mockTrustProvider.Object);
    }

    [Fact]
    public void PageTitle_should_be_AcademiesDetails()
    {
        _sut.PageTitle.Should().Be("Academies Ofsted ratings");
    }

    [Fact]
    public void TabName_should_be_Details()
    {
        _sut.TabName.Should().Be("Ofsted ratings");
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingsModelTests
{
    private readonly OfstedRatingsModel _sut;

    public OfstedRatingsModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        Mock<IDataSourceProvider> mockDataUpdatedProvider = new();
        _sut = new OfstedRatingsModel(mockTrustProvider.Object, mockDataUpdatedProvider.Object);
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }

    [Fact]
    public void PageTitle_should_be_AcademiesOfstedRatingsPage()
    {
        _sut.PageTitle.Should().Be("Academies Ofsted ratings");
    }

    [Fact]
    public void TabName_should_be_OfstedRatings()
    {
        _sut.TabName.Should().Be("Ofsted ratings");
    }
}

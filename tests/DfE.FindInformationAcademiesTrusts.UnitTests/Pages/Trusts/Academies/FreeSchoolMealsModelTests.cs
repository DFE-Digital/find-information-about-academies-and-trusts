using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class FreeSchoolMealsModelTests
{
    private readonly FreeSchoolMealsModel _sut;

    public FreeSchoolMealsModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        var mockFreeSchoolMealsAverageProvider = new Mock<IFreeSchoolMealsAverageProvider>();
        _sut = new FreeSchoolMealsModel(mockTrustProvider.Object, mockFreeSchoolMealsAverageProvider.Object);
    }

    [Fact]
    public void PageTitle_should_be_AcademiesDetails()
    {
        _sut.PageTitle.Should().Be("Academies free school meals");
    }

    [Fact]
    public void TabName_should_be_Details()
    {
        _sut.TabName.Should().Be("Free school meals");
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }
}

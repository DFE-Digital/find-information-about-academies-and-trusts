using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class FreeSchoolMealsModelTests
{
    private readonly FreeSchoolMealsModel _sut;
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider;

    public FreeSchoolMealsModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        _mockFreeSchoolMealsAverageProvider = new Mock<IFreeSchoolMealsAverageProvider>();
        _sut = new FreeSchoolMealsModel(mockTrustProvider.Object, _mockFreeSchoolMealsAverageProvider.Object,
            new MockDataSourceProvider().Object, new MockLogger<FreeSchoolMealsModel>().Object);
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

    [Fact]
    public void GetLaAverageFreeSchoolMeals_should_return_double_if_free_School_meals_provider_returns_value()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(111);
        var mockPercentage = 24.5;
        _mockFreeSchoolMealsAverageProvider.Setup(p => p.GetLaAverage(dummyAcademy)).Returns(mockPercentage);

        var result = _sut.GetLaAverageFreeSchoolMeals(dummyAcademy);
        result.Should().Be(mockPercentage);
    }

    [Fact]
    public void GetNationalAverageFreeSchoolMeals_should_return_double_if_free_School_meals_provider_returns_value()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(111);
        var mockPercentage = 24.5;
        _mockFreeSchoolMealsAverageProvider.Setup(p => p.GetNationalAverage(dummyAcademy)).Returns(mockPercentage);

        var result = _sut.GetNationalAverageFreeSchoolMeals(dummyAcademy);
        result.Should().Be(mockPercentage);
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class FreeSchoolMealsModelTests
{
    private readonly FreeSchoolMealsModel _sut;
    private readonly Mock<IFreeSchoolMealsAverageProvider> _mockFreeSchoolMealsAverageProvider;
    private readonly Mock<ITrustProvider> _mockTrustProvider;

    public FreeSchoolMealsModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockFreeSchoolMealsAverageProvider = new Mock<IFreeSchoolMealsAverageProvider>();
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrust);
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync(dummyTrust.Uid))
            .ReturnsAsync(new TrustSummaryDto(dummyTrust.Uid, dummyTrust.Name, dummyTrust.Type,
                dummyTrust.Academies.Length));
        _sut = new FreeSchoolMealsModel(_mockTrustProvider.Object, _mockFreeSchoolMealsAverageProvider.Object,
            new MockDataSourceProvider().Object, new MockLogger<FreeSchoolMealsModel>().Object) { Uid = "1234" };
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

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryDto?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockFreeSchoolMealsAverageProvider.Verify(e => e.GetFreeSchoolMealsUpdated(), Times.Once);
        _sut.DataSources.Count.Should().Be(2);
        _sut.DataSources[0].Fields.Should().Contain(new[]
        {
            "Pupils eligible for free school meals"
        });
        _sut.DataSources[1].Fields.Should().Contain(new[]
        {
            "Local authority average 2022/23", "National average 2022/23"
        });
    }
}

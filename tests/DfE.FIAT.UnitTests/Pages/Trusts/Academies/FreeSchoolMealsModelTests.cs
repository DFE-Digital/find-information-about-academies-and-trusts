using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Pages.Trusts;
using DfE.FIAT.Web.Pages.Trusts.Academies;
using DfE.FIAT.Web.Services.Academy;
using DfE.FIAT.Web.Services.Export;
using DfE.FIAT.Web.Services.Trust;
using DfE.FIAT.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.UnitTests.Pages.Trusts.Academies;

public class FreeSchoolMealsModelTests
{
    private readonly FreeSchoolMealsModel _sut;
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockDataSourceService _mockDataSourceService = new();

    public FreeSchoolMealsModelTests()
    {
        var testTrustUid = "1234";
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(testTrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(testTrustUid, testTrustName, testTrustType,
                1));

        _sut = new FreeSchoolMealsModel(
                _mockDataSourceService.Object, new MockLogger<FreeSchoolMealsModel>().Object,
                _mockTrustService.Object, _mockAcademyService.Object, _mockExportService.Object,
                _mockDateTimeProvider.Object)
            { Uid = "1234" };
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
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceService.Verify(d => d.GetAsync(Source.ExploreEducationStatistics), Times.Once);
        _sut.DataSources.Count.Should().Be(2);
        _sut.DataSources[0].Fields.Should().Contain(new[]
        {
            "Pupils eligible for free school meals"
        });
        _sut.DataSources[1].Fields.Should().Contain(new[]
        {
            "Local authority average 2023/24", "National average 2023/24"
        });
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyFreeSchoolMealsServiceModel("1", "Academy 1", 12.5, 13.5, 14.5),
            new AcademyFreeSchoolMealsServiceModel("2", "Academy 2", null, 70.1, 64.1),
            new AcademyFreeSchoolMealsServiceModel("3", "Academy 3", 8.2, 4, 10)
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustFreeSchoolMealsAsync(_sut.Uid))
            .ReturnsAsync(academies);

        _ = await _sut.OnGetAsync();

        _sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview", "1234", false, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts", "1234", false, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (1)", "/Trusts/Academies/Details",
                "1234", true, "academies-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance", "1234", false,
                "governance-nav")
        ]);
    }
}

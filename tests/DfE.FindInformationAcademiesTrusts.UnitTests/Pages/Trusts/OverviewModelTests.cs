using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class OverviewModelTests
{
    private readonly OverviewModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();

    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new("1234", "", "", "", "", "", "", null, null, 0, new Dictionary<string, int>(), 0, 0);

    public OverviewModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TrustUid, "My Trust", "Multi-academy trust", 3));
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(BaseTrustOverviewServiceModel);

        _sut = new OverviewModel(
                _mockDataSourceService.Object,
                new MockLogger<OverviewModel>().Object,
                _mockTrustService.Object,
                _mockLinksToOtherServices.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void PageName_should_be_Overview()
    {
        _sut.PageName.Should().Be("Overview");
    }

    [Fact]
    public async Task OnGetAsync_sets_list_of_local_authorities()
    {
        var overviewWithLocalAuthorities = BaseTrustOverviewServiceModel with
        {
            AcademiesByLocalAuthority = new Dictionary<string, int>
            {
                { "localAuth1", 6 },
                { "localAuth2", 1 }
            }
        };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(overviewWithLocalAuthorities);

        await _sut.OnGetAsync();

        _sut.AcademiesInEachLocalAuthority
            .Should()
            .BeEquivalentTo(new (string Authority, int Total)[]
            {
                ("localAuth1", 6),
                ("localAuth2", 1)
            });
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string>
            { "Trust details", "Reference numbers", "Trust summary" });
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(BaseTrustOverviewServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(BaseTrustOverviewServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_returned_from_link_builder()
    {
        _mockLinksToOtherServices.Setup(l =>
                l.GetInformationAboutSchoolsListingLinkForTrust(BaseTrustOverviewServiceModel.Uid))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices
            .Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(BaseTrustOverviewServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices
            .Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(BaseTrustOverviewServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().Be("url");
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview", "1234", true, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts", "1234", false, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance", "1234", false,
                "governance-nav")
        ]);
    }
}

using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class TrustSummaryModelTests
{
    private readonly TrustSummaryModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new(TrustUid, "", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0, new Dictionary<string, int>(), 0,
            0);

    public TrustSummaryModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TrustUid, "My Trust", "Multi-academy trust", 3));
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(BaseTrustOverviewServiceModel);

        _sut = new TrustSummaryModel(
                _mockDataSourceService.Object,
                new MockLogger<TrustSummaryModel>().Object,
                _mockTrustService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public async Task OnGetAsync_sets_list_of_local_authorities()
    {
        var overviewWithLocalAuthorities = BaseTrustOverviewServiceModel with
        {
            AcademiesByLocalAuthority = new Dictionary<string, int>
            {
                { "localAuth1", 6 },
                { "localAuth2", 1 },
                { "localAuth3", 10 },
                { "localAuth10", 4 },
                { "localAuth11", 8 },
                { "localAuth12", 6 }
            }
        };
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(overviewWithLocalAuthorities);

        await _sut.OnGetAsync();

        _sut.AcademiesInEachLocalAuthority
            .Should()
            .BeEquivalentTo(new (string Authority, int Total)[]
            {
                ("localAuth3", 10),
                ("localAuth11", 8),
                ("localAuth1", 6),
                ("localAuth12", 6),
                ("localAuth10", 4),
                ("localAuth2", 1)
            }, options => options.WithStrictOrdering());
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
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview/TrustDetails", "1234", true, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts/InDfe", "1234", false, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel("Ofsted", "/Trusts/Ofsted/CurrentRatings", "1234", false, "ofsted-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance/TrustLeadership", "1234", false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_SubNavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().BeEquivalentTo([
            new TrustSubNavigationLinkModel("Trust details", "./TrustDetails", "1234", "Overview", false),
            new TrustSubNavigationLinkModel("Trust summary", "./TrustSummary", "1234", "Overview", true),
            new TrustSubNavigationLinkModel("Reference numbers", "./ReferenceNumbers", "1234", "Overview", false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Trust summary");
        _sut.TrustPageMetadata.PageName.Should().Be("Overview");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}

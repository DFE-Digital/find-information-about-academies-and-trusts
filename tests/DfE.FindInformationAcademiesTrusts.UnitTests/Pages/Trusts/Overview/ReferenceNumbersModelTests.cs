using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class ReferenceNumbersModelTests
{
    private readonly ReferenceNumbersModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new(TrustUid, "", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0, new Dictionary<string, int>(), 0,
            0);

    public ReferenceNumbersModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TrustUid, "My Trust", "Multi-academy trust", 3));
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(BaseTrustOverviewServiceModel);

        _sut = new ReferenceNumbersModel(
                _mockDataSourceService.Object,
                new MockLogger<ReferenceNumbersModel>().Object,
                _mockTrustService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel(ViewConstants.OverviewPageName, "/Trusts/Overview/TrustDetails", "1234", true,
                "overview-nav"),
            new TrustNavigationLinkModel(ViewConstants.ContactsPageName, "/Trusts/Contacts/InDfe", "1234", false,
                "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/InTrust/Details",
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/CurrentRatings", "1234", false,
                "ofsted-nav"),
            new TrustNavigationLinkModel(ViewConstants.GovernancePageName, "/Trusts/Governance/TrustLeadership",
                "1234", false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_SubNavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().BeEquivalentTo([
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustDetailsPageName, "./TrustDetails", "1234",
                ViewConstants.OverviewPageName,
                false),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustSummaryPageName, "./TrustSummary", "1234",
                ViewConstants.OverviewPageName,
                false),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewReferenceNumbersPageName, "./ReferenceNumbers",
                "1234",
                ViewConstants.OverviewPageName, true)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewReferenceNumbersPageName);
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.OverviewPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}

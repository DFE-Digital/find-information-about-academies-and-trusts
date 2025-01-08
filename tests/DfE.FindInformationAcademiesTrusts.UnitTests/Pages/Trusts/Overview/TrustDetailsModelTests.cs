using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class TrustDetailsModelTests
{
    private readonly TrustDetailsModel _sut;
    private const string TrustUid = "1234";
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();

    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new(TrustUid, "GroupID", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0,
            new Dictionary<string, int>(), 0,
            0);

    public TrustDetailsModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(TrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TrustUid, "My Trust", "Multi-academy trust", 3));
        _mockTrustService.Setup(t => t.GetTrustOverviewAsync(TrustUid)).ReturnsAsync(BaseTrustOverviewServiceModel);

        _sut = new TrustDetailsModel(
                _mockDataSourceService.Object,
                new MockLogger<TrustDetailsModel>().Object,
                _mockTrustService.Object,
                _mockLinksToOtherServices.Object)
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
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l =>
                l.CompaniesHouseListingLink(BaseTrustOverviewServiceModel.CompaniesHouseNumber))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l =>
                l.CompaniesHouseListingLink(BaseTrustOverviewServiceModel.CompaniesHouseNumber))
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
            .Setup(l => l.FinancialBenchmarkingInsightsToolListingLink(
                BaseTrustOverviewServiceModel.CompaniesHouseNumber))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FinancialBenchmarkingInsightsToolLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices
            .Setup(l => l.FinancialBenchmarkingInsightsToolListingLink(
                BaseTrustOverviewServiceModel.CompaniesHouseNumber))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FinancialBenchmarkingInsightsToolLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel.Uid,
                BaseTrustOverviewServiceModel.Type, BaseTrustOverviewServiceModel.SingleAcademyTrustAcademyUrn))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel.Uid,
                BaseTrustOverviewServiceModel.Type, BaseTrustOverviewServiceModel.SingleAcademyTrustAcademyUrn))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().Be("url");
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
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
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
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustDetailsPageName, "./TrustDetails", "1234", ViewConstants.OverviewPageName,
                true),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewTrustSummaryPageName, "./TrustSummary", "1234", ViewConstants.OverviewPageName,
                false),
            new TrustSubNavigationLinkModel(ViewConstants.OverviewReferenceNumbersPageName, "./ReferenceNumbers", "1234",
                ViewConstants.OverviewPageName, false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewTrustDetailsPageName);
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.OverviewPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public void SharepointLink_should_be_empty_string_by_default()
    {
        _sut.SharepointLink.Should().Be("");
    }

    [Fact]
    public async Task OnGetAsync_should_Set_correct_SharepointLink()
    {
        _mockLinksToOtherServices.Setup(l =>
                l.SharepointFolderLink(BaseTrustOverviewServiceModel.GroupId))
            .Returns("url/groupID");
        await _sut.OnGetAsync();

        _sut.SharepointLink.Should().Be("url/groupID");
    }
}

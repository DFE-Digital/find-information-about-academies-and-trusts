using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class TrustDetailsModelTests : BaseOverviewAreaModelTests<TrustDetailsModel>
{
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();

    public TrustDetailsModelTests()
    {
        _sut = new TrustDetailsModel(
                _mockDataSourceService.Object,
                new MockLogger<TrustDetailsModel>().Object,
                _mockTrustService.Object,
                _mockLinksToOtherServices.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink("123456"))
            .Returns((string?)null);

        await _sut.OnGetAsync();

        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink("123456"))
            .Returns("url");

        await _sut.OnGetAsync();

        _sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_returned_from_link_builder()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLinkForTrust(TrustUid))
            .Returns("url");

        await _sut.OnGetAsync();

        _sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
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

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await _sut.OnGetAsync();

        _sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./TrustDetails");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewTrustDetailsPageName);
    }
}

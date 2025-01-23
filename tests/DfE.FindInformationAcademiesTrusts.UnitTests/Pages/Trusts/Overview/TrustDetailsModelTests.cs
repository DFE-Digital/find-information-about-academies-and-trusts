using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class TrustDetailsModelTests : BaseOverviewAreaModelTests<TrustDetailsModel>
{
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();

    public TrustDetailsModelTests()
    {
        Sut = new TrustDetailsModel(
                MockDataSourceService.Object,
                new MockLogger<TrustDetailsModel>().Object,
                MockTrustService.Object,
                _mockLinksToOtherServices.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink("123456"))
            .Returns((string?)null);

        await Sut.OnGetAsync();

        Sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink("123456"))
            .Returns("url");

        await Sut.OnGetAsync();

        Sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_returned_from_link_builder()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLinkForTrust(TrustUid))
            .Returns("url");

        await Sut.OnGetAsync();

        Sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        SetupTrustOverview(BaseTrustOverviewServiceModel with { CompaniesHouseNumber = "123456" });
        _mockLinksToOtherServices
            .Setup(l => l.FinancialBenchmarkingInsightsToolListingLink(
                BaseTrustOverviewServiceModel.CompaniesHouseNumber))
            .Returns((string?)null);
        await Sut.OnGetAsync();
        Sut.FinancialBenchmarkingInsightsToolLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices
            .Setup(l => l.FinancialBenchmarkingInsightsToolListingLink(
                BaseTrustOverviewServiceModel.CompaniesHouseNumber))
            .Returns("url");
        await Sut.OnGetAsync();
        Sut.FinancialBenchmarkingInsightsToolLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel.Uid,
                BaseTrustOverviewServiceModel.Type, BaseTrustOverviewServiceModel.SingleAcademyTrustAcademyUrn))
            .Returns((string?)null);
        await Sut.OnGetAsync();
        Sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(BaseTrustOverviewServiceModel.Uid,
                BaseTrustOverviewServiceModel.Type, BaseTrustOverviewServiceModel.SingleAcademyTrustAcademyUrn))
            .Returns("url");
        await Sut.OnGetAsync();
        Sut.FindSchoolPerformanceLink.Should().Be("url");
    }

    [Fact]
    public void SharepointLink_should_be_empty_string_by_default()
    {
        Sut.SharepointLink.Should().Be("");
    }

    [Fact]
    public async Task OnGetAsync_should_Set_correct_SharepointLink()
    {
        _mockLinksToOtherServices.Setup(l =>
                l.SharepointFolderLink(BaseTrustOverviewServiceModel.GroupId))
            .Returns("url/groupID");
        await Sut.OnGetAsync();

        Sut.SharepointLink.Should().Be("url/groupID");
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./TrustDetails");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OverviewTrustDetailsPageName);
    }
}

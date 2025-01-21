using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class CurrentRatingsModelTests : BaseOfstedAreaModelTests<CurrentRatingsModel>
{
    public CurrentRatingsModelTests()
    {
        _sut = new CurrentRatingsModel(_mockDataSourceService.Object,
                _mockTrustService.Object,
                _mockAcademyService.Object,
                _mockExportService.Object,
                _mockDateTimeProvider.Object,
                new MockLogger<CurrentRatingsModel>().Object
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await _sut.OnGetAsync();

        _sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./CurrentRatings");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Current ratings");
    }
}

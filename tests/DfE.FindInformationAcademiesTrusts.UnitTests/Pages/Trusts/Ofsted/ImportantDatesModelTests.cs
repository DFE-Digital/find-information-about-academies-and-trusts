using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class ImportantDatesModelTests : BaseOfstedAreaModelTests<ImportantDatesModel>
{
    public ImportantDatesModelTests()
    {
        _sut = new ImportantDatesModel(_mockDataSourceService.Object,
                _mockTrustService.Object,
                _mockAcademyService.Object,
                _mockExportService.Object,
                _mockDateTimeProvider.Object,
                new MockLogger<ImportantDatesModel>().Object
            )
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await _sut.OnGetAsync();

        _sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./ImportantDates");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Important dates");
    }
}

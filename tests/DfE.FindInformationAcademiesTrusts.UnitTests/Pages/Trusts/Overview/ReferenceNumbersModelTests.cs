using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Overview;

public class ReferenceNumbersModelTests : BaseOverviewAreaModelTests<ReferenceNumbersModel>
{
    public ReferenceNumbersModelTests()
    {
        Sut = new ReferenceNumbersModel(
                MockDataSourceService,
                MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Reference numbers");
    }
}

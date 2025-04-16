using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests : BaseTrustPageTests<TrustsAreaModel>
{
    private class TrustsAreaModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService) : TrustsAreaModel(dataSourceService, trustService);

    public TrustsAreaModelTests()
    {
        Sut = new TrustsAreaModelImpl(MockDataSourceService, MockTrustService)
            { Uid = TrustUid };
    }

    [Fact]
    public void GroupUid_should_be_empty_string_by_default()
    {
        Sut = new TrustsAreaModelImpl(MockDataSourceService, MockTrustService);
        Sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        await Sut.OnGetAsync();
        Sut.PageMetadata.PageName.Should().BeNull();
    }

    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        //Default to empty
        Sut.DataSourcesPerPage.Should().BeEmpty();
    }
}

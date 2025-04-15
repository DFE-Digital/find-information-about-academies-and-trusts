using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests : BaseTrustPageTests<TrustsAreaModel>
{
    private readonly ILogger<TrustsAreaModel> _logger = MockLogger.CreateLogger<TrustsAreaModel>();

    private class TrustsAreaModelImpl(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        ILogger<TrustsAreaModel> logger) : TrustsAreaModel(dataSourceService, trustService, logger);

    public TrustsAreaModelTests()
    {
        Sut = new TrustsAreaModelImpl(MockDataSourceService, MockTrustService, _logger)
            { Uid = TrustUid };
    }

    [Fact]
    public void GroupUid_should_be_empty_string_by_default()
    {
        Sut = new TrustsAreaModelImpl(MockDataSourceService, MockTrustService, _logger);
        Sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        await Sut.OnGetAsync();
        Sut.TrustPageMetadata.PageName.Should().BeNull();
    }

    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        //Default to empty
        Sut.DataSourcesPerPage.Should().BeEmpty();
    }
}

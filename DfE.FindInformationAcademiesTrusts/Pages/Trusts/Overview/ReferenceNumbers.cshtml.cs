using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

public class ReferenceNumbersModel(
    IDataSourceService dataSourceService,
    ILogger<ReferenceNumbersModel> logger,
    ITrustService trustService)
    : OverviewAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = "Reference numbers" };
}

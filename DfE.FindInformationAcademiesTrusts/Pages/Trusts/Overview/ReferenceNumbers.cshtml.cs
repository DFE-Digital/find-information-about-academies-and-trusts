using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

public class ReferenceNumbersModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : OverviewAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "Reference numbers";

    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = SubPageName };
}

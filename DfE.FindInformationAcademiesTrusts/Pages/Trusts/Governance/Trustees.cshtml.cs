using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class TrusteesModel(
    IDataSourceService dataSourceService,
    ILogger<TrusteesModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger)
{
    public const string SubPageName = "Trustees";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}

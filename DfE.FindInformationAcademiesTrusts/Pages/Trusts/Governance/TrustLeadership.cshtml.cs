using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class TrustLeadershipModel(
    IDataSourceService dataSourceService,
    ILogger<TrustLeadershipModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger)
{
    public const string SubPageName = "Trust leadership";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}

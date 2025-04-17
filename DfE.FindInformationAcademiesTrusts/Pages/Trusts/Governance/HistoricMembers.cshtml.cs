using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class HistoricMembersModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "Historic members";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}

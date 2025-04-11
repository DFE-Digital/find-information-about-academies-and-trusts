using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class MembersModel(
    IDataSourceService dataSourceService,
    ILogger<MembersModel> logger,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService, logger)
{
    public const string SubPageName = "Members";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}

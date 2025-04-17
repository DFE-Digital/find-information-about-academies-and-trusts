using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class TrusteesModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : GovernanceAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "Trustees";

    public override PageMetadata PageMetadata => base.PageMetadata with { SubPageName = SubPageName };
}

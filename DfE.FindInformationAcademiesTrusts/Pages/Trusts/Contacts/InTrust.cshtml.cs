using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class InTrustModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : ContactsAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "In this trust";

    public override PageMetadata PageMetadata => base.PageMetadata with { SubPageName = SubPageName };
}

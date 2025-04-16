using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class InDfeModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : ContactsAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "In DfE";

    public override PageMetadata PageMetadata => base.PageMetadata with { SubPageName = SubPageName };
}

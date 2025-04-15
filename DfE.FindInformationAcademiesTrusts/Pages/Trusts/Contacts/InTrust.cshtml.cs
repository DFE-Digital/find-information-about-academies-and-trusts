using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class InTrustModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<InTrustModel> logger)
    : ContactsAreaModel(dataSourceService, trustService, logger)
{
    public const string SubPageName = "In this trust";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { SubPageName = SubPageName };
}

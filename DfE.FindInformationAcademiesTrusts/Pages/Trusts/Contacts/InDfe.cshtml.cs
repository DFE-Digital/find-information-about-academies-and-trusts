using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class InDfeModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<InDfeModel> logger)
    : ContactsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { SubPageName = ViewConstants.ContactsInDfePageName };
}

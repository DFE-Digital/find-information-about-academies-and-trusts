using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public class EditTrustRelationshipManagerModel(
    IDataSourceService dataSourceService,
    ILogger<EditTrustRelationshipManagerModel> logger,
    ITrustService trustService)
    : EditContactModel(dataSourceService, trustService,
        logger, ContactRole.TrustRelationshipManager)
{
    protected override InternalContact? GetContactFromServiceModel(TrustContactsServiceModel contacts)
    {
        return contacts.TrustRelationshipManager;
    }
}

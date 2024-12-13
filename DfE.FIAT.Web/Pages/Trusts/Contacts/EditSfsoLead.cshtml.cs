using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public class EditSfsoLeadModel(
    IDataSourceService dataSourceService,
    ILogger<EditSfsoLeadModel> logger,
    ITrustService trustService)
    : EditContactModel(dataSourceService, trustService,
        logger, ContactRole.SfsoLead)
{
    protected override InternalContact? GetContactFromServiceModel(TrustContactsServiceModel contacts)
    {
        return contacts.SfsoLead;
    }
}

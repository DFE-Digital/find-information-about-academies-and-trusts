using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

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

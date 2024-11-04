using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

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

using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.FeatureManagement.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

[FeatureGate(FeatureFlags.EditContactsUI)]
public class EditTrustRelationshipManagerModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<EditTrustRelationshipManagerModel> logger,
    ITrustService trustService)
    : EditContactModel(trustProvider, dataSourceService, trustService,
        logger, ContactRole.TrustRelationshipManager)
{
    protected override InternalContact? GetContactFromServiceModel(TrustContactsServiceModel contacts)
    {
        return contacts.TrustRelationshipManager;
    }
}

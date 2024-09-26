using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.FeatureManagement.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

[FeatureGate(FeatureFlags.EditContactsUI)]
public class EditSfsoLeadModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<EditSfsoLeadModel> logger,
    ITrustService trustService)
    : EditContactModel(trustProvider, dataSourceService, trustService,
        logger, ContactRole.SfsoLead)
{
    protected override InternalContact? GetContactFromServiceModel(TrustContactsServiceModel contacts)
    {
        return contacts.SfsoLead;
    }
}

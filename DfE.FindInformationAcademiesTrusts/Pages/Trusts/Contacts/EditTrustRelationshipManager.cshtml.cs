using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

[FeatureGate(FeatureFlags.EditContactsUI)]
public class EditTrustRelationshipManagerModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<EditTrustRelationshipManagerModel> logger,
    ITrustService trustService)
    : EditContactModel(trustProvider, dataSourceService, trustService,
        logger, "Trust relationship manager")
{
    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        var contacts = await TrustService.GetTrustContactsAsync(Uid);

        Email = contacts.TrustRelationshipManager?.Email;
        Name = contacts.TrustRelationshipManager?.FullName;

        return pageResult;
    }
}

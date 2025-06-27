using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Contacts;

[FeatureGate(FeatureFlags.ContactsInDfeForSchools)]
public class InDfeModel(
    ISchoolService schoolService,
    ITrustService trustService,
    ISchoolContactsService schoolContactsService,
    IDataSourceService dataSourceService,
    ISchoolNavMenu schoolNavMenu)
    : ContactsAreaModel(schoolService, trustService, dataSourceService, schoolNavMenu)
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName
    };

    public static string SubPageName => "In DfE";

    public Person RegionsGroupLocalAuthorityLead { get; private set; } = null!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        var contacts = await schoolContactsService.GetInternalContactsAsync(Urn);

        RegionsGroupLocalAuthorityLead = contacts.RegionsGroupLocalAuthorityLead;

        return pageResult;
    }
}

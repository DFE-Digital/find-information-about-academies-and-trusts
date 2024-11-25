using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class ContactAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<ContactAreaModel> logger,
    Func<TrustSubNavigationLinkModel, bool> setActivePage
)
    : TrustsAreaModel(dataSourceService, trustService, logger, ViewConstants.ContactsPageTitle)
{
    public Person? ChairOfTrustees { get; set; }
    public Person? AccountingOfficer { get; set; }
    public Person? ChiefFinancialOfficer { get; set; }
    public InternalContact? SfsoLead { get; set; }
    public InternalContact? TrustRelationshipManager { get; set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel("In DfE", "./InDfE", Uid, ViewConstants.ContactsPageTitle),
            new TrustSubNavigationLinkModel("In the trust", "./InTrust", Uid, ViewConstants.ContactsPageTitle)
        ];

        SubNavigationLinks.First(setActivePage).LinkIsActive = true;

        (TrustRelationshipManager, SfsoLead, AccountingOfficer, ChairOfTrustees, ChiefFinancialOfficer) =
            await TrustService.GetTrustContactsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(
            new DataSourceServiceModel(Source.FiatDb, TrustRelationshipManager?.LastModifiedAtTime, null,
                TrustRelationshipManager?.LastModifiedByEmail),
            new List<string>
                { ContactRole.TrustRelationshipManager.MapRoleToViewString() }));

        DataSources.Add(new DataSourceListEntry(
            new DataSourceServiceModel(Source.FiatDb, SfsoLead?.LastModifiedAtTime, null,
                SfsoLead?.LastModifiedByEmail),
            new List<string>
                { ContactRole.SfsoLead.MapRoleToViewString() }));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias), new List<string>
            { "Accounting officer name", "Chief financial officer name", "Chair of trustees name" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Mstr),
            new List<string>
                { "Accounting officer email", "Chief financial officer email", "Chair of trustees email" }));

        return pageResult;
    }
}

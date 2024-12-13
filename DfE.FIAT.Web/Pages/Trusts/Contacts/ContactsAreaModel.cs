using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Extensions;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts.Contacts;

public class ContactAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<ContactAreaModel> logger
)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Contacts")
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
            new TrustSubNavigationLinkModel("In DfE", "./InDfE", Uid, PageName, this is InDfeModel),
            new TrustSubNavigationLinkModel("In the trust", "./InTrust", Uid, PageName, this is InTrustModel)
        ];

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

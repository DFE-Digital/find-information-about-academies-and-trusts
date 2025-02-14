using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;

public class ContactsAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<ContactsAreaModel> logger
)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { PageName = ViewConstants.ContactsPageName };

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
            new TrustSubNavigationLinkModel(ViewConstants.ContactsInDfePageName, "./InDfE", Uid,
                TrustPageMetadata.PageName!, this is InDfeModel),
            new TrustSubNavigationLinkModel("In this trust", "./InTrust", Uid, TrustPageMetadata.PageName!,
                this is InTrustModel)
        ];

        (TrustRelationshipManager, SfsoLead, AccountingOfficer, ChairOfTrustees, ChiefFinancialOfficer) =
            await TrustService.GetTrustContactsAsync(Uid);

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);
        var mstrDataSource = await DataSourceService.GetAsync(Source.Mstr);

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(ViewConstants.ContactsInDfePageName, [
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb,
                            TrustRelationshipManager?.LastModifiedAtTime, null,
                            TrustRelationshipManager?.LastModifiedByEmail),
                        ContactRole.TrustRelationshipManager.MapRoleToViewString()),
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb, SfsoLead?.LastModifiedAtTime,
                        null,
                        SfsoLead?.LastModifiedByEmail), ContactRole.SfsoLead.MapRoleToViewString())
                ]
            ),
            new DataSourcePageListEntry("In this trust", [
                    new DataSourceListEntry(giasDataSource, "Accounting officer name"),
                    new DataSourceListEntry(giasDataSource, "Chief financial officer name"),
                    new DataSourceListEntry(giasDataSource, "Chair of trustees name"),
                    new DataSourceListEntry(mstrDataSource, "Accounting officer email"),
                    new DataSourceListEntry(mstrDataSource, "Chief financial officer email"),
                    new DataSourceListEntry(mstrDataSource, "Chair of trustees email")
                ]
            )
        ]);

        return pageResult;
    }
}

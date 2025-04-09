using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class GovernanceAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<GovernanceAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger)
{
    public const string PageName = "Governance";

    public override TrustPageMetadata TrustPageMetadata => base.TrustPageMetadata with { PageName = PageName };

    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel(
                $"{ViewConstants.GovernanceTrustLeadershipPageName} ({TrustGovernance.CurrentTrustLeadership.Length})",
                "./TrustLeadership", Uid, TrustPageMetadata.PageName!, this is TrustLeadershipModel),
            new TrustSubNavigationLinkModel(
                $"{ViewConstants.GovernanceTrusteesPageName} ({TrustGovernance.CurrentTrustees.Length})", "./Trustees",
                Uid,
                TrustPageMetadata.PageName!, this is TrusteesModel),
            new TrustSubNavigationLinkModel(
                $"{ViewConstants.GovernanceMembersPageName} ({TrustGovernance.CurrentMembers.Length})", "./Members",
                Uid,
                TrustPageMetadata.PageName!, this is MembersModel),
            new TrustSubNavigationLinkModel(
                $"{ViewConstants.GovernanceHistoricMembersPageName} ({TrustGovernance.HistoricMembers.Length})",
                "./HistoricMembers", Uid, TrustPageMetadata.PageName!, this is HistoricMembersModel)
        ];

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry(ViewConstants.GovernanceTrustLeadershipPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.GovernanceTrusteesPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.GovernanceMembersPageName,
                [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry(ViewConstants.GovernanceHistoricMembersPageName,
                [new DataSourceListEntry(giasDataSource)])
        ]);

        return pageResult;
    }
}

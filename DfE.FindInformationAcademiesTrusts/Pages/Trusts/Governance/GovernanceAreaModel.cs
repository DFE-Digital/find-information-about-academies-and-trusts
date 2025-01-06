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
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { PageName = "Governance" };

    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel($"Trust leadership ({TrustGovernance.CurrentTrustLeadership.Length})",
                "./TrustLeadership", Uid, TrustPageMetadata.PageName!, this is TrustLeadershipModel),
            new TrustSubNavigationLinkModel($"Trustees ({TrustGovernance.CurrentTrustees.Length})", "./Trustees", Uid,
                TrustPageMetadata.PageName!, this is TrusteesModel),
            new TrustSubNavigationLinkModel($"Members ({TrustGovernance.CurrentMembers.Length})", "./Members", Uid,
                TrustPageMetadata.PageName!, this is MembersModel),
            new TrustSubNavigationLinkModel($"Historic members ({TrustGovernance.HistoricMembers.Length})",
                "./HistoricMembers", Uid, TrustPageMetadata.PageName!, this is HistoricMembersModel)
        ];

        // Add data sources
        var giasDataSource = await DataSourceService.GetAsync(Source.Gias);

        DataSourcesPerPage.AddRange([
            new DataSourcePageListEntry("Trust leadership", [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry("Trustees", [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry("Members", [new DataSourceListEntry(giasDataSource)]),
            new DataSourcePageListEntry("Historic members", [new DataSourceListEntry(giasDataSource)])
        ]);

        return pageResult;
    }
}

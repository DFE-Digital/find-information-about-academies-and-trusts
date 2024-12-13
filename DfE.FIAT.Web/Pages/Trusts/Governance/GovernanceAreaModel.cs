using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts.Governance;

public class GovernanceAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<GovernanceAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Governance")
{
    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel($"Trust leadership ({TrustGovernance.CurrentTrustLeadership.Length})",
                "./TrustLeadership", Uid, PageName, this is TrustLeadershipModel),
            new TrustSubNavigationLinkModel($"Trustees ({TrustGovernance.CurrentTrustees.Length})", "./Trustees", Uid,
                PageName, this is TrusteesModel),
            new TrustSubNavigationLinkModel($"Members ({TrustGovernance.CurrentMembers.Length})", "./Members", Uid,
                PageName, this is MembersModel),
            new TrustSubNavigationLinkModel($"Historic members ({TrustGovernance.HistoricMembers.Length})",
                "./HistoricMembers", Uid, PageName, this is HistoricMembersModel)
        ];

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Governance" }));

        return pageResult;
    }
}

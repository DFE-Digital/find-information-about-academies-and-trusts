using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;

public class GovernanceAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    ILogger<GovernanceAreaModel> logger,
    Func<TrustSubNavigationLinkModel, bool> setActivePage)
    : TrustsAreaModel(dataSourceService, trustService, logger, ViewConstants.GovernancePageTitle)
{
    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel($"Trust Leadership ({TrustGovernance.CurrentTrustLeadership.Length})",
                "./TrustLeadership", Uid, ViewConstants.GovernancePageTitle),
            new TrustSubNavigationLinkModel($"Trustees ({TrustGovernance.CurrentTrustees.Length})", "./Trustees", Uid,
                ViewConstants.GovernancePageTitle),
            new TrustSubNavigationLinkModel($"Members ({TrustGovernance.CurrentMembers.Length})", "./Members", Uid,
                ViewConstants.GovernancePageTitle),
            new TrustSubNavigationLinkModel($"Historic Members ({TrustGovernance.HistoricMembers.Length})",
                "./HistoricMembers", Uid, ViewConstants.GovernancePageTitle)
        ];

        SubNavigationLinks.First(setActivePage).LinkIsActive = true;

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Governance" }));

        return pageResult;
    }
}

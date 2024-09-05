using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class GovernanceModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<GovernanceModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Governance")
{
    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Governance" }));

        return pageResult;
    }
}

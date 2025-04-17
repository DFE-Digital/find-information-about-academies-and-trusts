using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Overview;

public class TrustSummaryModel(
    IDataSourceService dataSourceService,
    ITrustService trustService)
    : OverviewAreaModel(dataSourceService, trustService)
{
    public const string SubPageName = "Trust summary";

    public override PageMetadata PageMetadata => base.PageMetadata with { SubPageName = SubPageName };

    public IEnumerable<(string Authority, int Total)> AcademiesInEachLocalAuthority =>
        TrustOverview.AcademiesByLocalAuthority
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Select(kv => (Authority: kv.Key, Total: kv.Value));
}

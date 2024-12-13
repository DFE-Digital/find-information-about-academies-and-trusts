using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.Web.Pages.Trusts.Overview;

public class TrustSummaryModel(
    IDataSourceService dataSourceService,
    ILogger<TrustSummaryModel> logger,
    ITrustService trustService)
    : OverviewAreaModel(dataSourceService, trustService, logger)
{
    public IEnumerable<(string Authority, int Total)> AcademiesInEachLocalAuthority =>
        TrustOverview.AcademiesByLocalAuthority
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Select(kv => (Authority: kv.Key, Total: kv.Value));
}

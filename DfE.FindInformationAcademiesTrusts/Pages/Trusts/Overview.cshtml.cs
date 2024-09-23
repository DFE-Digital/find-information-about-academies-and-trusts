using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;
public class OverviewModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<OverviewModel> logger,
    ITrustService trustService) : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Overview")
{
    private readonly ITrustService _trustService = trustService;

    public TrustOverviewServiceModel TrustOverview { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        // Fetch the trust overview data
        TrustOverview = await _trustService.GetTrustOverviewAsync(Uid);


        // Add data sources
        DataSources.Add(new DataSourceListEntry(
            await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Trust summary", "Ofsted ratings" }
        ));

        return Page();
    }


    public int TotalAcademies => TrustOverview.TotalAcademies;

    public IEnumerable<(string Authority, int Total)> AcademiesInEachLocalAuthority =>
        TrustOverview.AcademiesByLocalAuthority
            .OrderByDescending(kv => kv.Value)
            .ThenBy(kv => kv.Key)
            .Select(kv => (Authority: kv.Key, Total: kv.Value));

    public int TotalPupilNumbers => TrustOverview.TotalPupilNumbers;

    public int TotalCapacity => TrustOverview.TotalCapacity;

    public int? PercentageFull =>
        TotalCapacity > 0
            ? (int)Math.Round((double)TotalPupilNumbers / TotalCapacity * 100)
            : null;

    // Ofsted Ratings
    public IEnumerable<(OfstedRatingScore Rating, int Total)> OfstedRatings =>
        TrustOverview.OfstedRatings
            .OrderBy(kv => kv.Key)
            .Select(kv => (Rating: kv.Key, Total: kv.Value));

    public int GetNumberOfAcademiesWithOfstedRating(OfstedRatingScore score) =>
        TrustOverview.OfstedRatings.TryGetValue(score, out var total) ? total : 0;
}
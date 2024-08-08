using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class OverviewModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<OverviewModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Overview")
{
    public Trust Trust { get; set; } = default!;

    public IEnumerable<(string? Authority, int Total)> AcademiesInEachLocalAuthority =>
        Trust.Academies
            .GroupBy(x => x.LocalAuthority)
            .Select(c => (Authority: c.Key, Total: c.Count()))
            .OrderByDescending(t => t.Total)
            .ThenBy(t => t.Authority);

    public IEnumerable<(OfstedRatingScore Rating, int Total)> OfstedRatings
        => Trust.Academies
            .GroupBy(x => x.CurrentOfstedRating.OfstedRatingScore)
            .Select(c => (Rating: c.Key, Total: c.Count()));

    public int NumberOfAcademiesInTrust => Trust.Academies.Length;
    public int TotalPupilNumbersInTrust => Trust.Academies.Select(x => x.NumberOfPupils ?? 0).Sum();
    public int TotalPupilCapacityInTrust => Trust.Academies.Select(x => x.SchoolCapacity ?? 0).Sum();

    public int? TotalPercentageCapacityInTrust =>
        TotalPupilCapacityInTrust > 0
            ? (int)Math.Round(TotalPupilNumbersInTrust / (double)TotalPupilCapacityInTrust * 100)
            : null;

    public int GetNumberOfAcademiesWithOfstedRating(OfstedRatingScore score)
    {
        return OfstedRatings.Any(x => x.Rating == score)
            ? OfstedRatings.Single(x => x.Rating == score).Total
            : 0;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Trust summary", "Ofsted ratings" }));

        return pageResult;
    }
}

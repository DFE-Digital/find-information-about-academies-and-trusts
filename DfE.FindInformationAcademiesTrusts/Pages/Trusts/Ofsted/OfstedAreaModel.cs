using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

public class OfstedAreaModel(
    IDataSourceService dataSourceService,
    ITrustService trustService,
    IAcademyService academyService,
    ILogger<OfstedAreaModel> logger,
    Func<TrustSubNavigationLinkModel, bool> setActivePage)
    : TrustsAreaModel(dataSourceService, trustService, logger, ViewConstants.OfstedPageTitle)
{
    public AcademyOfstedServiceModel[] Academies { get; set; } = default!;
    private IAcademyService AcademyService { get; } = academyService;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustOfstedAsync(Uid);

        SubNavigationLinks =
        [
            new TrustSubNavigationLinkModel("Current ratings", "./CurrentRatings", Uid, ViewConstants.OfstedPageTitle),
            new TrustSubNavigationLinkModel("Previous ratings", "./PreviousRatings", Uid,
                ViewConstants.OfstedPageTitle),
            new TrustSubNavigationLinkModel("Important dates", "./ImportantDates", Uid, ViewConstants.OfstedPageTitle),
            new TrustSubNavigationLinkModel("Safeguarding and concerns", "./SafeguardingAndConcerns", Uid,
                ViewConstants.OfstedPageTitle)
        ];

        SubNavigationLinks.First(setActivePage).LinkIsActive = true;

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new[] { "Date joined trust" }));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Mis),
            new[]
            {
                "Current Ofsted rating", "Date of last inspection", "Previous Ofsted rating",
                "Date of previous inspection"
            }));

        return pageResult;
    }

    public string MapOfstedRatingToWording(OfstedRatingScore score) => score switch
    {
        OfstedRatingScore.None => "Not yet inspected",
        OfstedRatingScore.Outstanding => "Outstanding",
        OfstedRatingScore.Good => "Good",
        OfstedRatingScore.RequiresImprovement => "Requires improvement",
        OfstedRatingScore.Inadequate => "Inadequate",
        OfstedRatingScore.NoJudgement => "No Judgement",
        _ => string.Empty
    };
    

    public bool IsCurrentInspectionAfterJoining(AcademyOfstedServiceModel academy) =>
        academy.CurrentOfstedRating.InspectionDate >= academy.DateAcademyJoinedTrust;

    public bool IsPreviousInspectionAfterJoining(AcademyOfstedServiceModel academy) =>
        academy.PreviousOfstedRating.InspectionDate >= academy.DateAcademyJoinedTrust;
}

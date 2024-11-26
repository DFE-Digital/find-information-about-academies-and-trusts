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
    ILogger<OfstedAreaModel> logger)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Ofsted")
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
            new TrustSubNavigationLinkModel("Current ratings", "./CurrentRatings", Uid, PageName,
                this is CurrentRatingsModel),
            new TrustSubNavigationLinkModel("Previous ratings", "./PreviousRatings", Uid,
                PageName, this is PreviousRatingsModel),
            new TrustSubNavigationLinkModel("Important dates", "./ImportantDates", Uid, PageName,
                this is ImportantDatesModel),
            new TrustSubNavigationLinkModel("Safeguarding and concerns", "./SafeguardingAndConcerns", Uid,
                PageName, this is SafeguardingAndConcernsModel)
        ];

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            ["Date joined trust"]));

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Mis),
        [
            "Current Ofsted rating",
            "Date of last inspection",
            "Previous Ofsted rating",
            "Date of previous inspection"
        ]));

        return pageResult;
    }
}

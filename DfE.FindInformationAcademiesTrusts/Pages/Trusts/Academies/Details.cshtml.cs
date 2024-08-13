using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class AcademiesDetailsModel : TrustsAreaModel, IAcademiesAreaModel
{
    public AcademyDetailsServiceModel[] Academies { get; set; } = default!;
    public IOtherServicesLinkBuilder LinkBuilder { get; }
    private IAcademyService AcademyService { get; }

    public AcademiesDetailsModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
        IOtherServicesLinkBuilder linkBuilder, ILogger<AcademiesDetailsModel> logger,
        ITrustService trustService, IAcademyService academyService) : base(trustProvider,
        dataSourceService, trustService, logger, "Academies in this trust")
    {
        PageTitle = "Academies details";
        LinkBuilder = linkBuilder;
        AcademyService = academyService;
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustDetailsAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Details" }));

        return pageResult;
    }

    public string TabName => "Details";
}

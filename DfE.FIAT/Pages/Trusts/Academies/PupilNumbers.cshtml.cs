using DfE.FIAT.Data;
using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.Academy;
using DfE.FIAT.Web.Services.DataSource;
using DfE.FIAT.Web.Services.Export;
using DfE.FIAT.Web.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FIAT.Web.Pages.Trusts.Academies;

public class PupilNumbersModel : AcademiesPageModel
{
    public IAcademyService AcademyService { get; }
    public AcademyPupilNumbersServiceModel[] Academies { get; set; } = default!;

    public PupilNumbersModel(IDataSourceService dataSourceService,
        ILogger<PupilNumbersModel> logger, ITrustService trustService, IAcademyService academyService, IExportService exportService, IDateTimeProvider dateTimeProvider)
        : base(dataSourceService, trustService, exportService, logger, dateTimeProvider)
    {
        AcademyService = academyService;
        PageTitle = "Academies pupil numbers";
        TabName = "Pupil numbers";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Academies = await AcademyService.GetAcademiesInTrustPupilNumbersAsync(Uid);

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Pupil numbers" }));

        return pageResult;
    }

    public static string PhaseAndAgeRangeSortValue(AcademyPupilNumbersServiceModel academy)
    {
        return $"{academy.PhaseOfEducation}{academy.AgeRange.Minimum:D2}{academy.AgeRange.Maximum:D2}";
    }
}

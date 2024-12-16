using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class PupilNumbersModel : AcademiesPageModel
{
    public override TrustPageMetadata TrustPageMetadata =>
        base.TrustPageMetadata with { TabName = "Pupil numbers" };

    public IAcademyService AcademyService { get; }
    public AcademyPupilNumbersServiceModel[] Academies { get; set; } = default!;

    public PupilNumbersModel(IDataSourceService dataSourceService,
        ILogger<PupilNumbersModel> logger, ITrustService trustService, IAcademyService academyService,
        IExportService exportService, IDateTimeProvider dateTimeProvider)
        : base(dataSourceService, trustService, exportService, logger, dateTimeProvider)
    {
        AcademyService = academyService;
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

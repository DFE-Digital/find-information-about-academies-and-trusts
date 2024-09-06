using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class PupilNumbersModel : ExportableAcademiesPageModel, IAcademiesAreaModel
{
    public Trust Trust { get; set; } = default!;

    public PupilNumbersModel(ITrustProvider trustProvider, IDataSourceService dataSourceService,
        ILogger<PupilNumbersModel> logger, ITrustService trustService, IExportService exportService)
        : base(trustProvider, dataSourceService, trustService, exportService, logger)
    {
        PageTitle = "Academies pupil numbers";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        Trust = (await TrustProvider.GetTrustByUidAsync(Uid))!;

        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Pupil numbers" }));

        return pageResult;
    }

    public string TabName => "Pupil numbers";

    public string PhaseAndAgeRangeSortValue(Academy academy)
    {
        return $"{academy.PhaseOfEducation}{academy.AgeRange.Minimum:D2}{academy.AgeRange.Maximum:D2}";
    }
}

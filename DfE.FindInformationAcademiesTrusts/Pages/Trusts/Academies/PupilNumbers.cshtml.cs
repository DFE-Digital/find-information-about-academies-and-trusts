using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

public class PupilNumbersModel : TrustsAreaModel, IAcademiesAreaModel
{
    public PupilNumbersModel(ITrustProvider trustProvider, IDataSourceProvider dataSourceProvider) : base(
        trustProvider, dataSourceProvider, "Academies in this trust")
    {
        PageTitle = "Academies pupil numbers";
    }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        var giasSource = await GetGiasDataUpdated();
        DataSources = new[] { new DataSourceListEntry(giasSource!, "Pupil numbers") };
        return pageResult;
    }

    public string TabName => "Pupil numbers";

    public string PhaseAndAgeRangeSortValue(Academy academy)
    {
        return $"{academy.PhaseOfEducation}{academy.AgeRange.Minimum:D2}{academy.AgeRange.Maximum:D2}";
    }
}

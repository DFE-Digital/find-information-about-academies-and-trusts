using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools;

public class SchoolAreaModel(ISchoolService schoolService) : BasePageModel, ISchoolAreaModel
{
    [BindProperty(SupportsGet = true)] public string Urn { get; set; } = "";

    public List<DataSourcePageListEntry> DataSourcesPerPage { get; set; } = [];
    public virtual PageMetadata PageMetadata => new(SchoolName, ModelState.IsValid);

    public SchoolSummaryServiceModel SchoolSummary { get; set; } = null!;
    public string SchoolName => SchoolSummary.Name;
    public string SchoolType => SchoolSummary.Type;
    public SchoolCategory SchoolCategory => SchoolSummary.Category;

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var schoolSummary = await schoolService.GetSchoolSummaryAsync(Urn);

        if (schoolSummary == null)
        {
            return new NotFoundResult();
        }

        SchoolSummary = schoolSummary;

        return Page();
    }
}

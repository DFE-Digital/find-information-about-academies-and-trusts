using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools;

public class SchoolAreaModel(ISchoolService schoolService, ITrustService trustService) : BasePageModel, ISchoolAreaModel
{
    [BindProperty(SupportsGet = true)] public int Urn { get; set; }

    public List<DataSourcePageListEntry> DataSourcesPerPage { get; set; } = [];
    public virtual PageMetadata PageMetadata => new(SchoolName, ModelState.IsValid);

    public SchoolSummaryServiceModel SchoolSummary { get; set; } = null!;
    public string SchoolName => SchoolSummary.Name;
    public string SchoolType => SchoolSummary.Type;
    public SchoolCategory SchoolCategory => SchoolSummary.Category;

    public TrustSummaryServiceModel? TrustSummary { get; private set; }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        var schoolSummary = await schoolService.GetSchoolSummaryAsync(Urn);

        if (schoolSummary == null)
        {
            return new NotFoundResult();
        }

        if (schoolSummary.Category == SchoolCategory.Academy)
        {
            TrustSummary = await trustService.GetTrustSummaryAsync(schoolSummary.Urn);
        }

        SchoolSummary = schoolSummary;

        return Page();
    }
}

using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public abstract class OverviewAreaModel(ISchoolService schoolService) : SchoolAreaModel(schoolService)
{
    public const string PageName = "Overview";
    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };
}

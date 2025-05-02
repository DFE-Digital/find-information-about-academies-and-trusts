using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.School;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public abstract class OverviewAreaModel(ISchoolService schoolService, ITrustService trustService) : SchoolAreaModel(schoolService, trustService)
{
    public const string PageName = "Overview";
    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };
}

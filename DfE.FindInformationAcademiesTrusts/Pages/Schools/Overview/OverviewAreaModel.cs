using DfE.FindInformationAcademiesTrusts.Pages.Shared;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class OverviewAreaModel : SchoolAreaModel
{
    public const string PageName = "Overview";
    public override PageMetadata PageMetadata => base.PageMetadata with { PageName = PageName };
}

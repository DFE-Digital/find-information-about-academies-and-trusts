using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class DetailsModel : OverviewAreaModel
{
    public override PageMetadata PageMetadata => base.PageMetadata with
    {
        SubPageName = SubPageName(SchoolCategory)
    };

    public static string SubPageName(SchoolCategory schoolCategory)
    {
        return schoolCategory switch
        {
            SchoolCategory.LaMaintainedSchool => "School details",
            SchoolCategory.Academy => "Academy details",
            _ => throw new ArgumentOutOfRangeException(nameof(schoolCategory))
        };
    }
}

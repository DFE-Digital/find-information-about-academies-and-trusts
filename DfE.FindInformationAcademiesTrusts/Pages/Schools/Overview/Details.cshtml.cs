using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.School;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

public class DetailsModel(ISchoolService schoolService) : OverviewAreaModel(schoolService)
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

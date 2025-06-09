using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared.NavMenu;

public record NavLink(
    bool LinkIsActive,
    string? VisuallyHiddenLinkText,
    string LinkDisplayText,
    string AspPage,
    string TestId,
    Dictionary<string, string> AspAllRouteData,
    SchoolCategory? WhichSchoolCategory = null)
{
    public bool ShowNavLink => WhichSchoolCategory != SchoolCategory.Academy;
}

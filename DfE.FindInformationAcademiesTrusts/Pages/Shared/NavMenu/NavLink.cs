namespace DfE.FindInformationAcademiesTrusts.Pages.Shared.NavMenu;

public record NavLink(
    bool LinkIsActive,
    string? VisuallyHiddenLinkText,
    string LinkDisplayText,
    string AspPage,
    string TestId,
    Dictionary<string, string> AspAllRouteData);

using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using DfE.FindInformationAcademiesTrusts.Pages.Shared.NavMenu;

namespace DfE.FindInformationAcademiesTrusts.Pages.Schools;

public static class SchoolNavMenu
{
    public static NavLink[] GetServiceNavLinks(ISchoolAreaModel activePage)
    {
        return
        [
            GetServiceNavLinkTo<OverviewAreaModel>(OverviewAreaModel.PageName, "/Schools/Overview/Details",
                activePage)
        ];
    }

    private static NavLink GetServiceNavLinkTo<T>(string linkDisplayText, string aspPage, ISchoolAreaModel activePage)
    {
        return new NavLink(activePage is T,
            activePage.SchoolCategory.ToDisplayString(),
            linkDisplayText,
            aspPage,
            $"{linkDisplayText}-nav".Kebabify(),
            new Dictionary<string, string> { { "urn", activePage.Urn.ToString() } });
    }

    public static NavLink[] GetSubNavLinks(ISchoolAreaModel activePage)
    {
        return activePage switch
        {
            OverviewAreaModel =>
            [
                GetSubNavLinkTo<DetailsModel>(
                    OverviewAreaModel.PageName,
                    DetailsModel.SubPageName(activePage.SchoolCategory),
                    "/Schools/Overview/Details",
                    activePage,
                    "overview-details-subnav"
                ),
                GetSubNavLinkTo<FederationModel>(
                    OverviewAreaModel.PageName,
                    FederationModel.SubPageName,
                    "/Schools/Overview/Federation",
                    activePage,
                    "overview-federation-subnav"),
                GetSubNavLinkTo<SenModel>(
                    OverviewAreaModel.PageName,
                    SenModel.SubPageName,
                    "/Schools/Overview/Sen",
                    activePage,
                    "overview-sen-subnav")
            ],
            _ => throw new ArgumentOutOfRangeException(nameof(activePage), activePage, "Page type is not supported.")
        };
    }

    private static NavLink GetSubNavLinkTo<T>(string serviceName, string linkDisplayText, string aspPage,
        ISchoolAreaModel activePage, string? testIdOverride = null)
    {
        return new NavLink(
            activePage is T,
            serviceName,
            linkDisplayText,
            aspPage,
            testIdOverride ?? $"{serviceName}-{linkDisplayText}-subnav".Kebabify(),
            new Dictionary<string, string> { { "urn", activePage.Urn.ToString() } }
        );
    }
}

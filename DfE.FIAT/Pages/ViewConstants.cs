using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Pages;

[ExcludeFromCodeCoverage]
public static class ViewConstants
{
    public const string ServiceName = "Find information about academies and trusts";
    public const string AboutTheTrustSectionName = "About the trust";

    public const string ReportAProblemMailToLink =
        "mailto:regionalservices.rg@education.gov.uk?subject=Report a problem with Find information about academies and trusts";

    public const string ReportNotFoundMailtoLink =
        "mailto:regionalservices.rg@education.gov.uk?subject=Page not found – Find information about academies and trusts (FIAT)";

    public const string GetHelpFormLink =
        "https://forms.office.com/Pages/ResponsePage.aspx?id=yXfS-grGoU2187O4s0qC-X7F89QcWu5CjlJXwF0TVktUMTFEUVRCVVg4WlMyS1AzUEJSUDAySlhQTCQlQCN0PWcu";

    public const string FeedbackFormLink =
        "https://forms.office.com/Pages/ResponsePage.aspx?id=yXfS-grGoU2187O4s0qC-SZtRygfwTNOqcfRq-MXpv9UOTIyQlNYR0hJT1Q0TUFVSlJGVFhES01LVC4u";

    public const string SuggestChangeFormLink =
        "https://forms.office.com/Pages/ResponsePage.aspx?id=yXfS-grGoU2187O4s0qC-fkHK2JGo_BIpVChpLMaBFpUNUFDSzhQN0FHVklTV0JWTzFZTjNKWTNJUi4u";

    public const string NoDataText = "No Data";

    public static readonly List<ExternalServiceLink> ExternalServiceLinks =
    [
        new ExternalServiceLink("Prepare conversions and transfers",
            "Create a transfer or conversion project document for an advisory board.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Prepare-Conversions-and-Transfers.aspx"),

        new ExternalServiceLink("Complete conversions, transfers and changes",
            "Manage a conversion or transfer project after it has been to an advisory board.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/complete-conversions-transfers-and-changes.aspx"),

        new ExternalServiceLink("Record concerns or support for trusts",
            "Add cases or interactions, record risks and log support and concerns for trusts.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Record-concerns-and-support-for-trusts.aspx"),

        new ExternalServiceLink("Manage free school projects",
            "Manage presumption projects in the pre-opening phase.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Manage-free-school-projects.aspx?web=1"),

        new ExternalServiceLink("Reporting and data tools",
            "Tools to help you gather educational data and create reports.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/reportinganddata.aspx"),

        new ExternalServiceLink("High quality trust framework",
            "Process guidance and tools for making trust-related project decisions.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/RG%20high%20quality%20trust%20framework.aspx")
    ];
}

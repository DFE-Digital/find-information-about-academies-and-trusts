using DfE.FindInformationAcademiesTrusts.Data;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Pages;

[ExcludeFromCodeCoverage]
public static class ViewConstants
{
    public const string ServiceName = "Find information about academies and trusts";

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

    public const string NoDataText = "No data";
    public const string UnconfirmedDateText = "Unconfirmed";

    public static readonly List<ExternalServiceLink> ExternalServiceLinks =
    [
        new("Prepare conversions and transfers",
            "Create a transfer or conversion project document for an advisory board.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Prepare-Conversions-and-Transfers.aspx"),

        new("Complete conversions, transfers and changes",
            "Manage a conversion or transfer project after it has been to an advisory board.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/complete-conversions-transfers-and-changes.aspx"),

        new("Record concerns or support for trusts",
            "Add cases or interactions, record risks and log support and concerns for trusts.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Record-concerns-and-support-for-trusts.aspx"),

        new("Manage free school projects",
            "Manage presumption projects in the pre-opening phase.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/Manage-free-school-projects.aspx?web=1"),

        new("Reporting and data tools",
            "Tools to help you gather educational data and create reports.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/reportinganddata.aspx"),

        new("High quality trust framework",
            "Process guidance and tools for making trust-related project decisions.",
            "https://educationgovuk.sharepoint.com/sites/lvewp00299/SitePages/RG%20high%20quality%20trust%20framework.aspx")
    ];

    public const string OverviewPageName = "Overview";
    public const string OverviewTrustDetailsPageName = "Trust details";
    public const string OverviewTrustSummaryPageName = "Trust summary";
    public const string OverviewReferenceNumbersPageName = "Reference numbers";

    public const string ContactsPageName = "Contacts";
    public const string ContactsInDfePageName = "In DfE";
    public const string ContactsInTrustPageName = "In this trust";

    public const string OfstedPageName = "Ofsted";
    public const string OfstedSingleHeadlineGradesPageName = "Single headline grades";
    public const string OfstedCurrentRatingsPageName = "Current ratings";
    public const string OfstedPreviousRatingsPageName = "Previous ratings";
    public const string OfstedSafeguardingAndConcernsPageName = "Safeguarding and concerns";

    public const string GovernancePageName = "Governance";
    public const string GovernanceTrustLeadershipPageName = "Trust leadership";
    public const string GovernanceTrusteesPageName = "Trustees";
    public const string GovernanceMembersPageName = "Members";
    public const string GovernanceHistoricMembersPageName = "Historic members";

    public const string AcademiesPageName = "Academies";
    public const string AcademiesInTrustSubNavName = "In this trust";
    public const string PipelineAcademiesSubNavName = "Pipeline";
    public const string AcademiesInTrustDetailsPageName = "Details";
    public const string AcademiesInTrustPupilNumbersPageName = "Pupil numbers";
    public const string AcademiesInTrustFreeSchoolMealsPageName = "Free school meals";
    public const string PipelineAcademiesPreAdvisoryBoardPageName = "Pre advisory board";
    public const string PipelineAcademiesPostAdvisoryBoardPageName = "Post advisory board";
    public const string PipelineAcademiesFreeSchoolsPageName = "Free schools";
}

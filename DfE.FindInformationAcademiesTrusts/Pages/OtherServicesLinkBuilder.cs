using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? CompaniesHouseListingLink(string? companiesHouseNumber);
    string? FindSchoolPerformanceDataListingLink(string uid, TrustType trustType, string? satAcademyUrn);
    string GetInformationAboutSchoolsListingLinkForTrust(string trustUid);
    string GetInformationAboutSchoolsListingLinkForAcademy(string urn);
    string? SharepointFolderLink(string groupId);

    string? FinancialBenchmarkingInsightsToolListingLink(string? companiesHouseNumber);
}

public class OtherServicesLinkBuilder : IOtherServicesLinkBuilder
{
    private const string GetInformationAboutSchoolsBaseUrl = "https://www.get-information-schools.service.gov.uk";

    private const string CompaniesHouseBaseUrl =
        "https://find-and-update.company-information.service.gov.uk";

    private const string FinancialBenchmarkingInsightsToolBaseUrl =
        "https://financial-benchmarking-and-insights-tool.education.gov.uk";

    private const string FindSchoolPerformanceDataBaseUrl =
        "https://www.find-school-performance-data.service.gov.uk";

    private const string SharepointBaseUrl = "https://educationgovuk.sharepoint.com";

    public string GetInformationAboutSchoolsListingLinkForTrust(string trustUid)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Groups/Group/Details/{trustUid}";
    }

    public string GetInformationAboutSchoolsListingLinkForAcademy(string urn)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Establishments/Establishment/Details/{urn}";
    }

    public string? CompaniesHouseListingLink(string? companiesHouseNumber)
    {
        return string.IsNullOrEmpty(companiesHouseNumber)
            ? null
            : $"{CompaniesHouseBaseUrl}/company/{companiesHouseNumber}";
    }

    public string? FinancialBenchmarkingInsightsToolListingLink(string? companiesHouseNumber)
    {
        return string.IsNullOrEmpty(companiesHouseNumber)
            ? null
            : $"{FinancialBenchmarkingInsightsToolBaseUrl}/trust/{companiesHouseNumber}";
    }

    public string? FindSchoolPerformanceDataListingLink(string uid, TrustType trustType, string? satAcademyUrn)
    {
        return trustType switch
        {
            TrustType.MultiAcademyTrust => $"{FindSchoolPerformanceDataBaseUrl}/multi-academy-trust/{uid}",
            TrustType.SingleAcademyTrust when satAcademyUrn is not null =>
                $"{FindSchoolPerformanceDataBaseUrl}/school/{satAcademyUrn}",
            _ => null
        };
    }

    public string? SharepointFolderLink(string groupId)
    {
        if (string.IsNullOrEmpty(groupId)) return null;
        return
            $"{SharepointBaseUrl}/_layouts/15/sharepoint.aspx?oobRefiners=%7B%22FileType%22%3A%5B%22other%22%5D%7D&q={groupId}&v=%2Fsearch";
    }
}

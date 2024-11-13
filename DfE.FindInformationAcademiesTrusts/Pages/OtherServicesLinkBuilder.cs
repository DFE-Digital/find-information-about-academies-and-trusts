using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? CompaniesHouseListingLink(string? companiesHouseNumber);
    string? FindSchoolPerformanceDataListingLink(string uid, TrustType trustType, string? satAcademyUrn);
    string GetInformationAboutSchoolsListingLinkForTrust(string trustUid);
    string GetInformationAboutSchoolsListingLinkForAcademy(string urn);

    string? SchoolFinancialBenchmarkingServiceListingLink(TrustType trustType, string? satAcademyUrn,
        string? companiesHouseNumber);
}

public class OtherServicesLinkBuilder : IOtherServicesLinkBuilder
{
    private const string GetInformationAboutSchoolsBaseUrl = "https://www.get-information-schools.service.gov.uk";

    private const string CompaniesHouseBaseUrl =
        "https://find-and-update.company-information.service.gov.uk";

    private const string SchoolFinancialBenchmarkingServiceBaseUrl =
        "https://schools-financial-benchmarking.service.gov.uk";

    private const string FindSchoolPerformanceDataBaseUrl =
        "https://www.find-school-performance-data.service.gov.uk";

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

    public string? SchoolFinancialBenchmarkingServiceListingLink(TrustType trustType, string? satAcademyUrn,
        string? companiesHouseNumber)
    {
        return trustType switch
        {
            TrustType.MultiAcademyTrust when companiesHouseNumber is not null =>
                $"{SchoolFinancialBenchmarkingServiceBaseUrl}/Trust?companyNo={companiesHouseNumber}",
            TrustType.SingleAcademyTrust when satAcademyUrn is not null =>
                $"{SchoolFinancialBenchmarkingServiceBaseUrl}/school?urn={satAcademyUrn}",
            _ => null
        };
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
}

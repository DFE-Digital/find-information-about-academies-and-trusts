using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? GetInformationAboutSchoolsListingLink(Trust trust);
    string? CompaniesHouseListingLink(Trust trust);
    string? SchoolFinancialBenchmarkingServiceListingLink(Trust trust);
    string? FindSchoolPerformanceDataListingLink(Trust trust);
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

    public string? GetInformationAboutSchoolsListingLink(Trust trust)
    {
        if (trust.IsOpen())
        {
            return $"{GetInformationAboutSchoolsBaseUrl}/Groups/Group/Details/{trust.Uid}";
        }

        return null;
    }

    public string CompaniesHouseListingLink(Trust trust)
    {
        return $"{CompaniesHouseBaseUrl}/company/{trust.CompaniesHouseNumber}";
    }

    public string? SchoolFinancialBenchmarkingServiceListingLink(Trust trust)
    {
        if (trust.IsOpen() && trust.IsMultiAcademyTrust())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/Trust?companyNo={trust.CompaniesHouseNumber}";
        }

        if (trust.IsOpen() && trust.IsSingleAcademyTrust() && trust.Academies.Any())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/school?urn={trust.FirstAcademyUrn()}";
        }

        return null;
    }

    public string? FindSchoolPerformanceDataListingLink(Trust trust)
    {
        if (trust.IsOpen() && trust.IsMultiAcademyTrust())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/multi-academy-trust/{trust.Uid}";
        }

        if (trust.IsOpen() && trust.IsSingleAcademyTrust() && trust.Academies.Any())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/school/{trust.FirstAcademyUrn()}";
        }

        return null;
    }
}

using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? GetInformationAboutSchoolsListingLink(Trust trust);
    string GetInformationAboutSchoolsListingLink(Academy academy);
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
        return $"{GetInformationAboutSchoolsBaseUrl}/Groups/Group/Details/{trust.Uid}";
    }

    public string GetInformationAboutSchoolsListingLink(Academy academy)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Establishments/Establishment/Details/{academy.Urn}";
    }

    public string? CompaniesHouseListingLink(Trust trust)
    {
        if (string.IsNullOrEmpty(trust.CompaniesHouseNumber)) return null;
        return $"{CompaniesHouseBaseUrl}/company/{trust.CompaniesHouseNumber}";
    }

    public string? SchoolFinancialBenchmarkingServiceListingLink(Trust trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/Trust?companyNo={trust.CompaniesHouseNumber}";
        }

        if (trust.IsSingleAcademyTrust() && trust.Academies.Any())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/school?urn={trust.Academies[0].Urn}";
        }

        return null;
    }

    public string? FindSchoolPerformanceDataListingLink(Trust trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/multi-academy-trust/{trust.Uid}";
        }

        if (trust.IsSingleAcademyTrust() && trust.Academies.Any())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/school/{trust.Academies[0].Urn}";
        }

        return null;
    }
}

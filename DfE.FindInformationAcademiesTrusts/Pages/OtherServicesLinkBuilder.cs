using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.ServiceModels;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? GetInformationAboutSchoolsListingLink(TrustDetailsServiceModel trust);
    string GetInformationAboutSchoolsListingLink(Academy academy);
    string? CompaniesHouseListingLink(TrustDetailsServiceModel trust);
    string? SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsServiceModel trust);
    string? FindSchoolPerformanceDataListingLink(TrustDetailsServiceModel trust);
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

    public string GetInformationAboutSchoolsListingLink(TrustDetailsServiceModel trust)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Groups/Group/Details/{trust.Uid}";
    }

    public string GetInformationAboutSchoolsListingLink(Academy academy)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Establishments/Establishment/Details/{academy.Urn}";
    }

    public string? CompaniesHouseListingLink(TrustDetailsServiceModel trust)
    {
        if (string.IsNullOrEmpty(trust.CompaniesHouseNumber)) return null;
        return $"{CompaniesHouseBaseUrl}/company/{trust.CompaniesHouseNumber}";
    }

    public string? SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsServiceModel trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/Trust?companyNo={trust.CompaniesHouseNumber}";
        }

        if (trust.IsSingleAcademyTrust() && trust.SingleAcademyUrn is not null)
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/school?urn={trust.SingleAcademyUrn}";
        }

        return null;
    }

    public string? FindSchoolPerformanceDataListingLink(TrustDetailsServiceModel trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/multi-academy-trust/{trust.Uid}";
        }

        if (trust.IsSingleAcademyTrust() && trust.SingleAcademyUrn is not null)
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/school/{trust.SingleAcademyUrn}";
        }

        return null;
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Dto;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string? GetInformationAboutSchoolsListingLink(TrustDetailsDto trust);
    string GetInformationAboutSchoolsListingLink(Academy academy);
    string? CompaniesHouseListingLink(TrustDetailsDto trust);
    string? SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsDto trust);
    string? FindSchoolPerformanceDataListingLink(TrustDetailsDto trust);
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

    public string GetInformationAboutSchoolsListingLink(TrustDetailsDto trust)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Groups/Group/Details/{trust.Uid}";
    }

    public string GetInformationAboutSchoolsListingLink(Academy academy)
    {
        return $"{GetInformationAboutSchoolsBaseUrl}/Establishments/Establishment/Details/{academy.Urn}";
    }

    public string? CompaniesHouseListingLink(TrustDetailsDto trust)
    {
        if (string.IsNullOrEmpty(trust.CompaniesHouseNumber)) return null;
        return $"{CompaniesHouseBaseUrl}/company/{trust.CompaniesHouseNumber}";
    }

    public string? SchoolFinancialBenchmarkingServiceListingLink(TrustDetailsDto trust)
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

    public string? FindSchoolPerformanceDataListingLink(TrustDetailsDto trust)
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

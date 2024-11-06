using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface IOtherServicesLinkBuilder
{
    string GetInformationAboutSchoolsListingLinkForTrust(string trustUid);
    string GetInformationAboutSchoolsListingLinkForAcademy(string urn);
    string? CompaniesHouseListingLink(TrustOverviewServiceModel trust);
    string? SchoolFinancialBenchmarkingServiceListingLink(TrustOverviewServiceModel trust);
    string? FindSchoolPerformanceDataListingLink(TrustOverviewServiceModel trust);
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

    public string? CompaniesHouseListingLink(TrustOverviewServiceModel trust)
    {
        if (string.IsNullOrEmpty(trust.CompaniesHouseNumber)) return null;
        return $"{CompaniesHouseBaseUrl}/company/{trust.CompaniesHouseNumber}";
    }

    public string? SchoolFinancialBenchmarkingServiceListingLink(TrustOverviewServiceModel trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/Trust?companyNo={trust.CompaniesHouseNumber}";
        }

        if (trust.IsSingleAcademyTrust() && trust.SingleAcademyTrustAcademyUrn is not null)
        {
            return $"{SchoolFinancialBenchmarkingServiceBaseUrl}/school?urn={trust.SingleAcademyTrustAcademyUrn}";
        }

        return null;
    }

    public string? FindSchoolPerformanceDataListingLink(TrustOverviewServiceModel trust)
    {
        if (trust.IsMultiAcademyTrust())
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/multi-academy-trust/{trust.Uid}";
        }

        if (trust.IsSingleAcademyTrust() && trust.SingleAcademyTrustAcademyUrn is not null)
        {
            return $"{FindSchoolPerformanceDataBaseUrl}/school/{trust.SingleAcademyTrustAcademyUrn}";
        }

        return null;
    }
}

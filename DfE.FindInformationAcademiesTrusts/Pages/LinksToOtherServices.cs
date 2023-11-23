namespace DfE.FindInformationAcademiesTrusts.Pages;

public interface ILinksToOtherServices
{
    string GetInformationAboutSchoolsGroupLink(string uid);
    string CompaniesHouseCompaniesLink(string companiesHouseNumber);
    string SchoolFinancialBenchmarkingServiceTrustLink(string companiesHouseNumber);
    string SchoolFinancialBenchmarkingServiceSchoolLink(string urn);
    string FindSchoolPerformanceDataTrustLink(string uid);
    string FindSchoolPerformanceDataSchoolLink(string urn);
}

public class LinksToOtherServices : ILinksToOtherServices
{
    private const string GetInformationAboutSchoolsLink = "https://www.get-information-schools.service.gov.uk";

    private const string CompaniesHouseLink =
        "https://find-and-update.company-information.service.gov.uk";

    private const string SchoolFinancialBenchmarkingServiceLink =
        "https://schools-financial-benchmarking.service.gov.uk";

    private const string FindSchoolPerformanceDataLink =
        "https://www.find-school-performance-data.service.gov.uk";

    public string GetInformationAboutSchoolsGroupLink(string uid)
    {
        return $"{GetInformationAboutSchoolsLink}/Groups/Group/Details/{uid}";
    }

    public string CompaniesHouseCompaniesLink(string companiesHouseNumber)
    {
        return $"{CompaniesHouseLink}/company/{companiesHouseNumber}";
    }

    public string SchoolFinancialBenchmarkingServiceTrustLink(string companiesHouseNumber)
    {
        return $"{SchoolFinancialBenchmarkingServiceLink}/Trust?companyNo={companiesHouseNumber}";
    }

    public string SchoolFinancialBenchmarkingServiceSchoolLink(string urn)
    {
        return $"{SchoolFinancialBenchmarkingServiceLink}/school?urn={urn}";
    }

    public string FindSchoolPerformanceDataTrustLink(string uid)
    {
        return $"{FindSchoolPerformanceDataLink}/multi-academy-trust/{uid}";
    }

    public string FindSchoolPerformanceDataSchoolLink(string urn)
    {
        return $"{FindSchoolPerformanceDataLink}/school/{urn}";
    }
}

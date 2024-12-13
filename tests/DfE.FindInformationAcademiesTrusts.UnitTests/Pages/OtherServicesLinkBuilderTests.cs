using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class OtherServicesLinkBuilderTests
{
    private readonly OtherServicesLinkBuilder _sut = new();

    [Fact]
    public void CompaniesHouseListingLink_should_return_url_containing_CompaniesHouseNumber()
    {
        var result = _sut.CompaniesHouseListingLink("2345");

        result.Should()
            .Be("https://find-and-update.company-information.service.gov.uk/company/2345");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CompaniesHouseListingLink_should_return_null_if_trust_has_no_CompaniesHouseNumber(
        string? companiesHouseNumber)
    {
        var result = _sut.CompaniesHouseListingLink(companiesHouseNumber);
        result.Should().BeNull();
    }

    [Fact]
    public void GetInformationAboutSchoolsListingLink_should_return_url_containing_trust_uid()
    {
        var result = _sut.GetInformationAboutSchoolsListingLinkForTrust("1234");
        result.Should().Be("https://www.get-information-schools.service.gov.uk/Groups/Group/Details/1234");
    }

    [Fact]
    public void GetInformationAboutSchoolsListingLink_should_return_academy_urn_in_link()
    {
        var result = _sut.GetInformationAboutSchoolsListingLinkForAcademy("111");
        result.Should()
            .Be("https://www.get-information-schools.service.gov.uk/Establishments/Establishment/Details/111");
    }

    [Fact]
    public void FinancialBenchmarkingInsightsToolListingLink_should_be_to_the_correct_link()
    {
        var result = _sut.FinancialBenchmarkingInsightsToolListingLink("1111");
        result.Should().Be("https://financial-benchmarking-and-insights-tool.education.gov.uk/trust/1111");
    }

    [Fact]
    public void FinancialBenchmarkingInsightsToolListingLink_should_be_null_companies_house_is_null()
    {
        var result = _sut.FinancialBenchmarkingInsightsToolListingLink(null);
        result.Should().BeNull();
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var result =
            _sut.FindSchoolPerformanceDataListingLink("1234", TrustType.MultiAcademyTrust, null);
        result.Should().Be("https://www.find-school-performance-data.service.gov.uk/multi-academy-trust/1234");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var result =
            _sut.FindSchoolPerformanceDataListingLink("1234", TrustType.SingleAcademyTrust, "1111");
        result.Should().Be("https://www.find-school-performance-data.service.gov.uk/school/1111");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_null_if_trust_is_single_academy_type_with_no_academies()
    {
        var result =
            _sut.FindSchoolPerformanceDataListingLink("1234", TrustType.SingleAcademyTrust, null);
        result.Should().BeNull();
    }
}

using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.ServiceModels;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class OtherServicesLinkBuilderTests
{
    private readonly OtherServicesLinkBuilder _sut = new();

    private static readonly TrustDetailsServiceModel DummyTrustDetailsServiceModel =
        new("", "", "", "", "", "", "", null, null);

    [Fact]
    public void CompaniesHouseListingLink_should_return_url_containing_CompaniesHouseNumber()
    {
        var result =
            _sut.CompaniesHouseListingLink(DummyTrustDetailsServiceModel with { CompaniesHouseNumber = "2345" });

        result.Should()
            .Be("https://find-and-update.company-information.service.gov.uk/company/2345");
    }

    [Fact]
    public void CompaniesHouseListingLink_should_return_null_if_trust_has_no_CompaniesHouseNumber()
    {
        var result = _sut.CompaniesHouseListingLink(DummyTrustDetailsServiceModel with { CompaniesHouseNumber = "" });
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
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var result =
            _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsServiceModel with
            {
                Type = "Multi-academy trust",
                CompaniesHouseNumber = "2345"
            });
        result.Should()
            .Be("https://schools-financial-benchmarking.service.gov.uk/Trust?companyNo=2345");
    }

    [Fact]
    public void SchoolFinancialBenchmarkingListingLink_should_be_to_school_page_if_single_academy_trust_with_academies()
    {
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsServiceModel with
        {
            Type = "Single-academy trust", SingleAcademyTrustAcademyUrn = "1111"
        });
        result.Should().Be("https://schools-financial-benchmarking.service.gov.uk/school?urn=1111");
    }

    [Fact]
    public void SchoolFinancialBenchmarkingListingLink_should_be_null_if_single_academy_type_with_no_academies()
    {
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsServiceModel with
        {
            Type = "Single-academy trust", SingleAcademyTrustAcademyUrn = null
        });
        result.Should().BeNull();
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var result =
            _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsServiceModel with
            {
                Uid = "1234", Type = "Multi-academy trust"
            });
        result.Should().Be("https://www.find-school-performance-data.service.gov.uk/multi-academy-trust/1234");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var result = _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsServiceModel with
        {
            Type = "Single-academy trust", SingleAcademyTrustAcademyUrn = "1111"
        });
        result.Should().Be("https://www.find-school-performance-data.service.gov.uk/school/1111");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_null_if_trust_is_single_academy_type_with_no_trusts()
    {
        var result = _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsServiceModel with
        {
            Type = "Single-academy trust", SingleAcademyTrustAcademyUrn = null
        });
        result.Should().BeNull();
    }
}

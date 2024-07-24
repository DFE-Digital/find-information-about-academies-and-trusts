using DfE.FindInformationAcademiesTrusts.Data.Dto;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class OtherServicesLinkBuilderTests
{
    private readonly OtherServicesLinkBuilder _sut = new();
    private static readonly TrustDetailsDto DummyTrustDetailsDto = new("", "", "", "", "", "", "", null, null);

    [Fact]
    public void CompaniesHouseListingLink_should_return_url_containing_CompaniesHouseNumber()
    {
        var result = _sut.CompaniesHouseListingLink(DummyTrustDetailsDto with { CompaniesHouseNumber = "2345" });
        result.Should().Contain("/company/2345");
    }

    [Fact]
    public void CompaniesHouseListingLink_should_return_null_if_trust_has_no_CompaniesHouseNumber()
    {
        var result = _sut.CompaniesHouseListingLink(DummyTrustDetailsDto with { CompaniesHouseNumber = "" });
        result.Should().BeNull();
    }

    [Fact]
    public void GetInformationAboutSchoolsListingLink_should_return_url_containing_trust_uid_if_trust_is_open()
    {
        var result = _sut.GetInformationAboutSchoolsListingLink(DummyTrustDetailsDto with { Uid = "1234" });
        result.Should().Contain("1234");
    }

    [Fact]
    public void GetInformationAboutSchoolsListingLink_should_return_academy_urn_if_academy_is_provided()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(111);
        var result = _sut.GetInformationAboutSchoolsListingLink(dummyAcademy);
        result.Should().Contain("/Establishments/Establishment/Details/111");
    }

    [Fact]
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var result =
            _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsDto with
            {
                Type = "Multi-academy trust",
                CompaniesHouseNumber = "2345"
            });
        result.Should().Contain("/Trust?companyNo=2345");
    }

    [Fact]
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsDto with
        {
            Type = "Single-academy trust", SingleAcademyUrn = 1111
        });
        result.Should().Contain("/school?urn=1111");
    }

    [Fact]
    public void SchoolFinancialBenchmarkingListingLink_should_be_null_if_single_academy_type_with_no_academies()
    {
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsDto with
        {
            Type = "Single-academy trust", SingleAcademyUrn = null
        });
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("test", null)]
    [InlineData("test", 2222)]
    [InlineData("", 1111)]
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_null_if_type_is_neither_multi_or_single_academy_trust(
            string type, int? singleAcademyUrn)
    {
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsDto with
        {
            Type = type, SingleAcademyUrn = singleAcademyUrn
        });
        result.Should().BeNull();
    }

    [Fact]
    public void
        FindSchoolPerformanceDataListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var result =
            _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsDto with
            {
                Uid = "1234", Type = "Multi-academy trust"
            });
        result.Should().Contain("/multi-academy-trust/1234");
    }

    [Fact]
    public void
        FindSchoolPerformanceDataListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var result = _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsDto with
        {
            Type = "Single-academy trust", SingleAcademyUrn = 1111
        });
        result.Should().Contain("/school/1111");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_null_if_trust_is_single_academy_type_with_no_trusts()
    {
        var result = _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsDto with
        {
            Type = "Single-academy trust", SingleAcademyUrn = null
        });
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("test", null)]
    [InlineData("test", 2222)]
    [InlineData("", 1)]
    public void
        FindSchoolPerformanceDataListingLink_should_be_null_if_type_is_neither_multi_or_single_academy_trust(
            string type, int? singleAcademyUrn)
    {
        var result = _sut.FindSchoolPerformanceDataListingLink(DummyTrustDetailsDto with
        {
            Type = type, SingleAcademyUrn = singleAcademyUrn
        });
        result.Should().BeNull();
    }
}

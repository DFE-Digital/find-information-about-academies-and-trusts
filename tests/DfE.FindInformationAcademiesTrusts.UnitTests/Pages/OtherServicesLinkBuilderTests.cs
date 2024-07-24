using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class OtherServicesLinkBuilderTests
{
    private readonly OtherServicesLinkBuilder _sut = new();
    private readonly DummyAcademyFactory _dummyAcademyFactory = new();

    [Fact]
    public void CompaniesHouseListingLink_should_return_url_containing_CompaniesHouseNumber()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234", companiesHouseNumber: "2345");
        var result = _sut.CompaniesHouseListingLink(dummyTrust);
        result.Should().Contain("/company/2345");
    }

    [Fact]
    public void CompaniesHouseListingLink_should_return_null_if_trust_has_no_CompaniesHouseNumber()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234", companiesHouseNumber: "");
        var result = _sut.CompaniesHouseListingLink(dummyTrust);
        result.Should().BeNull();
    }

    [Fact]
    public void GetInformationAboutSchoolsListingLink_should_return_url_containing_trust_uid_if_trust_is_open()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        var result = _sut.GetInformationAboutSchoolsListingLink(dummyTrust);
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
        var dummyTrust = DummyTrustFactory.GetDummyMultiAcademyTrust("1234", "2345");
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(dummyTrust);
        result.Should().Contain("/Trust?companyNo=2345");
    }

    [Fact]
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(1111);
        var dummyTrust = DummyTrustFactory.GetDummySingleAcademyTrust("1234", dummyAcademy);

        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(dummyTrust);
        result.Should().Contain("/school?urn=1111");
    }

    [Fact]
    public void SchoolFinancialBenchmarkingListingLink_should_be_null_if_single_academy_type_with_no_academies()
    {
        var dummyTrust = DummyTrustFactory.GetDummySingleAcademyTrust("1234");
        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(dummyTrust);
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("test", 0)]
    [InlineData("test", 1)]
    [InlineData("test", 2)]
    [InlineData("", 1)]
    public void
        SchoolFinancialBenchmarkingListingLink_should_be_null_if_type_is_neither_multi_or_single_academy_trust(
            string type, int numberOfAcademies)
    {
        var dummyAcademies = _dummyAcademyFactory.GetDummyAcademies(numberOfAcademies);
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234", type, "2345", dummyAcademies);

        var result = _sut.SchoolFinancialBenchmarkingServiceListingLink(dummyTrust);
        result.Should().BeNull();
    }

    [Fact]
    public void
        FindSchoolPerformanceDataListingLink_should_be_to_a_trust_page_if_multiacademy_trust()
    {
        var dummyTrust = DummyTrustFactory.GetDummyMultiAcademyTrust("1234");
        var result = _sut.FindSchoolPerformanceDataListingLink(dummyTrust);
        result.Should().Contain("/multi-academy-trust/1234");
    }

    [Fact]
    public void
        FindSchoolPerformanceDataListingLink_should_be_to_a_school_page_if_single_academy_trust_with_academies()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(1111);
        var dummyTrust = DummyTrustFactory.GetDummySingleAcademyTrust("1234", dummyAcademy);

        var result = _sut.FindSchoolPerformanceDataListingLink(dummyTrust);
        result.Should().Contain("/school/1111");
    }

    [Fact]
    public void FindSchoolPerformanceDataListingLink_should_be_null_if_trust_is_single_academy_type_with_no_trusts()
    {
        var dummyTrust = DummyTrustFactory.GetDummySingleAcademyTrust("1234");
        var result = _sut.FindSchoolPerformanceDataListingLink(dummyTrust);
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("test", 0)]
    [InlineData("test", 1)]
    [InlineData("test", 2)]
    [InlineData("", 1)]
    public void
        FindSchoolPerformanceDataListingLink_should_be_null_if_type_is_neither_multi_or_single_academy_trust(
            string type, int numberOfAcademies)
    {
        var dummyAcademies = _dummyAcademyFactory.GetDummyAcademies(numberOfAcademies);
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234", type, academies: dummyAcademies);

        var result = _sut.FindSchoolPerformanceDataListingLink(dummyTrust);
        result.Should().BeNull();
    }
}

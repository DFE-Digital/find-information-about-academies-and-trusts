using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class DetailsModelTests
{
    private readonly DetailsModel _sut;
    private readonly string _mockTrustUid = "1234";
    private readonly string _mockCompaniesHouseNo = "123456";
    private readonly int _mockAcademyUrn = 1111;
    private readonly string _mockFinancialBenchmarkingTrustLink = "FinancialBenchmarkingTrustLink";
    private readonly string _mockFinancialBenchmarkingSchoolLink = "FinancialBenchmarkingSchoolLink";
    private readonly string _mockFindSchoolPerformanceTrustLink = "FindSchoolPerformanceTrustLink";
    private readonly string _mockFindSchoolPerformanceSchoolLink = "FindSchoolPerformanceSchoolLink";


    public DetailsModelTests()
    {
        var mockLinksToOtherServices = new Mock<ILinksToOtherServices>();
        mockLinksToOtherServices.Setup(s => s.SchoolFinancialBenchmarkingServiceTrustLink(_mockCompaniesHouseNo))
            .Returns(_mockFinancialBenchmarkingTrustLink);
        mockLinksToOtherServices.Setup(s => s.SchoolFinancialBenchmarkingServiceSchoolLink(_mockAcademyUrn.ToString()))
            .Returns(_mockFinancialBenchmarkingSchoolLink);
        mockLinksToOtherServices.Setup(s => s.FindSchoolPerformanceDataTrustLink(_mockTrustUid))
            .Returns(_mockFindSchoolPerformanceTrustLink);
        mockLinksToOtherServices.Setup(s => s.FindSchoolPerformanceDataSchoolLink(_mockAcademyUrn.ToString()))
            .Returns(_mockFindSchoolPerformanceSchoolLink);
        _sut = new DetailsModel(new Mock<ITrustProvider>().Object, mockLinksToOtherServices.Object);
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        _sut.PageName.Should().Be("Details");
    }

    [Fact]
    public void SchoolsFinancialBenchmarkingLink_Should_return_trust_link_if_multi_academy_trust()
    {
        _sut.Trust = DummyTrustFactory.GetDummyMultiAcademyTrust(_mockTrustUid, _mockCompaniesHouseNo);
        var result = _sut.SchoolsFinancialBenchmarkingLink();

        result.Should()
            .Be(_mockFinancialBenchmarkingTrustLink);
    }

    [Fact]
    public void SchoolsFinancialBenchmarkingLink_Should_return_school_link_if_single_academy_trust()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(_mockAcademyUrn);

        _sut.Trust = DummyTrustFactory.GetDummySingleAcademyTrust(_mockTrustUid, dummyAcademy);
        var result = _sut.SchoolsFinancialBenchmarkingLink();

        result.Should()
            .Be(_mockFinancialBenchmarkingSchoolLink);
    }

    [Fact]
    public void FindSchoolPerformanceDataLink_should_return_trust_link_if_multi_academy_trust()
    {
        _sut.Trust = DummyTrustFactory.GetDummyMultiAcademyTrust(_mockTrustUid);
        var result = _sut.FindSchoolPerformanceDataLink();

        result.Should()
            .Be(_mockFindSchoolPerformanceTrustLink);
    }

    [Fact]
    public void FindSchoolPerformanceDataLink_Should_return_school_link_if_single_academy_trust()
    {
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(_mockAcademyUrn);

        _sut.Trust = DummyTrustFactory.GetDummySingleAcademyTrust(_mockTrustUid, dummyAcademy);
        var result = _sut.FindSchoolPerformanceDataLink();

        result.Should()
            .Be(_mockFindSchoolPerformanceSchoolLink);
    }
}

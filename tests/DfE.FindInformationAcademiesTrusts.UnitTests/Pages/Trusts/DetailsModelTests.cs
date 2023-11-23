using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class DetailsModelTests
{
    private readonly DetailsModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();

    public DetailsModelTests()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        var mockTrustProvider = new Mock<ITrustProvider>();
        mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrust);
        _sut = new DetailsModel(mockTrustProvider.Object, _mockLinksToOtherServices.Object);
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        _sut.PageName.Should().Be("Details");
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(_sut.Trust)).Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(_sut.Trust)).Returns("url");
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(_sut.Trust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().BeNull();
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(_sut.Trust)).Returns("url");
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(_sut.Trust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(_sut.Trust))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(_sut.Trust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(_sut.Trust))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().Be("url");
    }
}

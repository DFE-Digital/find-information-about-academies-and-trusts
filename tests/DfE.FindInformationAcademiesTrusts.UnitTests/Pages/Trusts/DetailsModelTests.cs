using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class DetailsModelTests
{
    private readonly DetailsModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly Trust _dummyTrust;
    private readonly MockDataSourceProvider _mockDataSourceProvider;

    public DetailsModelTests()
    {
        MockLogger<DetailsModel> logger = new();
        _dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockDataSourceProvider = new MockDataSourceProvider();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrust);
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync(_dummyTrust.Uid))
            .ReturnsAsync(new TrustSummaryDto(_dummyTrust.Uid, _dummyTrust.Name, _dummyTrust.Type,
                _dummyTrust.Academies.Length));

        _sut = new DetailsModel(_mockTrustProvider.Object, _mockDataSourceProvider.Object,
            _mockLinksToOtherServices.Object, logger.Object) { Uid = "1234" };
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        _sut.PageName.Should().Be("Details");
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(_dummyTrust)).Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(_dummyTrust)).Returns("url");
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(_dummyTrust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().BeNull();
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(_dummyTrust)).Returns("url");
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(_dummyTrust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(_dummyTrust))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(_dummyTrust))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(_dummyTrust))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().Be("url");
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryDto?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        var result = await _sut.OnGetAsync();
        _mockDataSourceProvider.Verify(e => e.GetGiasUpdated(), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Trust details", "Reference numbers" });
    }
}

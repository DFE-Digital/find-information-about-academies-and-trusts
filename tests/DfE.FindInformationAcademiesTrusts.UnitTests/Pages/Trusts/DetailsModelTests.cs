using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class DetailsModelTests
{
    private readonly DetailsModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();

    private static readonly TrustDetailsServiceModel DummyTrustDetailsServiceModel =
        new("1234", "", "", "", "", "", "", null, null);

    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();

    public DetailsModelTests()
    {
        _mockTrustRepository.Setup(t => t.GetTrustDetailsAsync(DummyTrustDetailsServiceModel.Uid))
            .ReturnsAsync(DummyTrustDetailsServiceModel);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(DummyTrustDetailsServiceModel.Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(DummyTrustDetailsServiceModel.Uid, "My trust", "", 0));

        _sut = new DetailsModel(_mockTrustProvider.Object, _mockDataSourceService.Object,
                _mockLinksToOtherServices.Object, new MockLogger<DetailsModel>().Object, _mockTrustRepository.Object)
            { Uid = DummyTrustDetailsServiceModel.Uid };
    }

    [Fact]
    public void PageName_should_be_Details()
    {
        _sut.PageName.Should().Be("Details");
    }

    [Fact]
    public async Task CompaniesHouseLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(DummyTrustDetailsServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().BeNull();
    }

    [Fact]
    public async Task CompaniesHouseLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.CompaniesHouseListingLink(DummyTrustDetailsServiceModel)).Returns("url");
        await _sut.OnGetAsync();
        _sut.CompaniesHouseLink.Should().Be("url");
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(DummyTrustDetailsServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().BeNull();
    }

    [Fact]
    public async Task GetInformationAboutSchoolsLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.GetInformationAboutSchoolsListingLink(DummyTrustDetailsServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.GetInformationAboutSchoolsLink.Should().Be("url");
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices
            .Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().BeNull();
    }

    [Fact]
    public async Task SchoolsFinancialBenchmarkingLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices
            .Setup(l => l.SchoolFinancialBenchmarkingServiceListingLink(DummyTrustDetailsServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.SchoolsFinancialBenchmarkingLink.Should().Be("url");
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_null_if_link_builder_returns_null()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(DummyTrustDetailsServiceModel))
            .Returns((string?)null);
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().BeNull();
    }

    [Fact]
    public async Task FindSchoolPerformanceLink_is_string_if_link_builder_returns_string()
    {
        _mockLinksToOtherServices.Setup(l => l.FindSchoolPerformanceDataListingLink(DummyTrustDetailsServiceModel))
            .Returns("url");
        await _sut.OnGetAsync();
        _sut.FindSchoolPerformanceLink.Should().Be("url");
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Trust details", "Reference numbers" });
    }
}

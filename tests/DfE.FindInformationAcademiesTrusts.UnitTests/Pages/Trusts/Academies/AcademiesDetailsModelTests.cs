using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class AcademiesDetailsModelTests
{
    private readonly AcademiesDetailsModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinkBuilder = new();
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();
    private readonly MockDataSourceProvider _mockDataSourceProvider = new();

    public AcademiesDetailsModelTests()
    {
        MockLogger<AcademiesDetailsModel> logger = new();
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");

        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrust);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(dummyTrust.Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(dummyTrust.Uid, dummyTrust.Name, dummyTrust.Type,
                dummyTrust.Academies.Length));

        _sut = new AcademiesDetailsModel(_mockTrustProvider.Object, _mockDataSourceProvider.Object,
            _mockLinkBuilder.Object, logger.Object, _mockTrustRepository.Object) { Uid = "1234" };
    }

    [Fact]
    public void PageTitle_should_be_AcademiesDetails()
    {
        _sut.PageTitle.Should().Be("Academies details");
    }

    [Fact]
    public void TabName_should_be_Details()
    {
        _sut.TabName.Should().Be("Details");
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }

    [Fact]
    public void OtherServicesLinkBuilder_should_be_injected()
    {
        _sut.LinkBuilder.Should().Be(_mockLinkBuilder.Object);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await _sut.OnGetAsync();
        _mockDataSourceProvider.Verify(e => e.GetGiasUpdated(), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Details" });
    }
}

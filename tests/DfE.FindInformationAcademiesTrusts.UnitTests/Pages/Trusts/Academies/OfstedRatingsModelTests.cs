using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingsModelTests
{
    private readonly OfstedRatingsModel _sut;
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly MockDataSourceProvider _mockDataSourceProvider;

    public OfstedRatingsModelTests()
    {
        MockLogger<OfstedRatingsModel> logger = new();
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockDataSourceProvider = new MockDataSourceProvider();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrust);
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync(dummyTrust.Uid))
            .ReturnsAsync(new TrustSummaryDto(dummyTrust.Uid, dummyTrust.Name, dummyTrust.Type,
                dummyTrust.Academies.Length));
        _sut = new OfstedRatingsModel(_mockTrustProvider.Object, _mockDataSourceProvider.Object, logger.Object)
            { Uid = "1234" };
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }

    [Fact]
    public void PageTitle_should_be_AcademiesOfstedRatingsPage()
    {
        _sut.PageTitle.Should().Be("Academies Ofsted ratings");
    }

    [Fact]
    public void TabName_should_be_OfstedRatings()
    {
        _sut.TabName.Should().Be("Ofsted ratings");
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryDto?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceProvider.Verify(e => e.GetGiasUpdated(), Times.Once);
        _mockDataSourceProvider.Verify(e => e.GetMisEstablishmentsUpdated(), Times.Once);
        _sut.DataSources.Count.Should().Be(2);
        _sut.DataSources[0].Fields.Should().Contain(new List<string>
            { "Date joined trust" });
        _sut.DataSources[1].Fields.Should().Contain(new List<string>
        {
            "Current Ofsted rating", "Date of last inspection", "Previous Ofsted rating", "Date of previous inspection"
        });
    }
}

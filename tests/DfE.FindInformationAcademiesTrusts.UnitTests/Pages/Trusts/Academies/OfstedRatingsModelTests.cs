using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingsModelTests
{
    private readonly OfstedRatingsModel _sut;
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly Mock<IDataSourceProvider> _mockDataSourceProvider;

    public OfstedRatingsModelTests()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockDataSourceProvider = new Mock<IDataSourceProvider>();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrust);
        _sut = new OfstedRatingsModel(_mockTrustProvider.Object, _mockDataSourceProvider.Object) { Uid = "1234" };
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
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync((Trust?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceProvider.Verify(e => e.GetGiasUpdated(), Times.Once);
        _mockDataSourceProvider.Verify(e => e.GetMisEstablishmentsUpdated(), Times.Once);
        _sut.DataSources.Count().Should().Be(2);
        _sut.DataSources.Should().Contain(i =>
            i.Fields == "Date joined trust, Current Ofsted rating, Date of last inspection");
        _sut.DataSources.Should().Contain(i => i.Fields == "Previous Ofsted rating, Date of previous inspection");
    }
}

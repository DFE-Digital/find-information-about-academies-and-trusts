using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustsAreaModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly Mock<IDataSourceProvider> _mockDataUpdatedProvider;
    private readonly TrustsAreaModel _sut;

    public TrustsAreaModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockDataUpdatedProvider = new Mock<IDataSourceProvider>();
        _sut = new TrustsAreaModel(_mockTrustProvider.Object, _mockDataUpdatedProvider.Object, "Details");
    }

    [Fact]
    public async void OnGetAsync_should_fetch_a_trust_by_uid()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync(dummyTrust.Uid))
            .ReturnsAsync(dummyTrust);
        _sut.Uid = dummyTrust.Uid;

        await _sut.OnGetAsync();
        _sut.Trust.Should().Be(dummyTrust);
    }

    [Fact]
    public async void GroupUid_should_be_empty_string_by_default()
    {
        await _sut.OnGetAsync();
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void PageName_should_be_set_at_initialisation()
    {
        var sut = new TrustsAreaModel(_mockTrustProvider.Object, _mockDataUpdatedProvider.Object, "Contacts");
        sut.PageName.Should().Be("Contacts");
    }

    [Fact]
    public void PageSection_should_be_AboutTheTrust()
    {
        _sut.Section.Should().Be("About the trust");
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync("1111"))
            .ReturnsAsync((Trust?)null);

        _sut.Uid = "1111";
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_Uid_is_not_provided()
    {
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }
}

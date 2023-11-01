using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class ContactsModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly ContactsModel _sut;

    public ContactsModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new ContactsModel(_mockTrustProvider.Object);
    }

    [Fact]
    public async void OnGetAsync_should_fetch_a_trust_by_Uid()
    {
        var dummyTrust = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync(dummyTrust.Uid))
            .ReturnsAsync(dummyTrust);
        _sut.Uid = dummyTrust.Uid;
        await _sut.OnGetAsync();
        _sut.Trust.Should().BeEquivalentTo(dummyTrust);
    }

    [Fact]
    public async void Uid_should_be_empty_string_by_default()
    {
        await _sut.OnGetAsync();
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void PageName_should_be_Contacts()
    {
        _sut.PageName.Should().Be("Contacts");
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

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class ContactsModelTests
{
    private readonly ContactsModel _sut;
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly Trust _dummyTrustWithGovernors;
    private readonly Trust _dummyTrustWithNoGovernors;

    public ContactsModelTests()
    {
        _dummyTrustWithGovernors = DummyTrustFactory.GetDummyTrustWithGovernors();
        _dummyTrustWithNoGovernors = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrustWithGovernors);
        _sut = new ContactsModel(_mockTrustProvider.Object) { Uid = "1234" };
    }

    [Fact]
    public void PageName_should_be_Contacts()
    {
        _sut.PageName.Should().Be("Contacts");
    }

    [Fact]
    public async Task OnGetAsync_Returns_Correct_Trust_Contacts()
    {
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.FullName.Should().Be("Present Chair");
        _sut.AccountingOfficer?.FullName.Should<string>().Be("Present Accountingofficer");
        _sut.ChiefFinancialOfficer?.FullName.Should<string>().Be("Present Chieffinancialofficer");
    }

    [Fact]
    public async Task OnGetAsync_Returns_Correct_Dfe_Contacts()
    {
        await _sut.OnGetAsync();
        _sut.Trust.TrustRelationshipManager?.FullName.Should().Be("Present Trm");
        _sut.Trust.SfsoLead?.FullName.Should().Be("Present Sfsolead");
    }

    [Fact]
    public async Task OnGetAsync_Returns_Null_Dfe_Contacts()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrustWithNoGovernors);
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.Should().Be(null);
        _sut.AccountingOfficer?.FullName.Should().Be(null);
        _sut.ChiefFinancialOfficer?.FullName.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync((Trust?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }
}

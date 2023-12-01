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
        _dummyTrustWithGovernors = DummyTrustFactory.GetDummyTrust("1234", governors: ListOfGovernors());
        _dummyTrustWithNoGovernors = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrustWithGovernors);
        _sut = new ContactsModel(_mockTrustProvider.Object) { Uid = "1234" };
    }

    private void SetupTrustWithNoGoverners()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrustWithNoGovernors);
    }

    [Fact]
    public void PageName_should_be_Contacts()
    {
        _sut.PageName.Should().Be("Contacts");
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_be_current_chair()
    {
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.FullName.Should().Be("Present Chair");
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.FullName.Should<string>().Be("Present Accountingofficer");
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.FullName.Should<string>().Be("Present Chieffinancialofficer");
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_relationship_manager()
    {
        await _sut.OnGetAsync();
        _sut.Trust.TrustRelationshipManager?.FullName.Should().Be("Present Trm");
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_sfsolead()
    {
        await _sut.OnGetAsync();
        _sut.Trust.SfsoLead?.FullName.Should().Be("Present Sfsolead");
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_null_when_trust_has_no_chair()
    {
        SetupTrustWithNoGoverners();
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_null_be_when_trust_has_no_officer()
    {
        SetupTrustWithNoGoverners();
        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.FullName.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_null_be_when_trust_has_no_officer()
    {
        SetupTrustWithNoGoverners();
        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.FullName.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync((Trust?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    private static Governor[] ListOfGovernors()
    {
        Governor[] listOfGovernors =
        {
            new("1", "1", "Past Chair", Email: "pastchair@test.com", Role: "Chair Of Trustees",
                AppointingBody: "testBody", DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("2", "2", "Present Chair", Email: "presentchair@test.com", Role: "Chair Of Trustees",
                AppointingBody: "testBody", DateOfAppointment: null, DateOfTermEnd: DateTime.Today),
            new("3", "3", "Past Accountingofficer", Email: "pastao@test.com", Role: "Accounting Officer",
                AppointingBody: "testBody", DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("4", "4", "Present Accountingofficer", Email: "presentao@test.com", Role: "Accounting Officer",
                AppointingBody: "testBody", DateOfAppointment: null, DateOfTermEnd: DateTime.Today),
            new("5", "5", "Past Chieffinancialofficer", Email: "pastcfo@test.com", Role: "Chief Financial Officer",
                AppointingBody: "testBody", DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("6", "6", "Present Chieffinancialofficer", Email: "presentcfo@test.com",
                Role: "Chief Financial Officer", AppointingBody: "testBody", DateOfAppointment: null,
                DateOfTermEnd: null)
        };

        return listOfGovernors;
    }
}

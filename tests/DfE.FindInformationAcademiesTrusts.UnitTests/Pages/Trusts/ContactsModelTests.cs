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
    private const string PresentChairOfTrustees = "Present ChairOfTrustees";
    private const string PresentAccountingOfficer = "Present AccountingOfficer";
    private const string PresentChiefFinancialOfficer = "Present ChiefFinancialOfficer";
    private const string ChairOfTrustees = "Chair Of Trustees";
    private const string AccountingOfficer = "Accounting Officer";
    private const string ChiefFinancialOfficer = "Chief Financial Officer";
    private static DateTime _currentGovernorDate;
    private static DateTime _pastGovernorDate;
    

    public ContactsModelTests()
    {
        _pastGovernorDate = DateTime.Today.AddDays(-1);
        _currentGovernorDate = DateTime.Today;
        _dummyTrustWithGovernors = DummyTrustFactory.GetDummyTrust("1234", governors: ListOfGovernors());
        _dummyTrustWithNoGovernors = DummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider = new Mock<ITrustProvider>();
        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(_dummyTrustWithGovernors);
        Mock<IDataSourceProvider> mockDataUpdatedProvider = new();
        _sut = new ContactsModel(_mockTrustProvider.Object, mockDataUpdatedProvider.Object) { Uid = "1234" };
    }

    private void SetupTrustWithNoGovernors()
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
        _sut.ChairOfTrustees?.FullName.Should().Be(PresentChairOfTrustees);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.FullName.Should<string>().Be(PresentAccountingOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.FullName.Should<string>().Be(PresentChiefFinancialOfficer);
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
        SetupTrustWithNoGovernors();
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_null_be_when_trust_has_no_officer()
    {
        SetupTrustWithNoGovernors();
        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.FullName.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_null_be_when_trust_has_no_officer()
    {
        SetupTrustWithNoGovernors();
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
            DummyGovernorFactory.GetDummyGovernor("Past Chair", ChairOfTrustees,_pastGovernorDate ),
            DummyGovernorFactory.GetDummyGovernor(PresentChairOfTrustees, ChairOfTrustees, _currentGovernorDate),
            DummyGovernorFactory.GetDummyGovernor("Past AccountingOfficer", AccountingOfficer, _pastGovernorDate),
            DummyGovernorFactory.GetDummyGovernor(PresentAccountingOfficer, AccountingOfficer, _currentGovernorDate),
            DummyGovernorFactory.GetDummyGovernor("Past ChiefFinancialOfficer", ChiefFinancialOfficer, _pastGovernorDate),
            DummyGovernorFactory.GetDummyGovernor(PresentChiefFinancialOfficer, ChiefFinancialOfficer, null)
        };

        return listOfGovernors;
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;
using DfE.FindInformationAcademiesTrusts.Data.Dto;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class ContactsModelTests
{
    private readonly ContactsModel _sut;
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();
    private const string PresentChairOfTrustees = "Present ChairOfTrustees";
    private const string PresentAccountingOfficer = "Present AccountingOfficer";
    private const string PresentChiefFinancialOfficer = "Present ChiefFinancialOfficer";
    private const string ChairOfTrustees = "Chair Of Trustees";
    private const string AccountingOfficer = "Accounting Officer";
    private const string ChiefFinancialOfficer = "Chief Financial Officer";
    private static readonly DateTime CurrentGovernorDate = DateTime.Today;
    private static readonly DateTime PastGovernorDate = DateTime.Today.AddDays(-1);

    private readonly MockDataSourceProvider _mockDataSourceProvider = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();

    private static Governor[] ListOfGovernors()
    {
        Governor[] listOfGovernors =
        {
            DummyGovernorFactory.GetDummyGovernor("Past Chair", ChairOfTrustees, PastGovernorDate),
            DummyGovernorFactory.GetDummyGovernor(PresentChairOfTrustees, ChairOfTrustees, CurrentGovernorDate),
            DummyGovernorFactory.GetDummyGovernor("Past AccountingOfficer", AccountingOfficer, PastGovernorDate),
            DummyGovernorFactory.GetDummyGovernor(PresentAccountingOfficer, AccountingOfficer, CurrentGovernorDate),
            DummyGovernorFactory.GetDummyGovernor("Past ChiefFinancialOfficer", ChiefFinancialOfficer,
                PastGovernorDate),
            DummyGovernorFactory.GetDummyGovernor(PresentChiefFinancialOfficer, ChiefFinancialOfficer, null)
        };

        return listOfGovernors;
    }

    public ContactsModelTests()
    {
        var dummyTrustWithGovernors = DummyTrustFactory.GetDummyTrust("1234", governors: ListOfGovernors());

        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrustWithGovernors);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(dummyTrustWithGovernors.Uid))
            .ReturnsAsync(new TrustSummaryDto(dummyTrustWithGovernors.Uid, dummyTrustWithGovernors.Name,
                dummyTrustWithGovernors.Type, dummyTrustWithGovernors.Academies.Length));

        _sut = new ContactsModel(_mockTrustProvider.Object, _mockDataSourceProvider.Object,
                new MockLogger<ContactsModel>().Object, _mockTrustRepository.Object)
            { Uid = "1234" };
    }

    private void SetupTrustWithNoGovernors()
    {
        var dummyTrustWithNoGovernors = DummyTrustFactory.GetDummyTrust("1234");

        _mockTrustProvider.Setup(tp => tp.GetTrustByUidAsync("1234")).ReturnsAsync(dummyTrustWithNoGovernors);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(dummyTrustWithNoGovernors.Uid))
            .ReturnsAsync(new TrustSummaryDto(dummyTrustWithNoGovernors.Uid, dummyTrustWithNoGovernors.Name,
                dummyTrustWithNoGovernors.Type, dummyTrustWithNoGovernors.Academies.Length));
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
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustRepository.Setup(r => r.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryDto?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceProvider.Verify(e => e.GetCdmUpdated(), Times.Once);
        _mockDataSourceProvider.Verify(e => e.GetGiasUpdated(), Times.Once);
        _mockDataSourceProvider.Verify(e => e.GetMstrUpdated(), Times.Once);
        _sut.DataSources.Count.Should().Be(3);
        _sut.DataSources[0].Fields.Should().Contain(new List<string>
            { "DfE contacts" });
        _sut.DataSources[1].Fields.Should().Contain(new List<string>
            { "Accounting officer name", "Chief financial officer name", "Chair of trustees name" });
        _sut.DataSources[2].Fields.Should().Contain(new List<string>
            { "Accounting officer email", "Chief financial officer email", "Chair of trustees email" });
    }
}

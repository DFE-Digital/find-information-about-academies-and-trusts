using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public class ContactsAreaModelTests
{
    private readonly ContactsAreaModel _sut;

    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private static readonly string TestUid = "1234";

    private readonly TrustSummaryServiceModel _fakeTrust = new(TestUid, "My Trust", "Multi-academy trust", 3);

    private readonly Person _chairOfTrustees = new("Chair Of Trustees", "cot@test.com");
    private readonly Person _chiefFinancialOfficer = new("Chief Financial Officer", "cfo@test.com");
    private readonly Person _accountingOfficer = new("Accounting Officer", "ao@test.com");
    private readonly InternalContact _sfsoLead = new("SFSO Lead", "sfsol@test.com", DateTime.Today, "test@email.com");

    private readonly InternalContact _trustRelationshipManager =
        new("Trust Relationship Manager", "trm@test.com", DateTime.Today, "test@email.com");

    public ContactsAreaModelTests()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TestUid)).ReturnsAsync(
            new TrustContactsServiceModel(_trustRelationshipManager, _sfsoLead, _accountingOfficer, _chairOfTrustees,
                _chiefFinancialOfficer));
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_fakeTrust.Uid))
            .ReturnsAsync(_fakeTrust);

        _sut = new ContactsAreaModel(_mockDataSourceService.Object, _mockTrustService.Object,
                new MockLogger<ContactsAreaModel>().Object)
            { Uid = TestUid };
    }

    private void SetupTrustWithNoGovernors()
    {
        var testTrustUid = TestUid;
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TestUid)).ReturnsAsync(
            new TrustContactsServiceModel(null, null, null, null, null));
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(testTrustUid))
            .ReturnsAsync(new TrustSummaryServiceModel(testTrustUid, testTrustName,
                testTrustType, 0));
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_be_current_chair()
    {
        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.Should().Be(_chairOfTrustees);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.Should().Be(_accountingOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_be_current_officer()
    {
        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.Should().Be(_chiefFinancialOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_relationship_manager()
    {
        await _sut.OnGetAsync();
        _sut.TrustRelationshipManager?.Should().Be(_trustRelationshipManager);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_sfsolead()
    {
        await _sut.OnGetAsync();
        _sut.SfsoLead?.Should().Be(_sfsoLead);
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
        _sut.AccountingOfficer?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_null_be_when_trust_has_no_officer()
    {
        SetupTrustWithNoGovernors();
        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(r => r.GetTrustSummaryAsync(TestUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Mstr), Times.Once);
        _sut.DataSources.Count.Should().Be(4);
        _sut.DataSources[0].Fields.Should().Contain(new List<string>
            { "Trust relationship manager" });
        _sut.DataSources[1].Fields.Should().Contain(new List<string>
            { "SFSO (Schools financial support and oversight) lead" });
        _sut.DataSources[2].Fields.Should().Contain(new List<string>
            { "Accounting officer name", "Chief financial officer name", "Chair of trustees name" });
        _sut.DataSources[3].Fields.Should().Contain(new List<string>
            { "Accounting officer email", "Chief financial officer email", "Chair of trustees email" });
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview/TrustDetails", TestUid, false, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts/InDfe", TestUid, true, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
                TestUid, false, "academies-nav"),
            new TrustNavigationLinkModel("Ofsted", "/Trusts/Ofsted/CurrentRatings", "1234", false, "ofsted-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance/TrustLeadership", TestUid, false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_SubNavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().BeEquivalentTo([
            new TrustSubNavigationLinkModel("In DfE", "./InDfE", TestUid, "Contacts", false),
            new TrustSubNavigationLinkModel("In the trust", "./InTrust", TestUid, "Contacts", false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be("Contacts");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public abstract class BaseContactsAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages where T : ContactsAreaModel
{
    private readonly TrustContactsServiceModel _baseTrustContactsServiceModel = new(null, null, null, null, null);

    private readonly InternalContact _trustRelationshipManager = new(
        "Trust Relationship Manager",
        "trm@test.com",
        new DateTime(2025, 01, 20),
        "test@email.com");

    private readonly InternalContact _sfsoLead = new(
        "SFSO Lead",
        "sfsol@test.com",
        new DateTime(2024, 02, 21),
        "test2@email.com");

    protected BaseContactsAreaModelTests()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(It.IsAny<string>()))
            .ReturnsAsync(_baseTrustContactsServiceModel);
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_be_current_chair()
    {
        var chairOfTrustees = new Person("Chair Of Trustees", "cot@test.com");
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChairOfTrustees = chairOfTrustees });

        await _sut.OnGetAsync();
        _sut.ChairOfTrustees.Should().Be(chairOfTrustees);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_be_current_officer()
    {
        var accountingOfficer = new Person("Accounting Officer", "ao@test.com");
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { AccountingOfficer = accountingOfficer });

        await _sut.OnGetAsync();

        _sut.AccountingOfficer.Should().Be(accountingOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_be_current_officer()
    {
        var chiefFinancialOfficer = new Person("Chief Financial Officer", "cfo@test.com");

        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChiefFinancialOfficer = chiefFinancialOfficer });

        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer.Should().Be(chiefFinancialOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_relationship_manager()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { TrustRelationshipManager = _trustRelationshipManager });

        await _sut.OnGetAsync();
        _sut.TrustRelationshipManager?.Should().Be(_trustRelationshipManager);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_sfsolead()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { SfsoLead = _sfsoLead });

        await _sut.OnGetAsync();
        _sut.SfsoLead?.Should().Be(_sfsoLead);
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_null_when_trust_has_no_chair()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChairOfTrustees = null });

        await _sut.OnGetAsync();
        _sut.ChairOfTrustees?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_null_be_when_trust_has_no_officer()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { AccountingOfficer = null });

        await _sut.OnGetAsync();
        _sut.AccountingOfficer?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_null_be_when_trust_has_no_officer()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChiefFinancialOfficer = null });

        await _sut.OnGetAsync();
        _sut.ChiefFinancialOfficer?.Should().Be(null);
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();

        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Mstr), Times.Once);

        _sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.ContactsInDfePageName, [
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb, null, null),
                        "Trust relationship manager"),
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb, null, null),
                        "SFSO (Schools financial support and oversight) lead")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.ContactsInTrustPageName, [
                    new DataSourceListEntry(_giasDataSource, "Accounting officer name"),
                    new DataSourceListEntry(_giasDataSource, "Chief financial officer name"),
                    new DataSourceListEntry(_giasDataSource, "Chair of trustees name"),
                    new DataSourceListEntry(_mstrDataSource, "Accounting officer email"),
                    new DataSourceListEntry(_mstrDataSource, "Chief financial officer email"),
                    new DataSourceListEntry(_mstrDataSource, "Chair of trustees email")
                ]
            )
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_trustRelationshipManager_last_modified_details_in_data_source_list()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { TrustRelationshipManager = _trustRelationshipManager });

        await _sut.OnGetAsync();

        _sut.DataSourcesPerPage[0].DataSources[0].DataSource.Should().Be(
            new DataSourceServiceModel(Source.FiatDb,
                _trustRelationshipManager.LastModifiedAtTime,
                null,
                _trustRelationshipManager.LastModifiedByEmail)
        );
    }

    [Fact]
    public async Task OnGetAsync_sets__sfsoLead_last_modified_details_in_data_source_list()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { SfsoLead = _sfsoLead });

        await _sut.OnGetAsync();

        _sut.DataSourcesPerPage[0].DataSources[1].DataSource.Should().Be(
            new DataSourceServiceModel(Source.FiatDb,
                _sfsoLead.LastModifiedAtTime,
                null,
                _sfsoLead.LastModifiedByEmail)
        );
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.PageName.Should().Be("Contacts");
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await _sut.OnGetAsync();

        _sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Contacts");
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await _sut.OnGetAsync();

        _sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("In DfE");
                    l.SubPageLink.Should().Be("./InDfE");
                    l.ServiceName.Should().Be("Contacts");
                },
                l =>
                {
                    l.LinkText.Should().Be("In the trust");
                    l.SubPageLink.Should().Be("./InTrust");
                    l.ServiceName.Should().Be("Contacts");
                });
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}

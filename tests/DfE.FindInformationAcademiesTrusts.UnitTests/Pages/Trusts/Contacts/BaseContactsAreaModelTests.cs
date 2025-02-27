using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

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
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(It.IsAny<string>()))
            .ReturnsAsync(_baseTrustContactsServiceModel);
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_be_current_chair()
    {
        var chairOfTrustees = new Person("Chair Of Trustees", "cot@test.com");
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChairOfTrustees = chairOfTrustees });

        await Sut.OnGetAsync();
        Sut.ChairOfTrustees.Should().Be(chairOfTrustees);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_be_current_officer()
    {
        var accountingOfficer = new Person("Accounting Officer", "ao@test.com");
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { AccountingOfficer = accountingOfficer });

        await Sut.OnGetAsync();

        Sut.AccountingOfficer.Should().Be(accountingOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_be_current_officer()
    {
        var chiefFinancialOfficer = new Person("Chief Financial Officer", "cfo@test.com");

        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChiefFinancialOfficer = chiefFinancialOfficer });

        await Sut.OnGetAsync();
        Sut.ChiefFinancialOfficer.Should().Be(chiefFinancialOfficer);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_relationship_manager()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { TrustRelationshipManager = _trustRelationshipManager });

        await Sut.OnGetAsync();
        Sut.TrustRelationshipManager?.Should().Be(_trustRelationshipManager);
    }

    [Fact]
    public async Task OnGetAsync_sets_trust_sfsolead()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { SfsoLead = _sfsoLead });

        await Sut.OnGetAsync();
        Sut.SfsoLead?.Should().Be(_sfsoLead);
    }

    [Fact]
    public async Task OnGetAsync_sets_chair_of_trustees_to_null_when_trust_has_no_chair()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChairOfTrustees = null });

        await Sut.OnGetAsync();
        Sut.ChairOfTrustees?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_accounting_officer_to_null_be_when_trust_has_no_officer()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { AccountingOfficer = null });

        await Sut.OnGetAsync();
        Sut.AccountingOfficer?.Should().Be(null);
    }

    [Fact]
    public async Task OnGetAsync_sets_chief_financial_officer_to_null_be_when_trust_has_no_officer()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { ChiefFinancialOfficer = null });

        await Sut.OnGetAsync();
        Sut.ChiefFinancialOfficer?.Should().Be(null);
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        MockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        MockDataSourceService.Verify(e => e.GetAsync(Source.Mstr), Times.Once);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry(ViewConstants.ContactsInDfePageName, [
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb, null, null),
                        "Trust relationship manager"),
                    new DataSourceListEntry(new DataSourceServiceModel(Source.FiatDb, null, null),
                        "SFSO (Schools financial support and oversight) lead")
                ]
            ),
            new DataSourcePageListEntry(ViewConstants.ContactsInTrustPageName, [
                    new DataSourceListEntry(GiasDataSource, "Accounting officer name"),
                    new DataSourceListEntry(GiasDataSource, "Chief financial officer name"),
                    new DataSourceListEntry(GiasDataSource, "Chair of trustees name"),
                    new DataSourceListEntry(MstrDataSource, "Accounting officer email"),
                    new DataSourceListEntry(MstrDataSource, "Chief financial officer email"),
                    new DataSourceListEntry(MstrDataSource, "Chair of trustees email")
                ]
            )
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_trustRelationshipManager_last_modified_details_in_data_source_list()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { TrustRelationshipManager = _trustRelationshipManager });

        await Sut.OnGetAsync();

        Sut.DataSourcesPerPage[0].DataSources[0].DataSource.Should().Be(
            new DataSourceServiceModel(Source.FiatDb,
                _trustRelationshipManager.LastModifiedAtTime,
                null,
                _trustRelationshipManager.LastModifiedByEmail)
        );
    }

    [Fact]
    public async Task OnGetAsync_sets__sfsoLead_last_modified_details_in_data_source_list()
    {
        MockTrustService.Setup(tp => tp.GetTrustContactsAsync(TrustUid))
            .ReturnsAsync(_baseTrustContactsServiceModel with { SfsoLead = _sfsoLead });

        await Sut.OnGetAsync();

        Sut.DataSourcesPerPage[0].DataSources[1].DataSource.Should().Be(
            new DataSourceServiceModel(Source.FiatDb,
                _sfsoLead.LastModifiedAtTime,
                null,
                _sfsoLead.LastModifiedByEmail)
        );
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Contacts");
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Contacts");
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("In DfE");
                    l.SubPageLink.Should().Be("./InDfE");
                    l.ServiceName.Should().Be("Contacts");
                },
                l =>
                {
                    l.LinkText.Should().Be("In this trust");
                    l.SubPageLink.Should().Be("./InTrust");
                    l.ServiceName.Should().Be("Contacts");
                });
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}

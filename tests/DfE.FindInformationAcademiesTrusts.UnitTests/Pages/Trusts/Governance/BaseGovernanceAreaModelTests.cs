using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public abstract class BaseGovernanceAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages
    where T : GovernanceAreaModel
{
    protected BaseGovernanceAreaModelTests()
    {
        MockTrustService.GetTrustGovernanceAsync(Arg.Any<string>())
            .Returns(Task.FromResult(new TrustGovernanceServiceModel([], [], [], [], 0)));
    }

    private static Governor[] GenerateGovernors(bool isCurrent, string role, int numberToGenerate)
    {
        return Enumerable.Repeat(new Governor(
            "9999",
            TrustUid,
            Role: role,
            FullName: "First Second Last",
            DateOfAppointment: DateTime.Today.AddYears(-3),
            DateOfTermEnd: isCurrent ? DateTime.Today.AddYears(1) : DateTime.Today.AddYears(-1),
            AppointingBody: "Nick Warms",
            Email: null
        ), numberToGenerate).ToArray();
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        await Sut.OnGetAsync();

        await MockDataSourceService.Received(1).GetAsync(Source.Gias);

        Sut.DataSourcesPerPage.Should().BeEquivalentTo([
            new DataSourcePageListEntry("Trust leadership", [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry("Trustees", [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry("Members", [new DataSourceListEntry(GiasDataSource)]),
            new DataSourcePageListEntry("Historic members", [new DataSourceListEntry(GiasDataSource)])
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_TrustGovernance()
    {
        var trustGovernanceServiceModelWithData = new TrustGovernanceServiceModel(
            GenerateGovernors(true, "Chair of Trustees", 4),
            GenerateGovernors(true, "Member", 3),
            GenerateGovernors(true, "Trustee", 2),
            GenerateGovernors(false, "Trustee", 1),
            10);

        MockTrustService.GetTrustGovernanceAsync(TrustUid).Returns(Task.FromResult(trustGovernanceServiceModelWithData));

        await Sut.OnGetAsync();

        await MockTrustService.Received(1).GetTrustGovernanceAsync(TrustUid);
        Sut.TrustGovernance.Should().BeEquivalentTo(trustGovernanceServiceModelWithData);
    }

    [Theory]
    [InlineData(0, 0, 0, 0)]
    [InlineData(0, 1, 2, 3)]
    [InlineData(3, 0, 1, 2)]
    [InlineData(2, 3, 0, 1)]
    [InlineData(1, 2, 3, 0)]
    public async Task OnGetAsync_should_include_numbers_of_governors_in_subpage_link_text(int numTrustLeaders,
        int numMembers, int numTrustees, int numHistoricMembers)
    {
        MockTrustService.GetTrustGovernanceAsync(TrustUid)
            .Returns(Task.FromResult(new TrustGovernanceServiceModel(
                GenerateGovernors(true, "Chair of Trustees", numTrustLeaders),
                GenerateGovernors(true, "Member", numMembers),
                GenerateGovernors(true, "Trustee", numTrustees),
                GenerateGovernors(false, "Trustee", numHistoricMembers),
                0)));

        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l => { l.LinkText.Should().Be($"Trust leadership ({numTrustLeaders})"); },
                l => { l.LinkText.Should().Be($"Trustees ({numTrustees})"); },
                l => { l.LinkText.Should().Be($"Members ({numMembers})"); },
                l => { l.LinkText.Should().Be($"Historic members ({numHistoricMembers})"); });
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();
        
        Sut.TrustPageMetadata.PageName.Should().Be("Governance");
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be(ViewConstants.GovernancePageName);
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
                    l.LinkText.Should().Be("Trust leadership (0)");
                    l.SubPageLink.Should().Be("./TrustLeadership");
                    l.ServiceName.Should().Be(ViewConstants.GovernancePageName);
                },
                l =>
                {
                    l.LinkText.Should().Be("Trustees (0)");
                    l.SubPageLink.Should().Be("./Trustees");
                    l.ServiceName.Should().Be(ViewConstants.GovernancePageName);
                },
                l =>
                {
                    l.LinkText.Should().Be("Members (0)");
                    l.SubPageLink.Should().Be("./Members");
                    l.ServiceName.Should().Be(ViewConstants.GovernancePageName);
                },
                l =>
                {
                    l.LinkText.Should().Be("Historic members (0)");
                    l.SubPageLink.Should().Be("./HistoricMembers");
                    l.ServiceName.Should().Be(ViewConstants.GovernancePageName);
                });
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}

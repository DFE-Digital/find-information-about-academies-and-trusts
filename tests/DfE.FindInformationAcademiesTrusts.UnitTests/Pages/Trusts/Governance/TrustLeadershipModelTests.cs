using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Governance;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Governance;

public class TrustLeadershipModelTests
{
    private readonly TrustLeadershipModel _sut;
    private static readonly DateTime StartDate = DateTime.Today.AddYears(-3);
    private static readonly DateTime FutureEndDate = DateTime.Today.AddYears(1);
    private static readonly DateTime HistoricEndDate = DateTime.Today.AddYears(-1);

    private static readonly Governor Member = new(
        "9999",
        "1234",
        Role: "Member",
        FullName: "First Second Last",
        DateOfAppointment: StartDate,
        DateOfTermEnd: FutureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor Trustee = new(
        "9998",
        "1234",
        Role: "Trustee",
        FullName: "First Second Last",
        DateOfAppointment: StartDate,
        DateOfTermEnd: FutureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor Leader = new(
        "9999",
        "1234",
        Role: "Chair of Trustees",
        FullName: "First Second Last",
        DateOfAppointment: StartDate,
        DateOfTermEnd: FutureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor Historic = new(
        "9999",
        "1234",
        Role: "Trustee",
        FullName: "First Second Last",
        DateOfAppointment: StartDate,
        DateOfTermEnd: HistoricEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly TrustGovernanceServiceModel DummyTrustGovernanceServiceModel =
        new([Leader], [Member], [Trustee], [Historic], 0);

    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();

    private static readonly string TestUid = "1234";

    public TrustLeadershipModelTests()
    {
        _mockTrustRepository.Setup(t => t.GetTrustGovernanceAsync(TestUid))
            .ReturnsAsync(DummyTrustGovernanceServiceModel);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(TestUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TestUid, "My trust", "", 0));

        _sut = new TrustLeadershipModel(_mockDataSourceService.Object,
                new MockLogger<TrustLeadershipModel>().Object, _mockTrustRepository.Object)
            { Uid = TestUid };
    }

    [Fact]
    public void ShowHeaderSearch_should_be_true()
    {
        _sut.ShowHeaderSearch.Should().Be(true);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_null()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(TestUid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_data_source_list()
    {
        await _sut.OnGetAsync();
        _mockDataSourceService.Verify(e => e.GetAsync(Source.Gias), Times.Once);
        _sut.DataSources.Should().ContainSingle();
        _sut.DataSources[0].Fields.Should().Contain(new List<string> { "Governance" });
    }

    [Fact]
    public async Task OnGetAsync_sets_Governance_Service()
    {
        await _sut.OnGetAsync();
        _mockTrustRepository.Verify(e => e.GetTrustGovernanceAsync(TestUid), Times.Once);
        _sut.TrustGovernance.Should().BeEquivalentTo(DummyTrustGovernanceServiceModel);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel("Overview", "/Trusts/Overview/TrustDetails", "1234", false, "overview-nav"),
            new TrustNavigationLinkModel("Contacts", "/Trusts/Contacts/InDfe", "1234", false, "contacts-nav"),
            new TrustNavigationLinkModel("Academies (0)", "/Trusts/Academies/Details",
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel("Ofsted", "/Trusts/Ofsted/CurrentRatings", "1234", false, "ofsted-nav"),
            new TrustNavigationLinkModel("Governance", "/Trusts/Governance/TrustLeadership", "1234", true,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_SubNavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().BeEquivalentTo([
            new TrustSubNavigationLinkModel("Trust leadership (1)", "./TrustLeadership", "1234", "Governance", true),
            new TrustSubNavigationLinkModel("Trustees (1)", "./Trustees", "1234", "Governance", false),
            new TrustSubNavigationLinkModel("Members (1)", "./Members", "1234", "Governance", false),
            new TrustSubNavigationLinkModel("Historic members (1)", "./HistoricMembers", "1234", "Governance", false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Trust leadership");
        _sut.TrustPageMetadata.PageName.Should().Be("Governance");
        _sut.TrustPageMetadata.TrustName.Should().Be("My trust");
    }
}

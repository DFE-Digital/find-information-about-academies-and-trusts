using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class GovernanceModelTests
{
    private readonly GovernanceModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinksToOtherServices = new();
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();
    private static readonly DateTime startDate = DateTime.Today.AddYears(-3);
    private static readonly DateTime futureEndDate = DateTime.Today.AddYears(1);
    private static readonly DateTime historicEndDate = DateTime.Today.AddYears(-1);

    private static readonly Governor member = new(
        "9999",
        "1234",
        Role: "Member",
        FullName: "First Second Last",
        DateOfAppointment: startDate,
        DateOfTermEnd: futureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor trustee = new(
        "9998",
        "1234",
        Role: "Trustee",
        FullName: "First Second Last",
        DateOfAppointment: startDate,
        DateOfTermEnd: futureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor leader = new(
        "9999",
        "1234",
        Role: "Chair of Trustees",
        FullName: "First Second Last",
        DateOfAppointment: startDate,
        DateOfTermEnd: futureEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly Governor historic = new(
        "9999",
        "1234",
        Role: "Trustee",
        FullName: "First Second Last",
        DateOfAppointment: startDate,
        DateOfTermEnd: historicEndDate,
        AppointingBody: "Nick Warms",
        Email: null
    );

    private static readonly TrustGovernanceServiceModel DummyTrustGovernanceServiceModel =
        new([leader], [member], [trustee], [historic]);

    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();

    private static readonly string TestUid = "1234";

    public GovernanceModelTests()
    {
        _mockTrustRepository.Setup(t => t.GetTrustGoverenaceAsync(TestUid))
            .ReturnsAsync(DummyTrustGovernanceServiceModel);
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(TestUid))
            .ReturnsAsync(new TrustSummaryServiceModel(TestUid, "My trust", "", 0));

        _sut = new GovernanceModel(_mockTrustProvider.Object, _mockDataSourceService.Object,
                new MockLogger<GovernanceModel>().Object, _mockTrustRepository.Object)
            { Uid = TestUid };
    }

    [Fact]
    public void PageName_should_be_Governance()
    {
        _sut.PageName.Should().Be("Governance");
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
        _mockTrustRepository.Verify(e => e.GetTrustGoverenaceAsync(TestUid), Times.Once);
        _sut.TrustGovernance.Should().BeEquivalentTo(DummyTrustGovernanceServiceModel);
    }
}

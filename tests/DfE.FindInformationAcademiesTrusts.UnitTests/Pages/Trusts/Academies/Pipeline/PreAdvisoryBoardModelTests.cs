using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.Pipeline;

public class PreAdvisoryBoardModelTests
{
    private readonly PreAdvisoryBoardModel _sut;
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockDataSourceService _mockDataSourceService = new();
    private const string Uid = "1234";

    public PreAdvisoryBoardModelTests()
    {
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(Uid, testTrustName, testTrustType,
                1));
        _mockAcademyService.Setup(t => t.GetAcademiesPipelineSummary())
            .Returns(new AcademyPipelineSummaryServiceModel(1, 2, 3));
        _sut = new PreAdvisoryBoardModel(
                _mockDataSourceService.Object, new MockLogger<PreAdvisoryBoardModel>().Object,
                _mockTrustService.Object, _mockAcademyService.Object, _mockExportService.Object,
                _mockDateTimeProvider.Object)
            { Uid = Uid };
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(Uid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        AcademyPipelineServiceModel[] academies =
        [
            new AcademyPipelineServiceModel(Uid, "Baking academy", new AgeRange(4, 16), "Bristol", "Conversion",
                new DateTime(2025, 3, 3)),
            new AcademyPipelineServiceModel(Uid, "Chocolate academy", new AgeRange(11, 18), "Birmingham",
                "Conversion",
                new DateTime(2025, 5, 3)),
            new AcademyPipelineServiceModel(Uid, "Fruity academy", new AgeRange(9, 16), "Sheffield", "Transfer",
                new DateTime(2025, 9, 3)),
            new AcademyPipelineServiceModel(null, null, null, null, null, null)
        ];
        _mockAcademyService.Setup(a => a.GetAcademiesPipelinePreAdvisory())
            .Returns(academies);

        _ = await _sut.OnGetAsync();

        _sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel(ViewConstants.OverviewPageName, "/Trusts/Overview/TrustDetails", Uid,
                false, "overview-nav"),
            new TrustNavigationLinkModel(ViewConstants.ContactsPageName, "/Trusts/Contacts/InDfe", Uid, false,
                "contacts-nav"),
            new TrustNavigationLinkModel("Academies (1)", "/Trusts/Academies/InTrust/Details",
                Uid, true, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/CurrentRatings", Uid, false,
                "ofsted-nav"),
            new TrustNavigationLinkModel(ViewConstants.GovernancePageName, "/Trusts/Governance/TrustLeadership",
                Uid, false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_and_TabList_to_correct_value()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().Equal([
            new TrustSubNavigationLinkModel("In the trust (1)",
                "/Trusts/Academies/InTrust/Details", Uid,
                ViewConstants.AcademiesPageName, false),
            new TrustSubNavigationLinkModel("Pipeline academies (6)",
                "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid,
                ViewConstants.AcademiesPageName, true)
        ]);
        _sut.TabList.Should().BeEquivalentTo([
            new TrustTabNavigationLinkModel("Pre advisory board (1)",
                "./PreAdvisoryBoard", Uid, "Pipeline", true),
            new TrustTabNavigationLinkModel("Post advisory board (2)",
                "./PostAdvisoryBoard", Uid, "Pipeline", false),
            new TrustTabNavigationLinkModel("Free schools (3)", "./FreeSchools", Uid,
                "Pipeline", false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.PipelineAcademiesPreAdvisoryBoardPageName);
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.AcademiesPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("Test Trust");
    }
}

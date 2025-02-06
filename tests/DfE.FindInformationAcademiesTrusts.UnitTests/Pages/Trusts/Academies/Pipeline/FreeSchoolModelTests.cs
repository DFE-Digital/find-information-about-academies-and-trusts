using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Pipeline;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.Pipeline;

public class FreeSchoolModelTests
{
    private readonly FreeSchoolsModel _sut;
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<IFeatureManager> _mockFeatureManager = new();

    private const string Uid = "1234";

    public FreeSchoolModelTests()
    {
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(Uid, testTrustName, testTrustType,
                1));
        _mockAcademyService.Setup(t => t.GetAcademiesPipelineSummaryAsync(Uid))
            .ReturnsAsync(new AcademyPipelineSummaryServiceModel(1, 2, 3));
        _mockFeatureManager.Setup(s => s.IsEnabledAsync(FeatureFlags.PipelineAcademies)).ReturnsAsync(true);
        _sut = new FreeSchoolsModel(
                _mockDataSourceService.Object, new MockLogger<FreeSchoolsModel>().Object,
                _mockTrustService.Object, _mockAcademyService.Object, _mockExportService.Object,
                _mockDateTimeProvider.Object, _mockFeatureManager.Object)
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
        // Arrange
        var academies = new[]
        {
        new AcademyPipelineServiceModel("1234", "Baking academy", new AgeRange(4, 16), "Bristol", "Conversion",
            new DateTime(2025, 3, 3)),
        new AcademyPipelineServiceModel("1234", "Chocolate academy", new AgeRange(11, 18), "Birmingham", "Conversion",
            new DateTime(2025, 5, 3)),
        new AcademyPipelineServiceModel("1234", "Fruity academy", new AgeRange(9, 16), "Sheffield", "Transfer",
            new DateTime(2025, 9, 3)),
        new AcademyPipelineServiceModel(null, null, null, null, null, null)
    };

        _mockAcademyService
        .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync("1234"))
        .ReturnsAsync("1234");

        _mockAcademyService
            .Setup(a => a.GetAcademiesPipelineFreeSchoolsAsync("1234"))
            .ReturnsAsync(academies);

        // Act
        await _sut.OnGetAsync();

        // Assert
        _sut.PipelineFreeSchools.Should().BeEquivalentTo(academies);
    }


    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        // Arrange
        _mockAcademyService
        .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync("1234"))
        .ReturnsAsync("1234");

        // Act
        _ = await _sut.OnGetAsync();

        // Assert
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel(ViewConstants.OverviewPageName, "/Trusts/Overview/TrustDetails", Uid,
                false, "overview-nav"),
            new TrustNavigationLinkModel(ViewConstants.ContactsPageName, "/Trusts/Contacts/InDfe", Uid, false,
                "contacts-nav"),
            new TrustNavigationLinkModel("Academies (1)", "/Trusts/Academies/InTrust/Details",
                Uid, true, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/SingleHeadlineGrades", Uid, false,
                "ofsted-nav"),
            new TrustNavigationLinkModel(ViewConstants.GovernancePageName, "/Trusts/Governance/TrustLeadership",
                Uid, false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_and_TabList_to_correct_value()
    {
        // Arrange
        _mockAcademyService
        .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync("1234"))
        .ReturnsAsync("1234");

        // Act 
        _ = await _sut.OnGetAsync();

        // Assert
        _sut.SubNavigationLinks.Should().Equal([
            new TrustSubNavigationLinkModel("In this trust (1)",
                "/Trusts/Academies/InTrust/Details", Uid,
                ViewConstants.AcademiesPageName, false),
            new TrustSubNavigationLinkModel("Pipeline academies (6)",
                "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid,
                ViewConstants.AcademiesPageName, true)
        ]);
        _sut.TabList.Should().BeEquivalentTo([
            new TrustTabNavigationLinkModel("Pre advisory board (1)",
                "./PreAdvisoryBoard", Uid, "Pipeline", false),
            new TrustTabNavigationLinkModel("Post advisory board (2)",
                "./PostAdvisoryBoard", Uid, "Pipeline", false),
            new TrustTabNavigationLinkModel("Free schools (3)", "./FreeSchools", Uid,
                "Pipeline", true)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        // Arrange
        _mockAcademyService
        .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync("1234"))
        .ReturnsAsync("1234");

        // Act
        _ = await _sut.OnGetAsync();

        // Assert 
        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.PipelineAcademiesFreeSchoolsPageName);
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.AcademiesPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("Test Trust");
    }
}
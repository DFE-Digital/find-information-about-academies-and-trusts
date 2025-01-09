using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class PupilNumbersModelTests
{
    private readonly PupilNumbersModel _sut;
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustRepository = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<DateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private const string Uid = "1234";

    public PupilNumbersModelTests()
    {
        MockLogger<PupilNumbersModel> logger = new();
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(Uid, testTrustName, testTrustType,
                1));
        _mockAcademyService.Setup(t => t.GetAcademiesInTrustPupilNumbersAsync(Uid))
            .ReturnsAsync([
                new AcademyPupilNumbersServiceModel(Uid, testTrustName, "Phase",
                    new AgeRange("11", "16"), 100, 200)
            ]);
        _mockAcademyService.Setup(t => t.GetAcademiesPipelineSummary())
            .Returns(new AcademyPipelineSummaryServiceModel(1, 2, 3));

        _sut = new PupilNumbersModel(_mockDataSourceService.Object, logger.Object,
                _mockTrustRepository.Object, _mockAcademyService.Object, _mockExportService.Object,
                _mockDateTimeProvider.Object)
            { Uid = Uid };
    }

    [Theory]
    [InlineData("Primary", 5, 11, "Primary0511")]
    [InlineData("Primary", 5, 9, "Primary0509")]
    [InlineData("Primary", 0, 7, "Primary0007")]
    [InlineData("16 plus", 16, 19, "16 plus1619")]
    [InlineData("Secondary", 10, 18, "Secondary1018")]
    public void PhaseAndAgeRangeSortValue_should_be_amalgamation_of_Phase_and_age_range_properties(string phase,
        int minAge, int maxAge, string expected)
    {
        var ageRange = new AgeRange(minAge, maxAge);
        var testAcademyUrn = Uid;
        var testAcademyName = "Test Academy";
        var testAcademyNumberOfPupils = 100;
        var testAcademySchoolCapacity = 100;

        var result = PupilNumbersModel.PhaseAndAgeRangeSortValue(new AcademyPupilNumbersServiceModel(testAcademyUrn,
            testAcademyName, phase, ageRange,
            testAcademyNumberOfPupils, testAcademySchoolCapacity));
        result.Should().Be(expected);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustRepository.Setup(t => t.GetTrustSummaryAsync(Uid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_returns_RegularPageResult_if_Trust_is_found()
    {
        var result = await _sut.OnGetAsync();
        result.Should().NotBeOfType<NotFoundResult>();
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
    public async Task OnGetAsync_sets_SubNavigationLinks_toEmptyArray()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().Equal([
            new TrustSubNavigationLinkModel("In the trust (1)",
                "/Trusts/Academies/InTrust/Details", Uid,
                "Academies", true),
            new TrustSubNavigationLinkModel("Pipeline academies (6)",
                "/Trusts/Academies/Pipeline/PreAdvisoryBoard", Uid,
                "Academies", false)
        ]);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesPupilNumbersPageName);
        _sut.TrustPageMetadata.PageName.Should().Be("Academies");
        _sut.TrustPageMetadata.TrustName.Should().Be("Test Trust");
    }
}

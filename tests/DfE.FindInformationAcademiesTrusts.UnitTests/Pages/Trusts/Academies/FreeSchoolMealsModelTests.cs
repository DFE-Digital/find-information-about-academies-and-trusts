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

public class FreeSchoolMealsModelTests
{
    private readonly FreeSchoolMealsModel _sut;
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockDataSourceService _mockDataSourceService = new();
    private const string Uid = "1234";

    public FreeSchoolMealsModelTests()
    {
        var testTrustName = "Test Trust";
        var testTrustType = "SAT";

        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(Uid))
            .ReturnsAsync(new TrustSummaryServiceModel(Uid, testTrustName, testTrustType,
                1));
        _mockAcademyService.Setup(t => t.GetAcademiesPipelineSummary())
            .Returns(new AcademyPipelineSummaryServiceModel(1, 2, 3));
        _sut = new FreeSchoolMealsModel(
                _mockDataSourceService.Object, new MockLogger<FreeSchoolMealsModel>().Object,
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
        var academies = new[]
        {
            new AcademyFreeSchoolMealsServiceModel("1", "Academy 1", 12.5, 13.5, 14.5),
            new AcademyFreeSchoolMealsServiceModel("2", "Academy 2", null, 70.1, 64.1),
            new AcademyFreeSchoolMealsServiceModel("3", "Academy 3", 8.2, 4, 10)
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustFreeSchoolMealsAsync(_sut.Uid))
            .ReturnsAsync(academies);

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

        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesFreeSchoolMealsPageName);
        _sut.TrustPageMetadata.PageName.Should().Be("Academies");
        _sut.TrustPageMetadata.TrustName.Should().Be("Test Trust");
    }
}

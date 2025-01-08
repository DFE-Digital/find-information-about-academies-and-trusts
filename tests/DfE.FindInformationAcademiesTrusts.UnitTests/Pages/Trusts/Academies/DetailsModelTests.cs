using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.Current;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class CurrentAcademiesDetailsModelTests
{
    private readonly CurrentAcademiesDetailsModel _sut;
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinkBuilder = new();
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly MockLogger<CurrentAcademiesDetailsModel> _mockLogger = new();

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    public CurrentAcademiesDetailsModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_fakeTrust.Uid))
            .ReturnsAsync(_fakeTrust);

        _sut = new CurrentAcademiesDetailsModel(_mockDataSourceService.Object,
                _mockLinkBuilder.Object, _mockLogger.Object, _mockTrustService.Object, _mockAcademyService.Object,
                _mockExportService.Object, _mockDateTimeProvider.Object)
            { Uid = _fakeTrust.Uid };
    }

    [Fact]
    public void OtherServicesLinkBuilder_should_be_injected()
    {
        _sut.LinkBuilder.Should().Be(_mockLinkBuilder.Object);
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_sut.Uid)).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyDetailsServiceModel("1", "", "", "", ""),
            new AcademyDetailsServiceModel("2", "", "", "", ""),
            new AcademyDetailsServiceModel("3", "", "", "", "")
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustDetailsAsync(_sut.Uid))
            .ReturnsAsync(academies);

        _ = await _sut.OnGetAsync();

        _sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public async Task OnGetAsync_sets_correct_NavigationLinks()
    {
        _ = await _sut.OnGetAsync();
        _sut.NavigationLinks.Should().BeEquivalentTo([
            new TrustNavigationLinkModel(ViewConstants.OverviewPageName, "/Trusts/Overview/TrustDetails", "1234",
                false, "overview-nav"),
            new TrustNavigationLinkModel(ViewConstants.ContactsPageName, "/Trusts/Contacts/InDfe", "1234", false,
                "contacts-nav"),
            new TrustNavigationLinkModel("Academies (3)", "/Trusts/Academies/Details",
                "1234", true, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/CurrentRatings", "1234", false,
                "ofsted-nav"),
            new TrustNavigationLinkModel(ViewConstants.GovernancePageName, "/Trusts/Governance/TrustLeadership",
                "1234", false,
                "governance-nav")
        ]);
    }

    [Fact]
    public async Task OnGetAsync_sets_SubNavigationLinks_toEmptyArray()
    {
        _ = await _sut.OnGetAsync();
        _sut.SubNavigationLinks.Should().Equal();
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesDetailsPageName);
        _sut.TrustPageMetadata.PageName.Should().Be("Academies");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class CurrentRatingsModelTests
{
    private readonly CurrentRatingsModel _sut;
    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();
    private readonly Mock<IExportService> _mockExportService = new();
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    public CurrentRatingsModelTests()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_fakeTrust.Uid))
            .ReturnsAsync(_fakeTrust);

        _sut = new CurrentRatingsModel(_mockDataSourceService.Object,
                _mockTrustService.Object,
                _mockAcademyService.Object,
                _mockExportService.Object,
                _mockDateTimeProvider.Object,
                new MockLogger<CurrentRatingsModel>().Object
            )
            { Uid = "1234" };
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyOfstedServiceModel("1", "Academy 1", new DateTime(2022, 12, 1),
                new OfstedRating(2, new DateTime(2023, 1, 1)),
                new OfstedRating(3, new DateTime(2023, 2, 1))),
            new AcademyOfstedServiceModel("2", "Academy 2", new DateTime(2022, 11, 2),
                new OfstedRating(2, new DateTime(2023, 1, 2)),
                new OfstedRating(3, new DateTime(2023, 3, 1))),
            new AcademyOfstedServiceModel("3", "Academy 3", new DateTime(2022, 10, 3),
                new OfstedRating(2, new DateTime(2023, 1, 3)),
                new OfstedRating(3, new DateTime(2023, 4, 1)))
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustOfstedAsync(_sut.Uid))
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
                "1234", false, "academies-nav"),
            new TrustNavigationLinkModel(ViewConstants.OfstedPageName, "/Trusts/Ofsted/CurrentRatings", "1234", true,
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
        _sut.SubNavigationLinks.Should().BeEquivalentTo([
            new TrustSubNavigationLinkModel(ViewConstants.OfstedCurrentRatingsPageName, "./CurrentRatings", "1234",
                ViewConstants.OfstedPageName,
                true),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedPreviousRatingsPageName, "./PreviousRatings", "1234",
                ViewConstants.OfstedPageName, false),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedImportantDatesPageName, "./ImportantDates", "1234",
                ViewConstants.OfstedPageName,
                false),
            new TrustSubNavigationLinkModel(ViewConstants.OfstedSafeguardingAndConcernsPageName,
                "./SafeguardingAndConcerns", "1234",
                ViewConstants.OfstedPageName, false)
        ]);
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid()
    {
        // Arrange
        var uid = "1234";

        var trustSummary = new TrustSummaryServiceModel(uid, "Sample Trust", "Multi-academy trust", 0);
        byte[] expectedBytes = [1, 2, 3];

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
        _mockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(uid)).ReturnsAsync(expectedBytes);


        // Act
        var result = await _sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        var fileResult = result as FileContentResult;
        fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        fileResult?.FileContents.Should().BeEquivalentTo(expectedBytes);
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldReturnNotFound_WhenUidIsInvalid()
    {
        // Arrange
        var uid = "invalid-uid";

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync((TrustSummaryServiceModel?)null);

        // Act
        var result = await _sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters()
    {
        // Arrange
        var uid = "1234";
        var trustSummary = new TrustSummaryServiceModel(uid, "Sample/Trust:Name?", "Multi-academy trust", 0);
        var expectedBytes = new byte[] { 1, 2, 3 };

        _mockTrustService.Setup(x => x.GetTrustSummaryAsync(uid)).ReturnsAsync(trustSummary);
        _mockExportService.Setup(x => x.ExportOfstedDataToSpreadsheetAsync(uid)).ReturnsAsync(expectedBytes);

        // Act
        var result = await _sut.OnGetExportAsync(uid);

        // Assert
        result.Should().BeOfType<FileContentResult>();
        var fileResult = result as FileContentResult;
        fileResult?.ContentType.Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        fileResult?.FileContents.Should().BeEquivalentTo(expectedBytes);
        fileResult?.FileDownloadName.Should().NotBeEmpty();

        // Verify that the file name is sanitized (no illegal characters)
        var fileDownloadName = fileResult?.FileDownloadName ?? string.Empty;
        var invalidFileNameChars = Path.GetInvalidFileNameChars();

        // Check that the file name doesn't contain any invalid characters
        var containsInvalidChars = fileDownloadName.Any(c => invalidFileNameChars.Contains(c));
        containsInvalidChars.Should().BeFalse("the file name should not contain any illegal characters");
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be(ViewConstants.OfstedCurrentRatingsPageName);
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.OfstedPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }
}

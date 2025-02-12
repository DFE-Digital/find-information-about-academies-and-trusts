using DfE.FindInformationAcademiesTrusts.Configuration;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using Microsoft.FeatureManagement;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public abstract class BaseAcademiesAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages, ITestTabPages, ITestExport
    where T : AcademiesAreaModel
{
    protected readonly Mock<IAcademyService> MockAcademyService = new();
    protected readonly Mock<IExportService> MockExportService = new();
    protected readonly Mock<IFeatureManager> MockFeatureManager = new();
    protected const string TrustReferenceNumber = "TRN00123";

    public BaseAcademiesAreaModelTests()
    {
        MockAcademyService
            .Setup(a => a.GetAcademyTrustTrustReferenceNumberAsync(TrustUid))
            .ReturnsAsync(TrustReferenceNumber);

        //Set default GetAcademiesPipelineSummaryAsync to enable base tests with different UIDs
        MockAcademyService
            .Setup(t => t.GetAcademiesPipelineSummaryAsync(It.IsAny<string>()))
            .ReturnsAsync(new AcademyPipelineSummaryServiceModel(0, 0, 0));

        MockAcademyService
            .Setup(t => t.GetAcademiesPipelineSummaryAsync(TrustReferenceNumber))
            .ReturnsAsync(new AcademyPipelineSummaryServiceModel(1, 2, 3));

        MockFeatureManager.Setup(s => s.IsEnabledAsync(FeatureFlags.PipelineAcademies)).ReturnsAsync(true);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Academies (3)");
    }

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("In this trust (3)");
                    l.SubPageLink.Should().Be("/Trusts/Academies/InTrust/Details");
                    l.ServiceName.Should().Be("Academies");
                },
                l =>
                {
                    l.LinkText.Should().Be("Pipeline academies (6)");
                    l.SubPageLink.Should().Be("/Trusts/Academies/Pipeline/PreAdvisoryBoard");
                    l.ServiceName.Should().Be("Academies");
                }
            );
    }

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages_when_pipeline_feature_disabled()
    {
        MockFeatureManager.Setup(s => s.IsEnabledAsync(FeatureFlags.PipelineAcademies)).ReturnsAsync(false);

        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("In this trust (3)");
                    l.SubPageLink.Should().Be("/Trusts/Academies/InTrust/Details");
                    l.ServiceName.Should().Be("Academies");
                }
            );
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Academies");
    }

    [Fact]
    public abstract Task OnGetAsync_sets_academies_from_academyService();

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();

    [Fact]
    public abstract Task OnGetAsync_should_set_active_TabList_to_current_tab();

    [Fact]
    public abstract Task OnGetAsync_should_populate_TabList_to_tabs();

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName();

    [Fact]
    public abstract Task OnGetExportAsync_ShouldReturnFileResult_WhenUidIsValid();

    [Fact]
    public abstract Task OnGetExportAsync_ShouldReturnNotFound_WhenUidIsInvalid();

    [Fact]
    public abstract Task OnGetExportAsync_ShouldSanitizeTrustName_WhenTrustNameContainsIllegalCharacters();
}

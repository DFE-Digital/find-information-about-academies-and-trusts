using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Export;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public abstract class BaseAcademiesAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages, ITestTabPages, ITestExport
    where T : AcademiesAreaModel
{
    protected readonly IAcademyService MockAcademyService = Substitute.For<IAcademyService>();
    protected readonly IExportService MockExportService = Substitute.For<IExportService>();
    protected const string TrustReferenceNumber = "TRN00123";

    public BaseAcademiesAreaModelTests()
    {
        MockTrustService.GetTrustReferenceNumberAsync(TrustUid).Returns(Task.FromResult(TrustReferenceNumber));

        //Set default GetAcademiesPipelineSummaryAsync to enable base tests with different UIDs
        MockAcademyService
            .GetAcademiesPipelineSummaryAsync(Arg.Any<string>())
            .Returns(Task.FromResult(new AcademyPipelineSummaryServiceModel(0, 0, 0)));

        MockAcademyService
            .GetAcademiesPipelineSummaryAsync(TrustReferenceNumber)
            .Returns(Task.FromResult(new AcademyPipelineSummaryServiceModel(1, 2, 3)));
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

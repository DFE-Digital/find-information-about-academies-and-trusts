using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public abstract class BaseFinancialDocumentsAreaModelTests<T> : BaseTrustPageTests<T>, ITestSubpages
    where T : FinancialDocumentsAreaModel
{
    protected readonly IFinancialDocumentService MockFinancialDocumentService =
        Substitute.For<IFinancialDocumentService>();

    private readonly FinancialDocumentServiceModel[] _unsortedFinancialDocs =
    [
        new(2023, 2024, FinancialDocumentStatus.NotYetSubmitted),
        new(2022, 2023, FinancialDocumentStatus.NotExpected),
        new(2024, 2025, FinancialDocumentStatus.Submitted, "https://www.google.com")
    ];

    protected BaseFinancialDocumentsAreaModelTests(FinancialDocumentType financialDocumentType)
    {
        MockFinancialDocumentService.GetFinancialDocumentsAsync(TrustUid, financialDocumentType)
            .Returns(_unsortedFinancialDocs);
    }

    [Fact]
    public override async Task OnGetAsync_sets_correct_data_source_list()
    {
        _ = await Sut.OnGetAsync();

        await MockDataSourceService.DidNotReceive().GetAsync(Arg.Any<Source>());
        Sut.DataSourcesPerPage.Should().BeEmpty();
    }

    [Fact]
    public async Task OnGetAsync_sets_FinancialDocuments_in_descending_order_for_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.FinancialDocuments.Should().BeEquivalentTo(_unsortedFinancialDocs);
        Sut.FinancialDocuments.Should().BeInDescendingOrder(d => d.YearTo);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_NavigationLink_to_current_page()
    {
        _ = await Sut.OnGetAsync();

        Sut.NavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.LinkText.Should().Be("Financial documents");
    }

    [Fact]
    public abstract Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();

    [Fact]
    public async Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should()
            .SatisfyRespectively(
                l =>
                {
                    l.LinkText.Should().Be("Financial statements");
                    l.SubPageLink.Should().Be("./FinancialStatements");
                    l.ServiceName.Should().Be("Financial documents");
                },
                l =>
                {
                    l.LinkText.Should().Be("Management letters");
                    l.SubPageLink.Should().Be("./ManagementLetters");
                    l.ServiceName.Should().Be("Financial documents");
                },
                l =>
                {
                    l.LinkText.Should().Be("Internal scrutiny reports");
                    l.SubPageLink.Should().Be("./InternalScrutinyReports");
                    l.ServiceName.Should().Be("Financial documents");
                },
                l =>
                {
                    l.LinkText.Should().Be("Self-assessment checklists");
                    l.SubPageLink.Should().Be("./SelfAssessmentChecklists");
                    l.ServiceName.Should().Be("Financial documents");
                });
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_PageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.PageName.Should().Be("Financial documents");
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}

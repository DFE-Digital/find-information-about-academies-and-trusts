using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class FinancialStatementModelTests : BaseFinancialDocumentsAreaModelTests<FinancialStatementsModel>
{
    public FinancialStatementModelTests() : base(FinancialDocumentType.FinancialStatement)
    {
        Sut = new FinancialStatementsModel(MockDataSourceService, new MockLogger<FinancialStatementsModel>().Object,
                MockTrustService.Object, MockFinancialDocumentService.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./FinancialStatements");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Financial statements");
    }

    [Fact]
    public void InternalUseOnly_should_be_false()
    {
        Sut.InternalUseOnly.Should().BeFalse();
    }

    [Fact]
    public void FinancialDocumentDisplayName_should_be_financial_statement()
    {
        Sut.FinancialDocumentDisplayName.Should().Be("financial statement");
    }
}

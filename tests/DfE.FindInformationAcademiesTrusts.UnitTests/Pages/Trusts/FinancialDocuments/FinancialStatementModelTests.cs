using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class FinancialStatementModelTests : BaseFinancialDocumentsAreaModelTests<FinancialStatementsModel>
{
    public FinancialStatementModelTests() : base(FinancialDocumentType.FinancialStatement)
    {
        Sut = new FinancialStatementsModel(MockDataSourceService,
                MockTrustService, MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Financial statements");
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

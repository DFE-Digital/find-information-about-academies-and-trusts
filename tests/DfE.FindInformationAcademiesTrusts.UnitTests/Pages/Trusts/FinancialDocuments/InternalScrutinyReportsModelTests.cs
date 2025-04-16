using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class InternalScrutinyReportsModelTests : BaseFinancialDocumentsAreaModelTests<InternalScrutinyReportsModel>
{
    public InternalScrutinyReportsModelTests() : base(FinancialDocumentType.InternalScrutinyReport)
    {
        Sut = new InternalScrutinyReportsModel(MockDataSourceService, MockTrustService,
                MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Internal scrutiny reports");
    }

    [Fact]
    public void InternalUseOnly_should_be_true()
    {
        Sut.InternalUseOnly.Should().BeTrue();
    }

    [Fact]
    public void FinancialDocumentDisplayName_should_be_scrutiny_report()
    {
        Sut.FinancialDocumentDisplayName.Should().Be("scrutiny report");
    }
}

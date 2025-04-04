using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class InternalScrutinyReportsModelTests : BaseFinancialDocumentsAreaModelTests<InternalScrutinyReportsModel>
{
    public InternalScrutinyReportsModelTests() : base(FinancialDocumentType.InternalScrutinyReport)
    {
        Sut = new InternalScrutinyReportsModel(MockDataSourceService,
                MockLogger.CreateLogger<InternalScrutinyReportsModel>(), MockTrustService,
                MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./InternalScrutinyReports");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Internal scrutiny reports");
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

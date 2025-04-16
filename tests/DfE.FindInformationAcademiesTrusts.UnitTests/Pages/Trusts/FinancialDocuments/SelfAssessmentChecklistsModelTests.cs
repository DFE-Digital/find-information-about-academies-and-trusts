using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class SelfAssessmentChecklistsModelTests : BaseFinancialDocumentsAreaModelTests<SelfAssessmentChecklistsModel>
{
    public SelfAssessmentChecklistsModelTests() : base(FinancialDocumentType.SelfAssessmentChecklist)
    {
        Sut = new SelfAssessmentChecklistsModel(MockDataSourceService, MockTrustService,
                MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.PageMetadata.SubPageName.Should().Be("Self-assessment checklists");
    }

    [Fact]
    public void InternalUseOnly_should_be_true()
    {
        Sut.InternalUseOnly.Should().BeTrue();
    }

    [Fact]
    public void FinancialDocumentDisplayName_should_be_self_assessment_checklist()
    {
        Sut.FinancialDocumentDisplayName.Should().Be("self-assessment checklist");
    }
}

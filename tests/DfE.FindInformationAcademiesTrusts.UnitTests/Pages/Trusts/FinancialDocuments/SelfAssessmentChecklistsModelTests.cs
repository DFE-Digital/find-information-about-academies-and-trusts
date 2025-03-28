using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class SelfAssessmentChecklistsModelTests : BaseFinancialDocumentsAreaModelTests<SelfAssessmentChecklistsModel>
{
    public SelfAssessmentChecklistsModelTests() : base(FinancialDocumentType.SelfAssessmentChecklist)
    {
        Sut = new SelfAssessmentChecklistsModel(MockDataSourceService,
                MockLogger.CreateLogger<SelfAssessmentChecklistsModel>(), MockTrustService,
                MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage()
    {
        _ = await Sut.OnGetAsync();

        Sut.SubNavigationLinks.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./SelfAssessmentChecklists");
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Self-assessment checklists");
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

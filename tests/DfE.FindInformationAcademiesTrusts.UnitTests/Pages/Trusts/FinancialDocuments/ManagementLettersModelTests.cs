using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.FinancialDocuments;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.FinancialDocuments;

public class ManagementLettersModelTests : BaseFinancialDocumentsAreaModelTests<ManagementLettersModel>
{
    public ManagementLettersModelTests() : base(FinancialDocumentType.ManagementLetter)
    {
        Sut = new ManagementLettersModel(MockDataSourceService, MockLogger.CreateLogger<ManagementLettersModel>(),
                MockTrustService, MockFinancialDocumentService)
            { Uid = TrustUid };
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.SubPageName.Should().Be("Management letters");
    }

    [Fact]
    public void InternalUseOnly_should_be_true()
    {
        Sut.InternalUseOnly.Should().BeTrue();
    }

    [Fact]
    public void FinancialDocumentDisplayName_should_be_management_letter()
    {
        Sut.FinancialDocumentDisplayName.Should().Be("management letter");
    }
}

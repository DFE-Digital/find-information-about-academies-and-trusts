using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class FinancialDocumentServiceTests
{
    private readonly ITrustDocumentRepository _mockTrustDocumentRepository = Substitute.For<ITrustDocumentRepository>();
    private readonly FinancialDocumentService _sut;

    public FinancialDocumentServiceTests()
    {
        _sut = new FinancialDocumentService(_mockTrustDocumentRepository);
    }

    [Theory]
    [InlineData("TR01234", FinancialDocumentType.FinancialStatement)]
    [InlineData("TR09876", FinancialDocumentType.ManagementLetter)]
    public async Task GetFinancialDocumentsAsync_should_return_links_from_TrustDocumentRepository(
        string trustReferenceNumber, FinancialDocumentType documentType)
    {
        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(trustReferenceNumber, documentType)
            .Returns([
                    new TrustDocument(new DateOnly(2023, 9, 1), new DateOnly(2024, 8, 31), "www.link1.com"),
                    new TrustDocument(new DateOnly(2022, 9, 1), new DateOnly(2023, 8, 31), "www.link2.com"),
                    new TrustDocument(new DateOnly(2021, 9, 1), new DateOnly(2022, 8, 31), "www.link3.com")
                ]
            );

        var result = await _sut.GetFinancialDocumentsAsync(trustReferenceNumber, documentType);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.Submitted, "www.link1.com"),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.Submitted, "www.link2.com"),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.Submitted, "www.link3.com")
        ]);
    }
}

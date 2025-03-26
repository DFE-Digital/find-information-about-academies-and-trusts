using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class TrustDocumentRepositoryTests
{
    private readonly TrustDocumentRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDb = new();

    public TrustDocumentRepositoryTests()
    {
        _sut = new TrustDocumentRepository(_mockAcademiesDb.Object);
    }

    [Theory]
    [InlineData(FinancialDocumentType.FinancialStatement, "AFS", "FS")]
    [InlineData(FinancialDocumentType.ManagementLetter, "AML", "ML")]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "ISR")]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "SRMSAT", "SRMSAC")]
    public async Task
        GetFinancialDocumentsAsync_should_only_return_financial_documents_for_given_trust_and_FinancialDocumentType(
            FinancialDocumentType docTypeUnderTest, params string[] folderPrefixes)
    {
        //Add trust docs which should not be retrieved
        _mockAcademiesDb.AddTrustDocLinks("TR_some_other_trust", "AFS", 10);
        _mockAcademiesDb.AddTrustDocLinks("TR01234", "Not the folder prefix we're looking for", 5);

        //Add trust docs which should be retrieved
        foreach (var folderPrefix in folderPrefixes)
        {
            _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
            {
                FolderPrefix = folderPrefix,
                TrustRefNumber = "TR01234",
                DocumentFilename = $"Trust Document for {folderPrefix}",
                DocumentLink = $"www.linkto-{folderPrefix}.com",
                FolderYear = 2022
            });
        }

        var result = await _sut.GetFinancialDocumentsAsync("TR01234", docTypeUnderTest);

        result.Should().HaveCount(folderPrefixes.Length);
    }

    [Theory]
    [InlineData(FinancialDocumentType.FinancialStatement, "AFS", 2024)]
    [InlineData(FinancialDocumentType.FinancialStatement, "FS", 2023)]
    [InlineData(FinancialDocumentType.ManagementLetter, "AML", 2022)]
    [InlineData(FinancialDocumentType.ManagementLetter, "ML", 2021)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "ISR", 2019)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "SRMSAT", 2018)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "SRMSAC", 2017)]
    public async Task GetFinancialDocumentsAsync_should_map_TrustDocLinks_to_TrustDocument(
        FinancialDocumentType financialDocType, string folderPrefix, int year)
    {
        var link = $"www.link-to-TR01234-{folderPrefix}-{year}.com";

        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = folderPrefix,
            TrustRefNumber = "TR01234",
            DocumentFilename = $"Trust Document for {folderPrefix} {year}",
            DocumentLink = link,
            FolderYear = year
        });

        var result = await _sut.GetFinancialDocumentsAsync("TR01234", financialDocType);

        var actualDoc = result.Should().ContainSingle().Subject;
        actualDoc.FinancialYear.Should().Be(new FinancialYear(year));
        actualDoc.Link.Should().Be(link);
    }
}

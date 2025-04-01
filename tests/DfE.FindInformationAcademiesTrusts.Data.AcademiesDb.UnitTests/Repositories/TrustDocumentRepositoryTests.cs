using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Repositories;

public class TrustDocumentRepositoryTests
{
    private readonly TrustDocumentRepository _sut;
    private readonly MockAcademiesDbContext _mockAcademiesDb = new();
    private readonly GiasGroup _giasGroup;
    private readonly ILogger<TrustDocumentRepository> _mockLogger = MockLogger.CreateLogger<TrustDocumentRepository>();
    private string Uid => _giasGroup.GroupUid!;
    private string TrustReferenceNumber => _giasGroup.GroupId!;

    public TrustDocumentRepositoryTests()
    {
        _giasGroup = _mockAcademiesDb.AddGiasGroup();
        _giasGroup.IncorporatedOnOpenDate = "01/01/2020";

        _sut = new TrustDocumentRepository(_mockAcademiesDb.Object, _mockLogger);
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
        _mockAcademiesDb.AddTrustDocLinks(TrustReferenceNumber, "Not the folder prefix we're looking for", 5);

        //Add trust docs which should be retrieved
        for (var i = 0; i < folderPrefixes.Length; i++)
        {
            var folderPrefix = folderPrefixes[i];
            _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
            {
                FolderPrefix = folderPrefix,
                TrustRefNumber = TrustReferenceNumber,
                DocumentFilename = $"Trust Document for {folderPrefix}",
                DocumentLink = $"www.linkto-{folderPrefix}.com",
                FolderYear = 2000 + i
            });
        }

        var result = await _sut.GetFinancialDocumentsAsync(Uid, docTypeUnderTest);

        result.financialDocuments.Should().HaveCount(folderPrefixes.Length);
    }

    [Theory]
    [InlineData(FinancialDocumentType.FinancialStatement, "AFS")]
    [InlineData(FinancialDocumentType.FinancialStatement, "FS")]
    [InlineData(FinancialDocumentType.ManagementLetter, "AML")]
    [InlineData(FinancialDocumentType.ManagementLetter, "ML")]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "ISR")]
    public async Task GetFinancialDocumentsAsync_should_set_document_link_in_trust_documents(
        FinancialDocumentType financialDocType, string folderPrefix)
    {
        var expectedLinks = _mockAcademiesDb.AddTrustDocLinks(TrustReferenceNumber, folderPrefix, 3)
            .Select(d => d.DocumentLink);

        var result = await _sut.GetFinancialDocumentsAsync(Uid, financialDocType);

        result.financialDocuments.Select(d => d.Link).Should().BeEquivalentTo(expectedLinks);
    }

    [Theory]
    [InlineData(FinancialDocumentType.FinancialStatement, "AFS", 2024)]
    [InlineData(FinancialDocumentType.FinancialStatement, "FS", 2023)]
    [InlineData(FinancialDocumentType.ManagementLetter, "AML", 2022)]
    [InlineData(FinancialDocumentType.ManagementLetter, "ML", 2021)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "ISR", 2019)]
    public async Task GetFinancialDocumentsAsync_should_set_financial_year_end_to_folder_year_for_most_doc_types(
        FinancialDocumentType financialDocType, string folderPrefix, int folderYear)
    {
        var link = $"www.link-to-{folderPrefix}-{folderYear}.com";

        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = folderPrefix,
            TrustRefNumber = TrustReferenceNumber,
            DocumentFilename = $"Trust Document for {folderPrefix} {folderYear}",
            DocumentLink = link,
            FolderYear = folderYear
        });

        var result = await _sut.GetFinancialDocumentsAsync(Uid, financialDocType);

        result.financialDocuments.Should().ContainSingle()
            .Which.FinancialYear.Should().Be(new FinancialYear(folderYear));
    }

    [Theory]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "SRMSAT", 2025, 2024)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "SRMSAC", 2017, 2016)]
    public async Task
        GetFinancialDocumentsAsync_should_set_financial_year_end_to_year_before_folder_year_for_SelfAssessmentChecklist(
            FinancialDocumentType financialDocType, string folderPrefix, int folderYear,
            int expectedFinancialYearEndYear)
    {
        var link = $"www.link-to-{folderPrefix}-{folderYear}.com";

        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = folderPrefix,
            TrustRefNumber = TrustReferenceNumber,
            DocumentFilename = $"Trust Document for {folderPrefix} {folderYear}",
            DocumentLink = link,
            FolderYear = folderYear
        });

        var result = await _sut.GetFinancialDocumentsAsync(Uid, financialDocType);

        result.financialDocuments.Should().ContainSingle()
            .Which.FinancialYear.Should().Be(new FinancialYear(expectedFinancialYearEndYear));
    }

    [Fact]
    public async Task GetFinancialDocumentsAsync_should_return_trust_open_date_when_docs_exist()
    {
        _giasGroup.IncorporatedOnOpenDate = "29/02/2024";
        _mockAcademiesDb.AddTrustDocLinks(TrustReferenceNumber, "AML", 5);

        var result = await _sut.GetFinancialDocumentsAsync(Uid, FinancialDocumentType.ManagementLetter);

        result.trustOpenDate.Should().Be(new DateOnly(2024, 2, 29));
        result.financialDocuments.Should().HaveCount(5);
    }

    [Fact]
    public async Task GetFinancialDocumentsAsync_should_return_trust_open_date_when_no_docs_exist()
    {
        _giasGroup.IncorporatedOnOpenDate = "28/02/2023";

        var result = await _sut.GetFinancialDocumentsAsync(Uid, FinancialDocumentType.ManagementLetter);

        result.trustOpenDate.Should().Be(new DateOnly(2023, 2, 28));
        result.financialDocuments.Should().BeEmpty();
    }

    [Fact]
    public async Task GetFinancialDocumentsAsync_should_log_error_when_no_trust_open_date()
    {
        _giasGroup.IncorporatedOnOpenDate = null;

        var result = await _sut.GetFinancialDocumentsAsync(Uid, FinancialDocumentType.ManagementLetter);

        result.trustOpenDate.Should().Be(DateOnly.MinValue);
        _mockLogger.VerifyLogError(
            $"Open date was not found for trust {Uid}. This should never happen and indicates a data integrity issue with the GIAS data in Academies Db");
    }

    [Fact]
    public async Task GetFinancialDocumentsAsync_should_return_newest_doc_when_more_than_one_in_year()
    {
        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = "AFS",
            TrustRefNumber = TrustReferenceNumber,
            DocumentFilename = "Old Trust Document",
            DocumentLink = "www.old.com",
            CreatedDateTime = new DateTime(2019, 11, 01, 11, 21, 31, DateTimeKind.Utc),
            FolderYear = 2019
        });
        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = "AFS",
            TrustRefNumber = TrustReferenceNumber,
            DocumentFilename = "New Trust Document",
            DocumentLink = "www.new.com",
            CreatedDateTime = new DateTime(2019, 12, 30, 9, 29, 39, DateTimeKind.Utc),
            FolderYear = 2019
        });
        _mockAcademiesDb.AddTrustDocLink(new SharepointTrustDocLink
        {
            FolderPrefix = "AFS",
            TrustRefNumber = TrustReferenceNumber,
            DocumentFilename = "Oldest Trust Document",
            DocumentLink = "www.oldest.com",
            CreatedDateTime = new DateTime(2019, 10, 01, 1, 2, 3, DateTimeKind.Utc),
            FolderYear = 2019
        });

        var result = await _sut.GetFinancialDocumentsAsync(Uid, FinancialDocumentType.FinancialStatement);

        var actualDoc = result.financialDocuments.Should().ContainSingle().Subject;
        actualDoc.Link.Should().Be("www.new.com");
    }
}

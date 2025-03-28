using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;
using DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class FinancialDocumentServiceTests
{
    private readonly ITrustDocumentRepository _mockTrustDocumentRepository = Substitute.For<ITrustDocumentRepository>();
    private readonly IDateTimeProvider _mockDateTimeProvider = Substitute.For<IDateTimeProvider>();
    private readonly FinancialDocumentService _sut;

    private DateTime _today = new(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public FinancialDocumentServiceTests()
    {
        _mockDateTimeProvider.Today.Returns(_ => _today);

        _sut = new FinancialDocumentService(_mockTrustDocumentRepository, _mockDateTimeProvider);
    }

    [Theory]
    [InlineData("1234", FinancialDocumentType.FinancialStatement)]
    [InlineData("5678", FinancialDocumentType.ManagementLetter)]
    public async Task
        GetFinancialDocumentsAsync_should_return_links_from_TrustDocumentRepository_for_given_documentType(
            string uid, FinancialDocumentType documentType)
    {
        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(uid, documentType)
            .Returns(([
                    new TrustDocument(2024, "www.link1.com"),
                    new TrustDocument(2023, "www.link2.com"),
                    new TrustDocument(2022, "www.link3.com")
                ], new DateOnly(2015, 2, 2))
            );

        var result = await _sut.GetFinancialDocumentsAsync(uid, documentType);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.Submitted, "www.link1.com"),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.Submitted, "www.link2.com"),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.Submitted, "www.link3.com")
        ]);
    }

    [Theory]
    [InlineData(FinancialDocumentType.FinancialStatement, "2025-01-01", 2024)]
    [InlineData(FinancialDocumentType.FinancialStatement, "2025-09-30", 2024)]
    [InlineData(FinancialDocumentType.FinancialStatement, "2025-10-01", 2025)]
    [InlineData(FinancialDocumentType.FinancialStatement, "2025-12-31", 2025)]
    [InlineData(FinancialDocumentType.ManagementLetter, "2025-01-01", 2024)]
    [InlineData(FinancialDocumentType.ManagementLetter, "2025-09-30", 2024)]
    [InlineData(FinancialDocumentType.ManagementLetter, "2025-10-01", 2025)]
    [InlineData(FinancialDocumentType.ManagementLetter, "2025-12-31", 2025)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "2025-01-01", 2024)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "2025-09-30", 2024)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "2025-10-01", 2025)]
    [InlineData(FinancialDocumentType.InternalScrutinyReport, "2025-12-31", 2025)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "2024-01-01", 2023)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "2024-12-31", 2023)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "2025-01-01", 2024)]
    [InlineData(FinancialDocumentType.SelfAssessmentChecklist, "2025-12-31", 2024)]
    public void
        GetFinancialYearsToDisplay_should_return_latest_year_with_open_submission_window_and_two_years_before_that(
            FinancialDocumentType financialDocumentType, string today, int openSubmissionWindowYear)
    {
        _today = DateTime.ParseExact(today, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        var result = _sut.GetFinancialYearsToDisplay(financialDocumentType);

        result.Should().ContainSingle(doc =>
                doc.SubmissionWindowStatus == FinancialDocumentService.FinancialYearSubmissionWindowStatus.Open)
            .Which.FinancialYear.End.Year.Should().Be(openSubmissionWindowYear);
        result.Select(doc => doc.FinancialYear.End.Year).Should().BeEquivalentTo([
            openSubmissionWindowYear, openSubmissionWindowYear - 1, openSubmissionWindowYear - 2
        ]);
    }

    [Fact]
    public async Task
        GetFinancialDocumentsAsync_should_return_not_expected_when_no_link_and_trust_opened_after_financial_year_ended()
    {
        const string uid = "1234";

        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(uid, Arg.Any<FinancialDocumentType>())
            .Returns((
                    financialDocuments:
                    [
                        new TrustDocument(2024, "www.link1.com")
                    ],
                    trustOpenDate: new DateOnly(2023, 9, 1))
            );

        var result = await _sut.GetFinancialDocumentsAsync(uid, FinancialDocumentType.ManagementLetter);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.Submitted, "www.link1.com"),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.NotExpected),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.NotExpected)
        ]);
    }

    [Fact]
    public async Task
        GetFinancialDocumentsAsync_should_return_submitted_when_link_exists_even_though_trust_opened_after_financial_year_ended()
    {
        const string uid = "1234";

        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(uid, Arg.Any<FinancialDocumentType>())
            .Returns((
                    financialDocuments:
                    [
                        new TrustDocument(2024, "www.link1.com"),
                        new TrustDocument(2023, "www.link2.com")
                    ],
                    trustOpenDate: new DateOnly(2023, 9, 1))
            );

        var result = await _sut.GetFinancialDocumentsAsync(uid, FinancialDocumentType.ManagementLetter);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.Submitted, "www.link1.com"),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.Submitted, "www.link2.com"),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.NotExpected)
        ]);
    }

    [Fact]
    public async Task
        GetFinancialDocumentsAsync_should_return_not_submitted_when_no_link_for_year_with_closed_submission_window()
    {
        const string uid = "1234";

        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(uid, Arg.Any<FinancialDocumentType>())
            .Returns((
                    financialDocuments:
                    [
                        new TrustDocument(2024, "www.link1.com")
                    ],
                    trustOpenDate: new DateOnly(2023, 8, 31))
            );

        var result = await _sut.GetFinancialDocumentsAsync(uid, FinancialDocumentType.ManagementLetter);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.Submitted, "www.link1.com"),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.NotSubmitted),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.NotExpected)
        ]);
    }


    [Fact]
    public async Task
        GetFinancialDocumentsAsync_should_return_not_yet_submitted_when_no_link_for_year_with_open_submission_window()
    {
        const string uid = "1234";

        _mockTrustDocumentRepository.GetFinancialDocumentsAsync(uid, Arg.Any<FinancialDocumentType>())
            .Returns((
                    financialDocuments:
                    [
                        new TrustDocument(2022, "www.link1.com")
                    ],
                    trustOpenDate: new DateOnly(2015, 9, 1))
            );

        var result = await _sut.GetFinancialDocumentsAsync(uid, FinancialDocumentType.ManagementLetter);

        result.Should().BeEquivalentTo([
            new FinancialDocumentServiceModel(2023, 2024, FinancialDocumentStatus.NotYetSubmitted),
            new FinancialDocumentServiceModel(2022, 2023, FinancialDocumentStatus.NotSubmitted),
            new FinancialDocumentServiceModel(2021, 2022, FinancialDocumentStatus.Submitted, "www.link1.com")
        ]);
    }
}

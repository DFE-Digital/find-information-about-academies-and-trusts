using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class TrustDocumentRepository(IAcademiesDbContext academiesDbContext, ILogger<TrustDocumentRepository> logger)
    : ITrustDocumentRepository
{
    private static readonly Dictionary<FinancialDocumentType, string[]> FolderPrefixes = new()
    {
        { FinancialDocumentType.FinancialStatement, ["AFS", "FS"] },
        { FinancialDocumentType.ManagementLetter, ["AML", "ML"] },
        { FinancialDocumentType.InternalScrutinyReport, ["ISR"] },
        { FinancialDocumentType.SelfAssessmentChecklist, ["SRMSAT", "SRMSAC"] }
    };

    public async Task<(TrustDocument[] financialDocuments, DateOnly trustOpenDate)> GetFinancialDocumentsAsync(
        string uid, FinancialDocumentType financialDocumentType)
    {
        var (trustOpenDate, trustReferenceNumber) = await GetTrustInfoFromGias(uid);

        var allSharepointTrustDocLinksForFinancialDocType = await academiesDbContext.SharepointTrustDocLinks
            .Where(doc => doc.TrustRefNumber == trustReferenceNumber
                          && FolderPrefixes[financialDocumentType].Contains(doc.FolderPrefix))
            .Select(doc => new { doc.FolderYear, DocumentLink = doc.DocumentLink!, doc.CreatedDateTime })
            .ToArrayAsync();

        var yearDuplicationsRemoved = allSharepointTrustDocLinksForFinancialDocType
            .GroupBy(doc => doc.FolderYear)
            .Select(g => g.OrderByDescending(d => d.CreatedDateTime).First());

        var trustDocuments = yearDuplicationsRemoved
            .Select(doc => new TrustDocument(doc.FolderYear, doc.DocumentLink))
            .ToArray();

        return (trustDocuments, trustOpenDate);
    }

    private async Task<(DateOnly trustOpenDate, string trustReferenceNumber)> GetTrustInfoFromGias(string uid)
    {
        var giasGroup = await academiesDbContext.Groups.Where(g => g.GroupUid == uid)
            .Select(g => new { g.GroupId, g.IncorporatedOnOpenDate })
            .SingleAsync();

        if (giasGroup.IncorporatedOnOpenDate is null)
            logger.LogError(
                "Open date was not found for trust {uid}. This should never happen and indicates a data integrity issue with the GIAS data in Academies Db",
                uid);

        var trustOpenDate = giasGroup.IncorporatedOnOpenDate is not null
            ? DateOnly.ParseExact(giasGroup.IncorporatedOnOpenDate, "dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo)
            : DateOnly.MinValue;
        var trustReferenceNumber = giasGroup.GroupId!; //Enforced by EF filter

        return (trustOpenDate, trustReferenceNumber);
    }
}

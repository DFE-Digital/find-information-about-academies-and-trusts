using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class TrustDocumentRepository(IAcademiesDbContext academiesDbContext) : ITrustDocumentRepository
{
    private static readonly Dictionary<FinancialDocumentType, string[]> FolderPrefixes = new()
    {
        { FinancialDocumentType.FinancialStatement, ["AFS", "FS"] },
        { FinancialDocumentType.ManagementLetter, ["AML", "ML"] },
        { FinancialDocumentType.InternalScrutinyReport, ["ISR"] },
        { FinancialDocumentType.SelfAssessmentChecklist, ["SRMSAT", "SRMSAC"] }
    };

    public async Task<TrustDocument[]> GetFinancialDocumentsAsync(string trustReferenceNumber,
        FinancialDocumentType financialDocumentType)
    {
        var sharepointTrustDocLinks = await academiesDbContext.SharepointTrustDocLinks
            .Where(doc => doc.DocumentLink != null
                          && doc.TrustRefNumber == trustReferenceNumber
                          && FolderPrefixes[financialDocumentType].Contains(doc.FolderPrefix))
            .Select(doc => new { doc.FolderYear, DocumentLink = doc.DocumentLink! })
            .ToArrayAsync();

        var trustDocuments = sharepointTrustDocLinks
            .Select(doc => new TrustDocument(doc.FolderYear, doc.DocumentLink))
            .ToArray();

        return trustDocuments;
    }
}

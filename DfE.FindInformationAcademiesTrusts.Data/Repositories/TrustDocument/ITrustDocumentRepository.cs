using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

public interface ITrustDocumentRepository
{
    Task<(TrustDocument[] financialDocuments, DateOnly trustOpenDate)> GetFinancialDocumentsAsync(string uid,
        FinancialDocumentType financialDocumentType);
}

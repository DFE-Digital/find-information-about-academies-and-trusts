using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

public interface ITrustDocumentRepository
{
    Task<TrustDocument[]> GetFinancialDocumentsAsync(string trustReferenceNumber,
        FinancialDocumentType financialDocumentType);
}

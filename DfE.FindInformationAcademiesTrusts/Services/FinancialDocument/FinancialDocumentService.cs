using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

public interface IFinancialDocumentService
{
    Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(FinancialDocumentType financialDocumentType);
}

public class FinancialDocumentService : IFinancialDocumentService
{
    public Task<FinancialDocumentServiceModel[]> GetFinancialDocumentsAsync(FinancialDocumentType financialDocumentType)
    {
        FinancialDocumentServiceModel[] documents =
        [
            new(2022, 2023, FinancialDocumentStatus.NotExpected, null),
            new(2023, 2024, FinancialDocumentStatus.NotYetSubmitted, null),
            new(2024, 2025, FinancialDocumentStatus.Submitted, "https://www.google.com")
        ];

        return Task.FromResult(documents);
    }
}

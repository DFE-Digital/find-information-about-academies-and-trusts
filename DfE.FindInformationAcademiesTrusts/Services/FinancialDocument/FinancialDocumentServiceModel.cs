using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

public record FinancialDocumentServiceModel(
    int YearFrom,
    int YearTo,
    FinancialDocumentStatus Status,
    string? Link = null)
{
    public FinancialDocumentServiceModel(FinancialYear financialYear, FinancialDocumentStatus Status,
        string? Link = null)
        : this(financialYear.Start.Year, financialYear.End.Year, Status, Link)
    {
    }
}

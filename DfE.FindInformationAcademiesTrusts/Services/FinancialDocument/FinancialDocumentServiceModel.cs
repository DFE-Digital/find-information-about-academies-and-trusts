using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.FinancialDocument;

public record FinancialDocumentServiceModel(int YearFrom, int YearTo, FinancialDocumentStatus Status, string? Link);

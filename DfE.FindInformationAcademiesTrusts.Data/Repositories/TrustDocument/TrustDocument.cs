namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

public record TrustDocument(
    DateOnly FinancialYearStart,
    DateOnly FinancialYearEnd,
    string DocumentLink);

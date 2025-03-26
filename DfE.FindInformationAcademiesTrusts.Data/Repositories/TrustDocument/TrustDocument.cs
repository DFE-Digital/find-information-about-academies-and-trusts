namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.TrustDocument;

public record TrustDocument
{
    public TrustDocument(int financialYearEndYear, string link)
    {
        FinancialYear = new FinancialYear(financialYearEndYear);
        Link = link;
    }

    public FinancialYear FinancialYear { get; }
    public string Link { get; }
}

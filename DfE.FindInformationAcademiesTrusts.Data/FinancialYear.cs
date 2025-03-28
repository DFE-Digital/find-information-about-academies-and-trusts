namespace DfE.FindInformationAcademiesTrusts.Data;

public record FinancialYear
{
    public FinancialYear(int year)
    {
        Start = new DateOnly(year - 1, 09, 01);
        End = new DateOnly(year, 08, 31);
    }

    public DateOnly Start { get; }
    public DateOnly End { get; }
}

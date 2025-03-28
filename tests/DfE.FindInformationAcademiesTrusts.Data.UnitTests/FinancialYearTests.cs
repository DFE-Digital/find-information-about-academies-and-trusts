namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class FinancialYearTests
{
    [Theory]
    [InlineData(2025, 2024)]
    [InlineData(2023, 2022)]
    public void Start_should_be_1st_september_year_before_given_year(int endYear, int expectedStartYear)
    {
        var financialYear = new FinancialYear(endYear);
        financialYear.Start.Year.Should().Be(expectedStartYear);
        financialYear.Start.Month.Should().Be(9);
        financialYear.Start.Day.Should().Be(1);
    }

    [Theory]
    [InlineData(2024)]
    [InlineData(2022)]
    public void End_should_be_31st_august_year_before_given_year(int endYear)
    {
        var financialYear = new FinancialYear(endYear);
        financialYear.End.Year.Should().Be(endYear);
        financialYear.End.Month.Should().Be(8);
        financialYear.End.Day.Should().Be(31);
    }
}

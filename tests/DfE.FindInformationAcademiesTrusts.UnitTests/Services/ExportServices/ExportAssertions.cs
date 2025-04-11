using ClosedXML.Excel;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services.ExportServices
{
    internal static class ExportAssertions
    {
        public static void AssertSpreadsheetMatches(this IXLWorksheet worksheet, int startingRow, params object[][] expectedValues)
        {
            for (var rowNumber = 0; rowNumber < expectedValues.Length; rowNumber++)
            {
                for (var columnNumber = 0; columnNumber < expectedValues[rowNumber].Length; columnNumber++)
                {
                    var actualCell = worksheet.Cell(rowNumber + startingRow, columnNumber + 1); //the worksheet is 1-indexed

                    switch (expectedValues[rowNumber][columnNumber])
                    {
                        case DateTime expectedCellValue:
                            actualCell.DataType.Should().Be(XLDataType.DateTime);
                            actualCell.GetValue<DateTime>().Should().Be(expectedCellValue);
                            break;

                        case string expectedCellValue:
                            actualCell.Value.ToString().Should().Be(expectedCellValue);
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(expectedValues));
                    }
                }
            }
        }

        public static string CellValue(this IXLWorksheet worksheet, int rowNumber, int column)
        {
            return worksheet.Cell(rowNumber, (int)column).Value.ToString();
        }
    }
}

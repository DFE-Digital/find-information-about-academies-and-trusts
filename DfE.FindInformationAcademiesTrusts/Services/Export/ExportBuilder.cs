using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    using ClosedXML.Excel;

    public abstract class ExportBuilder
    {
        private readonly XLWorkbook workbook = new();
        private readonly IXLWorksheet worksheet;

        public int CurrentRow { get; set; }

        protected ExportBuilder(string sheetName)
        {
            worksheet = workbook.Worksheets.Add(sheetName);
        }

        internal byte[] Build()
        {
            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        internal ExportBuilder WriteTrustInformation(TrustSummaryServiceModel trustSummary)
        {
            worksheet.Cell(1, 1).Value = trustSummary.Name;
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Value = trustSummary.Type;

            CurrentRow += 3;

            return this;
        }

        internal ExportBuilder WriteHeaders(List<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(CurrentRow, i + 1).Value = headers[i];
            }

            worksheet.Row(CurrentRow).Style.Font.Bold = true;

            CurrentRow += 1;

            return this;
        }

        internal void SetTextCell(int row, int column, string value)
        {
            worksheet.Cell(row, column).SetValue(value);
        }

        internal void SetDateCell(int row, int column, DateTime? dateValue)
        {
            var cell = worksheet.Cell(row, column);
            if (dateValue.HasValue)
            {
                cell.Value = dateValue.Value;
                cell.Style.NumberFormat.SetFormat(StringFormatConstants.DisplayDateFormat);
            }
            else
            {
                cell.Value = string.Empty;
            }
        }

        internal ExportBuilder WriteRows(Action action)
        {
            action.Invoke();

            return this;
        }
    }
}

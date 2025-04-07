using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Services.Export.Builders
{
    using ClosedXML.Excel;
    using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;

    public abstract class ExportBuilder
    {
        private readonly XLWorkbook? workbook;
        private readonly IXLWorksheet worksheet;

        public int CurrentRow { get; private set; }

        protected ExportBuilder(string sheetName)
        {
            this.workbook = new XLWorkbook();
            worksheet = workbook.Worksheets.Add(sheetName);
        }

        public void AddRow(int number = 1)
        {
            CurrentRow += number;
        }

        public byte[] Build()
        {
            worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook?.SaveAs(stream);
            return stream.ToArray();
        }

        public T WriteTrustInformation<T>(TrustSummary? trustSummary) where T : ExportBuilder
        {
            worksheet.Cell(1, 1).Value = trustSummary?.Name ?? string.Empty;
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Value = trustSummary?.Type ?? string.Empty;

            CurrentRow += 3;

            return (T)this;
        }

        public ExportBuilder WriteHeaders(List<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(CurrentRow, i + 1).Value = headers[i];
            }

            worksheet.Row(CurrentRow).Style.Font.Bold = true;

            CurrentRow += 1;

            return this;
        }

        public void SetTextCell(int row, int column, string value)
        {
            worksheet.Cell(row, column).SetValue(value);
        }

        public void SetDateCell(int row, int column, DateTime? dateValue)
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
    }
}
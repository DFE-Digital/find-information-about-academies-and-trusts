﻿using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    using ClosedXML.Excel;

    public abstract class ExportBuilder
    {
        private readonly XLWorkbook workbook = new();

        public IXLWorksheet Worksheet { get; set; }

        public readonly int TrustRows = 3;
        public readonly int HeaderRows = 1;

        public int CurrentRow { get; set; }

        protected ExportBuilder(string sheetName)
        {
            Worksheet = workbook.Worksheets.Add(sheetName);
        }

        public byte[] Build()
        {
            Worksheet.Columns().AdjustToContents();
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }

        public ExportBuilder WriteTrustInformation(TrustSummaryServiceModel trustSummary)
        {
            Worksheet.Cell(1, 1).Value = trustSummary.Name;
            Worksheet.Row(1).Style.Font.Bold = true;
            Worksheet.Cell(2, 1).Value = trustSummary.Type;

            CurrentRow += TrustRows;

            return this;
        }

        public ExportBuilder WriteHeaders(List<string> headers)
        {
            for (var i = 0; i < headers.Count; i++)
            {
                Worksheet.Cell(CurrentRow, i + 1).Value = headers[i];
            }

            Worksheet.Row(CurrentRow).Style.Font.Bold = true;

            CurrentRow += HeaderRows;

            return this;
        }

        internal void SetTextCell(int row, int column, string value)
        {
            Worksheet.Cell(row, column).SetValue(value);
        }

        public void SetDateCell(int row, int column, DateTime? dateValue)
        {
            var cell = Worksheet.Cell(row, column);
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

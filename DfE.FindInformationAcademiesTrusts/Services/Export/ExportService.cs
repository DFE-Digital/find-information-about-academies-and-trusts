using ClosedXML.Excel;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public interface IExportService
    {
        byte[] ExportAcademiesToSpreadsheetUsingProvider(Data.Trust? trust, Trust.TrustSummaryServiceModel? trustSummary);
    }

    public class ExportService : IExportService
    {

        public byte[] ExportAcademiesToSpreadsheetUsingProvider(Data.Trust? trust, Trust.TrustSummaryServiceModel? trustSummary)
        {
            var headers = new List<string>
            {
                "School Name", "URN", "Local Authority", "Type", "Rural or Urban", "Date joined",
                "Previous Ofsted Rating", "Date of Previous Ofsted", "Before/After Joining",
                "Current Ofsted Rating", "Date of Current Ofsted", "Before/After Joining",
                "Phase of Education", "Pupil Numbers", "Capacity", "% Full", "Pupils eligible for Free School Meals"
            };

            var academies = trust?.Academies ?? [];

            var dataExtractor = new Func<Data.Academy, object[]>((academy) =>
            [
                academy.EstablishmentName ?? string.Empty,
                academy.Urn,
                academy.LocalAuthority ?? string.Empty,
                academy.TypeOfEstablishment ?? string.Empty,
                academy.UrbanRural ?? string.Empty,
                academy.DateAcademyJoinedTrust.ToString("yyyy-MM-dd") ?? string.Empty,
                academy.PreviousOfstedRating?.OfstedRatingScore.ToString() ?? string.Empty,
                academy.PreviousOfstedRating?.InspectionEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                academy.PreviousOfstedRating?.InspectionEndDate < academy.DateAcademyJoinedTrust ? "Before Joining" : "After Joining",
                academy.CurrentOfstedRating?.OfstedRatingScore.ToString() ?? string.Empty,
                academy.CurrentOfstedRating?.InspectionEndDate?.ToString("yyyy-MM-dd") ?? string.Empty,
                academy.CurrentOfstedRating?.InspectionEndDate < academy.DateAcademyJoinedTrust ? "Before Joining" : "After Joining",
                academy.PhaseOfEducation ?? string.Empty,
                academy.NumberOfPupils?.ToString() ?? string.Empty,
                academy.SchoolCapacity?.ToString() ?? string.Empty,
                academy.PercentageFull?.ToString() ?? string.Empty,
                academy.PercentageFreeSchoolMeals?.ToString() ?? string.Empty
            ]);

            return GenerateSpreadsheet(trustSummary, academies, headers, dataExtractor);
        }

        private static byte[] GenerateSpreadsheet(Trust.TrustSummaryServiceModel? trustSummary, Data.Academy[]? academies, List<string> headers, Func<Data.Academy, object[]> dataExtractor)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Academies");

            // Adding Trust name and type 
            worksheet.Cell(1, 1).Value = trustSummary?.Name ?? string.Empty;
            worksheet.Row(1).Style.Font.Bold = true;
            worksheet.Cell(2, 1).Value = trustSummary?.Type ?? string.Empty;

            // Adding Excel headers for the data points for academies
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cell(3, i + 1).Value = headers[i];
            }
            worksheet.Row(3).Style.Font.Bold = true;
            // Adding Excel data beneath relevant headers
            for (int i = 0; i < academies?.Length; i++)
            {
                var rowData = dataExtractor(academies[i]);
                for (int j = 0; j < rowData.Length; j++)
                {
                    worksheet.Cell(i + 4, j + 1).SetValue(rowData[j].ToString());
                }
            }
            // Auto-size columns based on content
            worksheet.Columns().AdjustToContents();

            // Save to a memory stream and return as byte array
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

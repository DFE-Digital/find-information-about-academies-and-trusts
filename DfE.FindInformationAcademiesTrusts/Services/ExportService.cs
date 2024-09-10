using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Services
{
    public interface IExportService
    {
        byte[] ExportAcademiesToSpreadsheetUsingProvider(Data.Trust trust, Trust.TrustSummaryServiceModel? trustSummary);
    }

    public class ExportService : IExportService
    {

        public byte[] ExportAcademiesToSpreadsheetUsingProvider(Data.Trust trust, Trust.TrustSummaryServiceModel? trustSummary)
        {
            var headers = new List<string>
            {
                "School Name", "URN", "Local Authority", "Type", "Rural or Urban", "Date joined",
                "Previous Ofsted Rating", "Before/After Joining","Date of Previous Ofsted",
                "Current Ofsted Rating", "Before/After Joining", "Date of Current Ofsted",
                "Phase of Education", "Pupil Numbers", "Capacity", "% Full", "Pupils eligible for Free School Meals"
            };

            var academies = trust.Academies;

            var dataExtractor = new Func<Data.Academy, string[]>((academy) =>
             new string[]
             {
                academy.EstablishmentName ?? string.Empty,
                 academy.Urn.ToString() ?? string.Empty,
                academy.LocalAuthority ?? string.Empty,
                academy.TypeOfEstablishment ?? string.Empty,
                academy.UrbanRural ?? string.Empty,
                 academy.DateAcademyJoinedTrust.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                 academy.PreviousOfstedRating?.OfstedRatingScore.ToString() ?? string.Empty,
                 IsOfstedRatingBeforeOrAfterJoining(academy.PreviousOfstedRating?.OfstedRatingScore ?? OfstedRatingScore.None, academy.DateAcademyJoinedTrust, academy.PreviousOfstedRating?.InspectionEndDate).ToString(),
                 academy.PreviousOfstedRating?.InspectionEndDate?.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                 academy.CurrentOfstedRating?.OfstedRatingScore.ToString() ?? string.Empty,
                 IsOfstedRatingBeforeOrAfterJoining(academy.CurrentOfstedRating?.OfstedRatingScore ?? OfstedRatingScore.None, academy.DateAcademyJoinedTrust, academy.CurrentOfstedRating?.InspectionEndDate).ToString(),
                academy.CurrentOfstedRating?.InspectionEndDate?.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                academy.PhaseOfEducation ?? string.Empty,
                academy.NumberOfPupils?.ToString() ?? string.Empty,
                academy.SchoolCapacity?.ToString() ?? string.Empty,
                academy.PercentageFull.HasValue ? $"{academy.PercentageFull}%" : string.Empty,
                academy.PercentageFreeSchoolMeals.HasValue ? $"{academy.PercentageFreeSchoolMeals}%" : string.Empty
             });


            return GenerateSpreadsheet(trustSummary, academies, headers, dataExtractor);
        }

        private static string IsOfstedRatingBeforeOrAfterJoining(OfstedRatingScore ofstedRatingScore, DateTime dateAcademyJoinedTrust, DateTime? inspectionEndDate)
        {
            if (ofstedRatingScore == OfstedRatingScore.None)
            {
                return string.Empty;
            }

            if (inspectionEndDate < dateAcademyJoinedTrust)
            {
                return "Before Joining";
            }

            return "After Joining";
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

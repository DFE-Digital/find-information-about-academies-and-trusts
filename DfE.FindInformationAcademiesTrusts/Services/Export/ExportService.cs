using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.Services.Export;

public interface IExportService
{
    Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid);
}

public class ExportService(IAcademyRepository academyRepository, ITrustRepository trustRepository) : IExportService
{
    public async Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid)
    {
        var headers = new List<string>
        {
            "School Name", "URN", "Local Authority", "Type", "Rural or Urban", "Date joined",
            "Previous Ofsted Rating", "Before/After Joining", "Date of Previous Ofsted",
            "Current Ofsted Rating", "Before/After Joining", "Date of Current Ofsted",
            "Phase of Education", "Age Range", "Pupil Numbers", "Capacity", "% Full",
            "Pupils eligible for Free School Meals"
        };

        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var academiesDetails = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);
        var academiesOfstedRatings = await academyRepository.GetAcademiesInTrustOfstedAsync(uid);
        var academiesPupilNumbers = await academyRepository.GetAcademiesInTrustPupilNumbersAsync(uid);
        var academiesFreeSchoolMeals = await academyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        return GenerateSpreadsheet(trustSummary, academiesDetails, headers, academiesOfstedRatings,
            academiesPupilNumbers, academiesFreeSchoolMeals);
    }

    public static float CalculatePercentageFull(int? numberOfPupils, int? schoolCapacity)
    {
        if (numberOfPupils.HasValue && schoolCapacity.HasValue && schoolCapacity != 0)
        {
            return (float)Math.Round((double)numberOfPupils.Value / schoolCapacity.Value * 100);
        }

        return 0;
    }

    public static string IsOfstedRatingBeforeOrAfterJoining(OfstedRatingScore ofstedRatingScore,
        DateTime dateAcademyJoinedTrust, DateTime? inspectionEndDate)
    {
        if (ofstedRatingScore == OfstedRatingScore.None || !inspectionEndDate.HasValue)
        {
            return string.Empty;
        }

        return inspectionEndDate < dateAcademyJoinedTrust ? "Before Joining" : "After Joining";
    }

    private static byte[] GenerateSpreadsheet(
        TrustSummary? trustSummary,
        AcademyDetails[] academies,
        List<string> headers,
        AcademyOfsted[] academiesOfstedRatings,
        AcademyPupilNumbers[] academiesPupilNumbers,
        AcademyFreeSchoolMeals[] academiesFreeSchoolMeals)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Academies");

        // Adding Trust name and type 
        worksheet.Cell(1, 1).Value = trustSummary?.Name ?? string.Empty;
        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cell(2, 1).Value = trustSummary?.Type ?? string.Empty;

        // Adding Excel headers for the data points for academies
        for (var i = 0; i < headers.Count; i++)
        {
            worksheet.Cell(3, i + 1).Value = headers[i];
        }

        worksheet.Row(3).Style.Font.Bold = true;

        // Adding Excel data beneath relevant headers
        for (var i = 0; i < academies.Length; i++)
        {
            var academyDetails = academies[i];
            var urn = academyDetails.Urn;

            var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == urn);
            var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.None;
            var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.None;

            var pupilNumbersData = academiesPupilNumbers.SingleOrDefault(x => x.Urn == urn);
            var freeSchoolMealsData = academiesFreeSchoolMeals.SingleOrDefault(x => x.Urn == urn);

            var percentageFull =
                CalculatePercentageFull(pupilNumbersData?.NumberOfPupils, pupilNumbersData?.SchoolCapacity);

            var rowData = new[]
            {
                academyDetails.EstablishmentName ?? string.Empty,
                urn,
                academyDetails.LocalAuthority ?? string.Empty,
                academyDetails.TypeOfEstablishment ?? string.Empty,
                academyDetails.UrbanRural ?? string.Empty,
                ofstedData?.DateAcademyJoinedTrust.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                previousRating.OverallEffectiveness.ToDisplayString() ?? string.Empty,
                IsOfstedRatingBeforeOrAfterJoining(previousRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue, previousRating.InspectionDate),
                previousRating.InspectionDate?.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                currentRating.OverallEffectiveness.ToDisplayString() ?? string.Empty,
                IsOfstedRatingBeforeOrAfterJoining(currentRating.OverallEffectiveness,
                    ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue, currentRating.InspectionDate),
                currentRating.InspectionDate?.ToString(StringFormatConstants.ViewDate) ?? string.Empty,
                pupilNumbersData?.PhaseOfEducation ?? string.Empty,
                pupilNumbersData != null
                    ? $"{pupilNumbersData.AgeRange.Minimum} - {pupilNumbersData.AgeRange.Maximum}"
                    : string.Empty,
                pupilNumbersData?.NumberOfPupils?.ToString() ?? string.Empty,
                pupilNumbersData?.SchoolCapacity?.ToString() ?? string.Empty,
                percentageFull > 0 ? $"{percentageFull}%" : string.Empty,
                freeSchoolMealsData?.PercentageFreeSchoolMeals.HasValue == true
                    ? $"{freeSchoolMealsData.PercentageFreeSchoolMeals}%"
                    : string.Empty
            };

            for (var j = 0; j < rowData.Length; j++)
            {
                worksheet.Cell(i + 4, j + 1).SetValue(rowData[j]);
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

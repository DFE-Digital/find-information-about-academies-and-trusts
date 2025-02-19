using ClosedXML.Excel;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace DfE.FindInformationAcademiesTrusts.Services.Export;

public interface IExportService
{
    Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid);
    Task<byte[]> ExportOfstedDataToSpreadsheetAsync(string uid);
    Task<byte[]> ExportPipelineAcademiesToSpreadsheetAsync(string uid);
}

public class ExportService(
    IAcademyRepository academyRepository,
    ITrustRepository trustRepository,
    IAcademyService academyService) : IExportService
{
    private const string BeforeOrAfterJoiningHeader = "Before/After Joining";

    public async Task<byte[]> ExportAcademiesToSpreadsheetAsync(string uid)
    {
        var headers = new List<string>
        {
            "School Name",
            "URN",
            "Local Authority",
            "Type",
            "Rural or Urban",
            "Date joined",
            "Previous Ofsted Rating",
            BeforeOrAfterJoiningHeader,
            "Date of Previous Ofsted",
            "Current Ofsted Rating",
            BeforeOrAfterJoiningHeader,
            "Date of Current Ofsted",
            "Phase of Education",
            "Age Range",
            "Pupil Numbers",
            "Capacity",
            "% Full",
            "Pupils eligible for Free School Meals"
        };

        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var academiesDetails = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);
        var academiesOfstedRatings = await academyRepository.GetAcademiesInTrustOfstedAsync(uid);
        var academiesPupilNumbers = await academyRepository.GetAcademiesInTrustPupilNumbersAsync(uid);
        var academiesFreeSchoolMeals = await academyRepository.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

        return GenerateAcademiesSpreadsheet(
            trustSummary,
            academiesDetails,
            headers,
            academiesOfstedRatings,
            academiesPupilNumbers,
            academiesFreeSchoolMeals
        );
    }

    public static float CalculatePercentageFull(int? numberOfPupils, int? schoolCapacity)
    {
        if (numberOfPupils.HasValue && schoolCapacity.HasValue && schoolCapacity.Value != 0)
        {
            return (float)Math.Round((double)numberOfPupils.Value / schoolCapacity.Value * 100);
        }

        return 0;
    }

    public static string IsOfstedRatingBeforeOrAfterJoining(OfstedRatingScore ofstedRatingScore,
        DateTime? dateAcademyJoinedTrust, DateTime? inspectionEndDate)
    {
        if (ofstedRatingScore == OfstedRatingScore.NotInspected || !inspectionEndDate.HasValue)
        {
            return string.Empty;
        }

        return inspectionEndDate < dateAcademyJoinedTrust ? "Before Joining" : "After Joining";
    }

    private static byte[] GenerateAcademiesSpreadsheet(
        TrustSummary? trustSummary,
        AcademyDetails[] academies,
        List<string> headers,
        AcademyOfsted[] academiesOfstedRatings,
        AcademyPupilNumbers[] academiesPupilNumbers,
        AcademyFreeSchoolMeals[] academiesFreeSchoolMeals)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Academies");

        WriteTrustInformation(worksheet, trustSummary);
        WriteHeaders(worksheet, headers);

        var startRow = 4;
        for (var i = 0; i < academies.Length; i++)
        {
            var currentRow = startRow + i;
            var academy = academies[i];
            var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == academy.Urn);
            var pupilData = academiesPupilNumbers.SingleOrDefault(x => x.Urn == academy.Urn);
            var freeMealsData = academiesFreeSchoolMeals.SingleOrDefault(x => x.Urn == academy.Urn);

            GenerateAcademyRow(worksheet, currentRow, academy, ofstedData, pupilData, freeMealsData);
        }

        worksheet.Columns().AdjustToContents();
        return SaveWorkbookToByteArray(workbook);
    }

    private static void GenerateAcademyRow(
        IXLWorksheet worksheet,
        int rowNumber,
        AcademyDetails academy,
        AcademyOfsted? ofstedData,
        AcademyPupilNumbers? pupilNumbersData,
        AcademyFreeSchoolMeals? freeSchoolMealsData)
    {
        var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
        var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;
        var percentageFull =
            CalculatePercentageFull(pupilNumbersData?.NumberOfPupils, pupilNumbersData?.SchoolCapacity);

        SetTextCell(worksheet, rowNumber, 1, academy.EstablishmentName ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 2, academy.Urn);
        SetTextCell(worksheet, rowNumber, 3, academy.LocalAuthority ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 4, academy.TypeOfEstablishment ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 5, academy.UrbanRural ?? string.Empty);

        SetDateCell(worksheet, rowNumber, 6, ofstedData?.DateAcademyJoinedTrust);

        SetTextCell(worksheet, rowNumber, 7, previousRating.OverallEffectiveness.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 8,
            IsOfstedRatingBeforeOrAfterJoining(
                previousRating.OverallEffectiveness,
                ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue,
                previousRating.InspectionDate
            )
        );

        SetDateCell(worksheet, rowNumber, 9, previousRating.InspectionDate);

        SetTextCell(worksheet, rowNumber, 10, currentRating.OverallEffectiveness.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 11,
            IsOfstedRatingBeforeOrAfterJoining(
                currentRating.OverallEffectiveness,
                ofstedData?.DateAcademyJoinedTrust ?? DateTime.MinValue,
                currentRating.InspectionDate
            )
        );

        SetDateCell(worksheet, rowNumber, 12, currentRating.InspectionDate);

        SetTextCell(worksheet, rowNumber, 13, pupilNumbersData?.PhaseOfEducation ?? string.Empty);

        SetTextCell(worksheet, rowNumber, 14,
            pupilNumbersData != null
                ? $"{pupilNumbersData.AgeRange.Minimum} - {pupilNumbersData.AgeRange.Maximum}"
                : string.Empty
        );

        SetTextCell(worksheet, rowNumber, 15, pupilNumbersData?.NumberOfPupils?.ToString() ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 16, pupilNumbersData?.SchoolCapacity?.ToString() ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 17, percentageFull > 0 ? $"{percentageFull}%" : string.Empty);
        SetTextCell(
            worksheet,
            rowNumber,
            18,
            freeSchoolMealsData?.PercentageFreeSchoolMeals.HasValue == true
                ? $"{freeSchoolMealsData.PercentageFreeSchoolMeals}%"
                : string.Empty
        );
    }

    public async Task<byte[]> ExportOfstedDataToSpreadsheetAsync(string uid)
    {
        var headers = new List<string>
        {
            "School Name",
            "Date Joined",
            "Current single headline grade",
            BeforeOrAfterJoiningHeader,
            "Date of Current Inspection",
            "Previous single headline grade",
            BeforeOrAfterJoiningHeader,
            "Date of previous inspection",
            "Quality of Education",
            "Behaviour and Attitudes",
            "Personal Development",
            "Leadership and Management",
            "Early Years Provision",
            "Sixth Form Provision",
            "Previous Quality of Education",
            "Previous Behaviour and Attitudes",
            "Previous Personal Development",
            "Previous Leadership and Management",
            "Previous Early Years Provision",
            "Previous Sixth Form Provision",
            "Effective Safeguarding",
            "Category of Concern"
        };

        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var academiesDetails = await academyRepository.GetAcademiesInTrustDetailsAsync(uid);
        var academiesOfstedRatings = await academyRepository.GetAcademiesInTrustOfstedAsync(uid);

        return GenerateOfstedSpreadsheet(trustSummary, academiesDetails, headers, academiesOfstedRatings);
    }

    private static byte[] GenerateOfstedSpreadsheet(
        TrustSummary? trustSummary,
        AcademyDetails[] academies,
        List<string> headers,
        AcademyOfsted[] academiesOfstedRatings)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(ViewConstants.OfstedPageName);

        WriteTrustInformation(worksheet, trustSummary);
        WriteHeaders(worksheet, headers);

        var startRow = 4;
        for (var i = 0; i < academies.Length; i++)
        {
            var currentRow = startRow + i;
            var academy = academies[i];
            var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == academy.Urn);
            GenerateOfstedRow(worksheet, currentRow, academy, ofstedData);
        }

        worksheet.Columns().AdjustToContents();
        return SaveWorkbookToByteArray(workbook);
    }

    private static void GenerateOfstedRow(
        IXLWorksheet worksheet,
        int rowNumber,
        AcademyDetails academy,
        AcademyOfsted? ofstedData)
    {
        var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
        var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;

        // School Name
        SetTextCell(worksheet, rowNumber, 1, academy.EstablishmentName ?? string.Empty);

        // Date Joined Trust
        SetDateCell(worksheet, rowNumber, 2, ofstedData?.DateAcademyJoinedTrust);

        // Current Single Headline Grade
        SetTextCell(worksheet, rowNumber, 3, currentRating.OverallEffectiveness.ToDisplayString(true));

        // Before/After Joining (Current)
        SetTextCell(worksheet, rowNumber, 4,
            IsOfstedRatingBeforeOrAfterJoining(
                currentRating.OverallEffectiveness,
                ofstedData?.DateAcademyJoinedTrust,
                currentRating.InspectionDate
            )
        );

        // Current Inspection Date
        SetDateCell(worksheet, rowNumber, 5, currentRating.InspectionDate);

        // Previous Single Headline Grade
        SetTextCell(worksheet, rowNumber, 6, previousRating.OverallEffectiveness.ToDisplayString(false));

        // Before/After Joining (Previous)
        SetTextCell(worksheet, rowNumber, 7,
            IsOfstedRatingBeforeOrAfterJoining(
                previousRating.OverallEffectiveness,
                ofstedData?.DateAcademyJoinedTrust,
                previousRating.InspectionDate
            )
        );

        // Previous Inspection Date
        SetDateCell(worksheet, rowNumber, 8, previousRating.InspectionDate);

        // Current Ratings
        SetTextCell(worksheet, rowNumber, 9, currentRating.QualityOfEducation.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 10, currentRating.BehaviourAndAttitudes.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 11, currentRating.PersonalDevelopment.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 12,
            currentRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 13, currentRating.EarlyYearsProvision.ToDisplayString(true));
        SetTextCell(worksheet, rowNumber, 14, currentRating.SixthFormProvision.ToDisplayString(true));

        // Previous Ratings
        SetTextCell(worksheet, rowNumber, 15, previousRating.QualityOfEducation.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 16, previousRating.BehaviourAndAttitudes.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 17, previousRating.PersonalDevelopment.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 18,
            previousRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 19, previousRating.EarlyYearsProvision.ToDisplayString(false));
        SetTextCell(worksheet, rowNumber, 20, previousRating.SixthFormProvision.ToDisplayString(false));

        // Safeguarding Effective
        SetTextCell(worksheet, rowNumber, 21, currentRating.SafeguardingIsEffective.ToDisplayString());

        // Category of Concern
        SetTextCell(worksheet, rowNumber, 22, currentRating.CategoryOfConcern.ToDisplayString());
    }

    public async Task<byte[]> ExportPipelineAcademiesToSpreadsheetAsync(string uid)
    {
        var trustSummary = await trustRepository.GetTrustSummaryAsync(uid);
        var trustReferenceNumber = await trustRepository.GetTrustReferenceNumberAsync(uid);
        var preAdvisoryAcademies = await academyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber);
        var postAdvisoryAcademies = await academyService.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber);
        var freeSchools = await academyService.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber);

        return GeneratePipelineAcademiesSpreadsheet(
            trustSummary,
            preAdvisoryAcademies,
            postAdvisoryAcademies,
            freeSchools
        );
    }

    private static byte[] GeneratePipelineAcademiesSpreadsheet(
        TrustSummary? trustSummary,
        AcademyPipelineServiceModel[] preAdvisoryAcademies,
        AcademyPipelineServiceModel[] postAdvisoryAcademies,
        AcademyPipelineServiceModel[] freeSchools)
    {
        const int sectionGapRowCount = 1;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Pipeline Academies");

        var nextRow = WriteTrustInformation(worksheet, trustSummary);

        // Pre-advisory
        nextRow = GeneratePipelineAcademySectionHeading(worksheet, nextRow + sectionGapRowCount,
            "Pre-advisory academies",
            "School Name",
            "URN",
            "Age range",
            "Local authority",
            "Project type",
            "Proposed conversion or transfer date");
        nextRow = GeneratePipelineAcademySection(worksheet, nextRow, preAdvisoryAcademies);

        // Post-advisory
        nextRow = GeneratePipelineAcademySectionHeading(worksheet, nextRow + sectionGapRowCount,
            "Post-advisory academies",
            "School Name",
            "URN",
            "Age range",
            "Local authority",
            "Project type",
            "Proposed conversion or transfer date");
        nextRow = GeneratePipelineAcademySection(worksheet, nextRow, postAdvisoryAcademies);

        // Free schools
        nextRow = GeneratePipelineAcademySectionHeading(worksheet, nextRow + sectionGapRowCount, "Free schools",
            "School Name",
            "URN",
            "Age range",
            "Local authority",
            "Project type",
            "Provisional opening date"
        );
        GeneratePipelineAcademySection(worksheet, nextRow, freeSchools);

        worksheet.Columns().AdjustToContents();
        return SaveWorkbookToByteArray(workbook);
    }

    private static void GeneratePipelineAcademyRow(
        IXLWorksheet worksheet,
        int rowNumber,
        AcademyPipelineServiceModel pipelineAcademy)
    {
        SetTextCell(worksheet, rowNumber, 1, pipelineAcademy.EstablishmentName ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 2, pipelineAcademy.Urn ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 3, pipelineAcademy.AgeRange != null
            ? $"{pipelineAcademy.AgeRange.Minimum} - {pipelineAcademy.AgeRange.Maximum}"
            : "Unconfirmed"
        );
        SetTextCell(worksheet, rowNumber, 4, pipelineAcademy.LocalAuthority ?? string.Empty);
        SetTextCell(worksheet, rowNumber, 5, pipelineAcademy.ProjectType ?? string.Empty);

        if (pipelineAcademy.ChangeDate != null)
        {
            SetDateCell(worksheet, rowNumber, 6, pipelineAcademy.ChangeDate);
        }
        else
        {
            SetTextCell(worksheet, rowNumber, 6, "Unconfirmed");
        }
    }

    /// <summary>
    /// Returns next row number
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="startRow"></param>
    /// <param name="pipelineAcademies"></param>
    private static int GeneratePipelineAcademySection(
        IXLWorksheet worksheet,
        int startRow,
        AcademyPipelineServiceModel[] pipelineAcademies)
    {
        var orderedPipelineAcademies = pipelineAcademies.OrderBy(a => a.EstablishmentName).ToArray();

        for (var i = 0; i < orderedPipelineAcademies.Length; i++)
        {
            var currentRow = startRow + i;
            var pipelineAcademy = orderedPipelineAcademies[i];

            GeneratePipelineAcademyRow(worksheet, currentRow, pipelineAcademy);
        }

        return startRow + pipelineAcademies.Length;
    }

    /// <summary>
    /// Returns next row number
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="row"></param>
    /// <param name="heading"></param>
    /// <param name="headers"></param>
    /// <returns></returns>
    private static int GeneratePipelineAcademySectionHeading(
        IXLWorksheet worksheet,
        int row,
        string heading,
        params string[] headers)
    {
        worksheet.Cell(row, 1).Value = heading;
        worksheet.Row(row).Style.Font.Bold = true;
        WriteHeaders(worksheet, headers.ToList(), row + 1);

        return row + 2;
    }

    private static void SetDateCell(IXLWorksheet worksheet, int row, int column, DateTime? dateValue)
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

    private static void SetTextCell(IXLWorksheet worksheet, int row, int column, string value)
    {
        worksheet.Cell(row, column).SetValue(value);
    }

    /// <summary>
    /// Returns next row number
    /// </summary>
    /// <param name="worksheet"></param>
    /// <param name="trustSummary"></param>
    /// <returns></returns>
    private static int WriteTrustInformation(IXLWorksheet worksheet, TrustSummary? trustSummary)
    {
        worksheet.Cell(1, 1).Value = trustSummary?.Name ?? string.Empty;
        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Cell(2, 1).Value = trustSummary?.Type ?? string.Empty;

        return 3;
    }

    private static void WriteHeaders(IXLWorksheet worksheet, List<string> headers, int headerRow = 3)
    {
        for (var i = 0; i < headers.Count; i++)
        {
            worksheet.Cell(headerRow, i + 1).Value = headers[i];
        }

        worksheet.Row(headerRow).Style.Font.Bold = true;
    }

    private static byte[] SaveWorkbookToByteArray(XLWorkbook workbook)
    {
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}

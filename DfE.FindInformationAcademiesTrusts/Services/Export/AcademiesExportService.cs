using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using static DfE.FindInformationAcademiesTrusts.Services.Export.ExportColumns;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public interface IAcademiesExportService
    {
        Task<byte[]> Build(string uid);
    }

    public class AcademiesExportService(ITrustService trustService, IAcademyService academyService) : ExportBuilder("Academies"), IAcademiesExportService
    {
        private readonly List<string> headers =
        [
            CommonColumnNames.SchoolName,
            CommonColumnNames.Urn,
            CommonColumnNames.LocalAuthority,
            "Type",
            "Rural or Urban",
            CommonColumnNames.DateJoined,
            "Current Ofsted Rating",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
            "Date of Current Ofsted",
            "Previous Ofsted Rating",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
            "Date of Previous Ofsted",
            "Phase of Education",
            CommonColumnNames.AgeRange,
            "Pupil Numbers",
            "Capacity",
            "% Full",
            "Pupils eligible for Free School Meals",
            "LA average pupils eligible for Free School Meals",
            "National average pupils eligible for Free School Meals",
        ];

        public async Task<byte[]> Build(string uid)
        {
            var trustSummary = await trustService.GetTrustSummaryAsync(uid);

            if (trustSummary is null)
            {
                throw new DataIntegrityException($"Trust summary not found for UID {uid}");
            }

            var academiesDetailsTask = academyService.GetAcademiesInTrustDetailsAsync(uid);
            var academiesOfstedRatingsTask =  academyService.GetAcademiesInTrustOfstedAsync(uid);
            var academiesPupilNumbersTask = academyService.GetAcademiesInTrustPupilNumbersAsync(uid);
            var academiesFreeSchoolMealsTask = academyService.GetAcademiesInTrustFreeSchoolMealsAsync(uid);

            await Task.WhenAll(academiesDetailsTask, academiesOfstedRatingsTask, academiesPupilNumbersTask,
                academiesFreeSchoolMealsTask);

            var academiesDetails = await academiesDetailsTask;
            var academiesOfstedRatings = await academiesOfstedRatingsTask;
            var academiesPupilNumbers = await academiesPupilNumbersTask;
            var academiesFreeSchoolMeals = await academiesFreeSchoolMealsTask;

            return WriteTrustInformation(trustSummary)
                .WriteHeaders(headers)
                .WriteRows(() => { WriteRows(academiesDetails, academiesOfstedRatings, academiesPupilNumbers, academiesFreeSchoolMeals); })
                .Build();
        }

        private void WriteRows(AcademyDetailsServiceModel[] academies, AcademyOfstedServiceModel[] academiesOfstedRatings,
            AcademyPupilNumbersServiceModel[] academiesPupilNumbers,
            AcademyFreeSchoolMealsServiceModel[] academiesFreeSchoolMeals)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.Single(x => x.Urn == details.Urn);
                var pupilData = academiesPupilNumbers.Single(x => x.Urn == details.Urn);
                var freeMealsData = academiesFreeSchoolMeals.Single(x => x.Urn == details.Urn);

                GenerateAcademyRow(details, ofstedData, pupilData, freeMealsData);
            }
        }

        private void GenerateAcademyRow(
            AcademyDetailsServiceModel academy,
            AcademyOfstedServiceModel ofstedData,
            AcademyPupilNumbersServiceModel pupilNumbersData,
            AcademyFreeSchoolMealsServiceModel freeSchoolMealsData)
        {
            var previousRating = ofstedData.PreviousOfstedRating;
            var currentRating = ofstedData.CurrentOfstedRating;
            var percentageFull = pupilNumbersData.PercentageFull.GetValueOrDefault(0);

            SetTextCell(CurrentRow, (int)AcademyColumns.SchoolName, academy.EstablishmentName ?? string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.Urn, academy.Urn);
            SetTextCell(CurrentRow, (int)AcademyColumns.LocalAuthority, academy.LocalAuthority ?? string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.Type, academy.TypeOfEstablishment ?? string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.RuralOrUrban, academy.UrbanRural ?? string.Empty);

            SetDateCell(CurrentRow, (int)AcademyColumns.DateJoined, ofstedData.DateAcademyJoinedTrust);

            SetTextCell(CurrentRow, (int)AcademyColumns.CurrentOfstedRating, currentRating.OverallEffectiveness.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)AcademyColumns.CurrentBeforeAfterJoining, ofstedData.WhenDidCurrentInspectionHappen == BeforeOrAfterJoining.NotYetInspected ? string.Empty : ofstedData.WhenDidCurrentInspectionHappen.ToDisplayString());
            SetDateCell(CurrentRow, (int)AcademyColumns.DateOfCurrentInspection, currentRating.InspectionDate);

            SetTextCell(CurrentRow, (int)AcademyColumns.PreviousOfstedRating, previousRating.OverallEffectiveness.ToDisplayString(false));

            SetTextCell(CurrentRow, (int)AcademyColumns.PreviousBeforeAfterJoining, ofstedData.WhenDidPreviousInspectionHappen == BeforeOrAfterJoining.NotYetInspected ? string.Empty : ofstedData.WhenDidPreviousInspectionHappen.ToDisplayString());

            SetDateCell(CurrentRow, (int)AcademyColumns.DateOfPreviousInspection, previousRating.InspectionDate);

            SetTextCell(CurrentRow, (int)AcademyColumns.PhaseOfEducation, pupilNumbersData.PhaseOfEducation ?? string.Empty);

            SetTextCell(CurrentRow, (int)AcademyColumns.AgeRange,
                $"{pupilNumbersData.AgeRange.Minimum} - {pupilNumbersData.AgeRange.Maximum}"
            );

            SetTextCell(CurrentRow, (int)AcademyColumns.PupilNumbers, pupilNumbersData.NumberOfPupils?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.Capacity, pupilNumbersData.SchoolCapacity?.ToString() ?? string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.PercentFull, percentageFull > 0 ? $"{percentageFull}%" : string.Empty);
            SetTextCell(CurrentRow, (int)AcademyColumns.PupilsEligibleFreeSchoolMeals,
                freeSchoolMealsData.PercentageFreeSchoolMeals.HasValue
                    ? $"{freeSchoolMealsData.PercentageFreeSchoolMeals}%"
                    : string.Empty
            );

            SetTextCell(CurrentRow, (int)AcademyColumns.LaPupilsEligibleFreeSchoolMeals,
                freeSchoolMealsData.LaAveragePercentageFreeSchoolMeals > 0
                    ? $"{Math.Round(freeSchoolMealsData.LaAveragePercentageFreeSchoolMeals, 1)}%"
                    : string.Empty
            );
            SetTextCell(CurrentRow, (int)AcademyColumns.NationalPupilsEligibleFreeSchoolMeals,
                freeSchoolMealsData.NationalAveragePercentageFreeSchoolMeals > 0
                    ? $"{Math.Round(freeSchoolMealsData.NationalAveragePercentageFreeSchoolMeals, 1)}%"
                    : string.Empty
            );

            CurrentRow++;
        }
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using static DfE.FindInformationAcademiesTrusts.Services.Export.ExportColumns;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public interface IOfstedDataExportService
    {
        Task<byte[]> Build(string uid);
    }

    public class OfstedDataExportService(IAcademyService academyService, ITrustService trustService) : ExportBuilder("Ofsted"), IOfstedDataExportService
    {
        private readonly List<string> headers =
        [
            CommonColumnNames.SchoolName,
            CommonColumnNames.DateJoined,
            "Current single headline grade",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
            "Date of Current Inspection",
            "Previous single headline grade",
            CommonColumnNames.BeforeOrAfterJoiningHeader,
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
        ];
        
        public async Task<byte[]> Build(string uid)
        {
            var trustSummary = await trustService.GetTrustSummaryAsync(uid);
 
            if (trustSummary is null)
            {
                throw new DataIntegrityException($"Trust summary not found for UID {uid}");
            }

            var academiesDetailsTask = academyService.GetAcademiesInTrustDetailsAsync(uid);
            var academiesOfstedRatingsTask = academyService.GetAcademiesInTrustOfstedAsync(uid);

            await Task.WhenAll(academiesDetailsTask, academiesOfstedRatingsTask);

            var academiesDetails = await academiesDetailsTask;
            var academiesOfstedRatings = await academiesOfstedRatingsTask;

            return WriteTrustInformation(trustSummary)
                .WriteHeaders(headers)
                .WriteRows(() =>  WriteRows(academiesDetails, academiesOfstedRatings))
                .Build();
        }

        private void WriteRows(AcademyDetailsServiceModel[] academies, AcademyOfstedServiceModel[] academiesOfstedRatings)
        {
            foreach (var details in academies)
            {
                var ofstedData = academiesOfstedRatings.SingleOrDefault(x => x.Urn == details.Urn);

                GenerateOfstedRow(details, ofstedData);
            }
        }

        private void GenerateOfstedRow(AcademyDetailsServiceModel academy, AcademyOfstedServiceModel? ofstedData)
        {
            var previousRating = ofstedData?.PreviousOfstedRating ?? OfstedRating.NotInspected;
            var currentRating = ofstedData?.CurrentOfstedRating ?? OfstedRating.NotInspected;

            // School Name
            SetTextCell(CurrentRow, (int)OfstedColumns.SchoolName, academy.EstablishmentName ?? string.Empty);

            // Date Joined Trust
            SetDateCell(CurrentRow, (int)OfstedColumns.DateJoined, ofstedData?.DateAcademyJoinedTrust);

            // Current Single Headline Grade
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentSingleHeadlineGrade, currentRating.OverallEffectiveness.ToDisplayString(true));

            // Before/After Joining (Current)
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentBeforeAfterJoining, ofstedData?.WhenDidCurrentInspectionHappen.ToDisplayString() ?? string.Empty);

            // Current Inspection Date
            SetDateCell(CurrentRow, (int)OfstedColumns.DateOfCurrentInspection, currentRating.InspectionDate);

            // Previous Single Headline Grade
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousSingleHeadlineGrade, previousRating.OverallEffectiveness.ToDisplayString(false));

            // Before/After Joining (Previous)
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousBeforeAfterJoining, ofstedData?.WhenDidPreviousInspectionHappen.ToDisplayString()?? string.Empty);

            // Previous Inspection Date
            SetDateCell(CurrentRow, (int)OfstedColumns.DateOfPreviousInspection, previousRating.InspectionDate);

            // Current Ratings
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentQualityOfEducation, currentRating.QualityOfEducation.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentBehaviourAndAttitudes, currentRating.BehaviourAndAttitudes.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentPersonalDevelopment, currentRating.PersonalDevelopment.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentLeadershipAndManagement, currentRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentEarlyYearsProvision, currentRating.EarlyYearsProvision.ToDisplayString(true));
            SetTextCell(CurrentRow, (int)OfstedColumns.CurrentSixthFormProvision, currentRating.SixthFormProvision.ToDisplayString(true));

            // Previous Ratings
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousQualityOfEducation, previousRating.QualityOfEducation.ToDisplayString(false));
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousBehaviourAndAttitudes, previousRating.BehaviourAndAttitudes.ToDisplayString(false));
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousPersonalDevelopment, previousRating.PersonalDevelopment.ToDisplayString(false));
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousLeadershipAndManagement, previousRating.EffectivenessOfLeadershipAndManagement.ToDisplayString(false));
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousEarlyYearsProvision, previousRating.EarlyYearsProvision.ToDisplayString(false));
            SetTextCell(CurrentRow, (int)OfstedColumns.PreviousSixthFormProvision, previousRating.SixthFormProvision.ToDisplayString(false));

            // Safeguarding Effective
            SetTextCell(CurrentRow, (int)OfstedColumns.EffectiveSafeguarding, currentRating.SafeguardingIsEffective.ToDisplayString());

            // Category of Concern
            SetTextCell(CurrentRow, (int)OfstedColumns.CategoryOfConcern, currentRating.CategoryOfConcern.ToDisplayString());

            CurrentRow++;
        }

    }
}

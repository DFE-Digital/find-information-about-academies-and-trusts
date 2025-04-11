using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using static DfE.FindInformationAcademiesTrusts.Services.Export.ExportColumns;

namespace DfE.FindInformationAcademiesTrusts.Services.Export
{
    public interface IPipelineAcademiesExportService
    {
        Task<byte[]> Build(string uid);
    }

    public class PipelineAcademiesExportService(ITrustService trustService,
        IAcademyService academyService) : ExportBuilder("Pipeline Academies"), IPipelineAcademiesExportService
    {
        public async Task<byte[]> Build(string uid)
        {
            var trustSummary = await trustService.GetTrustSummaryAsync(uid);

            if (trustSummary is null)
            {
                throw new DataIntegrityException($"Trust summary not found for UID {uid}");
            }

            var trustReferenceNumber = await trustService.GetTrustReferenceNumberAsync(uid);

            var preAdvisoryAcademiesTask = academyService.GetAcademiesPipelinePreAdvisoryAsync(trustReferenceNumber);
            var postAdvisoryAcademiesTask = academyService.GetAcademiesPipelinePostAdvisoryAsync(trustReferenceNumber);
            var freeSchoolsTask = academyService.GetAcademiesPipelineFreeSchoolsAsync(trustReferenceNumber);

            await Task.WhenAll(preAdvisoryAcademiesTask, postAdvisoryAcademiesTask, freeSchoolsTask);

            var preAdvisoryAcademies = await preAdvisoryAcademiesTask;
            var postAdvisoryAcademies = await postAdvisoryAcademiesTask;
            var freeSchools = await freeSchoolsTask;

            return WriteTrustInformation(trustSummary)
                .WriteRows(WriteHeadersForPreAdvisory)
                .WriteRows(() => WriteRows(preAdvisoryAcademies))
                .WriteRows(WriteHeadersForPostAdvisory)
                .WriteRows(() => WriteRows(postAdvisoryAcademies))
                .WriteRows(WriteHeadersForFreeSchools)
                .WriteRows(() => WriteRows(freeSchools))
                .Build();
        }


        private void WriteHeadersForPreAdvisory()
        {
            CurrentRow++;
            WriteHeaders(["Pre-advisory academies"]);
            WriteHeaders([CommonColumnNames.SchoolName, CommonColumnNames.Urn, CommonColumnNames.AgeRange, CommonColumnNames.LocalAuthority, "Project type", "Proposed conversion or transfer date"]);
        }

        private void WriteHeadersForPostAdvisory()
        {
            CurrentRow++;
            WriteHeaders(["Post-advisory academies"]);
            WriteHeaders([CommonColumnNames.SchoolName, CommonColumnNames.Urn, CommonColumnNames.AgeRange, CommonColumnNames.LocalAuthority, "Project type", "Proposed conversion or transfer date"]);
        }

        private void WriteHeadersForFreeSchools()
        {
            CurrentRow++;
            WriteHeaders(["Free schools"]);
            WriteHeaders([CommonColumnNames.SchoolName, CommonColumnNames.Urn, CommonColumnNames.AgeRange, CommonColumnNames.LocalAuthority, "Project type", "Provisional opening date"]);
        }

        private void WriteRows(AcademyPipelineServiceModel[] pipelineAcademies)
        {
            var orderedPipelineAcademies = pipelineAcademies.OrderBy(a => a.EstablishmentName).ToArray();

            foreach (var details in orderedPipelineAcademies)
            {
                GeneratePipelineAcademyRow(details);
            }
        }

        private void GeneratePipelineAcademyRow(AcademyPipelineServiceModel pipelineAcademy)
        {
            SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.SchoolName, pipelineAcademy.EstablishmentName ?? string.Empty);
            SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.Urn, pipelineAcademy.Urn ?? string.Empty);
            SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.AgeRange, pipelineAcademy.AgeRange != null
                ? $"{pipelineAcademy.AgeRange.Minimum} - {pipelineAcademy.AgeRange.Maximum}"
                : "Unconfirmed"
            );
            SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.LocalAuthority, pipelineAcademy.LocalAuthority ?? string.Empty);
            SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.ProjectType, pipelineAcademy.ProjectType ?? string.Empty);

            if (pipelineAcademy.ChangeDate != null)
            {
                SetDateCell(CurrentRow, (int)PipelineAcademiesColumns.ChangeDate, pipelineAcademy.ChangeDate);
            }
            else
            {
                SetTextCell(CurrentRow, (int)PipelineAcademiesColumns.ChangeDate, "Unconfirmed");
            }

            CurrentRow++;
        }
    }
}

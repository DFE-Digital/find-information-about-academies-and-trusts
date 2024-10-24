using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Export;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies
{
    public abstract class AcademiesPageModel(
        IDataSourceService dataSourceService,
        ITrustService trustService,
        IExportService exportService,
        ILogger<AcademiesPageModel> logger,
        IDateTimeProvider dateTimeProvider
    ) : TrustsAreaModel(dataSourceService, trustService, logger, "Academies in this trust")
    {
        protected IExportService ExportService { get; } = exportService;
        public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;
        public string? TabName { get; init; }
        public virtual async Task<IActionResult> OnGetExportAsync(string uid)
        {
            TrustSummaryServiceModel? trustSummary = await TrustService.GetTrustSummaryAsync(uid);

            if (trustSummary == null)
            {
                return new NotFoundResult();
            }

            // Sanitize the trust name to remove any illegal characters
            string sanitizedTrustName = string.Concat(trustSummary.Name.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

            var fileContents = await ExportService.ExportAcademiesToSpreadsheetAsync(uid);
            string fileName = $"{sanitizedTrustName}-{DateTimeProvider.Now:yyyy-MM-dd}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContents, contentType, fileName);
        }


    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies
{
    public abstract class AcademiesPageModel(
        ITrustProvider trustProvider,
        IDataSourceService dataSourceService,
        ITrustService trustService,
        IExportService exportService,
        ILogger<AcademiesPageModel> logger,
        IDateTimeProvider dateTimeProvider
    ) : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Academies in this trust")
    {
        protected IExportService ExportService { get; } = exportService;
        public IDateTimeProvider DateTimeProvider { get; } = dateTimeProvider;
        public string? TabName { get; init; }
        public virtual async Task<IActionResult> OnGetExportAsync(string uid)
        {
            TrustSummaryServiceModel? trustSummary = await TrustService.GetTrustSummaryAsync(uid);
            Trust? allAcademiesDetails = await TrustProvider.GetTrustByUidAsync(uid);

            if (trustSummary == null || allAcademiesDetails == null)
            {
                return new NotFoundResult();
            }

            // Sanitize the trust name to remove any illegal characters
            string sanitizedTrustName = string.Concat(allAcademiesDetails.Name.Where(c => !Path.GetInvalidFileNameChars().Contains(c)));

            var fileContents = ExportService.ExportAcademiesToSpreadsheetUsingProvider(allAcademiesDetails, trustSummary);
            string fileName = $"{sanitizedTrustName}-{DateTimeProvider.Now:yyyy-MM-dd}.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            return File(fileContents, contentType, fileName);
        }


    }
}

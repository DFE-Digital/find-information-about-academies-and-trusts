using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;
public class GovernanceModel(
    IDataSourceService dataSourceService,
    ILogger<GovernanceModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(dataSourceService, trustService, logger, "Governance")
{
    public TrustGovernanceServiceModel TrustGovernance { get; private set; } = default!;
    public decimal TurnoverRate { get; private set; }

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();
        if (pageResult is NotFoundResult) return pageResult;

        TrustGovernance = await TrustService.GetTrustGovernanceAsync(Uid);

        DataSources.Add(new DataSourceListEntry(
            await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Governance" }));

        CalculateTurnoverRate();

        return pageResult;
    }

    public void CalculateTurnoverRate()
    {
        var today = DateTime.Today;

        // Past 12 Months 
        var past12MonthsStart = today.AddYears(-1);
        var past12MonthsEnd = today;

        // Get all governors (Trustees and Members)
        var allGovernors = TrustGovernance.Trustees
            .Concat(TrustGovernance.Members)
            .ToList();

        // Total number of governor positions 
        int totalCurrentGovernors = allGovernors.Count;

        // Appointments in the past 12 months
        int appointmentsInPast12Months = allGovernors
            .Count(g => g.DateOfAppointment != null &&
                        g.DateOfAppointment >= past12MonthsStart &&
                        g.DateOfAppointment <= past12MonthsEnd);

        // Resignations in the past 12 months
        int resignationsInPast12Months = allGovernors
            .Count(g => g.DateOfTermEnd != null &&
                        g.DateOfTermEnd >= past12MonthsStart &&
                        g.DateOfTermEnd <= past12MonthsEnd);

        int totalEvents = appointmentsInPast12Months + resignationsInPast12Months;

        // Calculate turnover rate and round to 1 decimal point
        TurnoverRate = totalCurrentGovernors > 0
            ? Math.Round((decimal)totalEvents / totalCurrentGovernors * 100m, 1)
            : 0m;
    }

}

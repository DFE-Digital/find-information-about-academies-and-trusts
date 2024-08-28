using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Extensions;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public class GovernanceModel(
    ITrustProvider trustProvider,
    IDataSourceService dataSourceService,
    ILogger<GovernanceModel> logger,
    ITrustService trustService)
    : TrustsAreaModel(trustProvider, dataSourceService, trustService, logger, "Governance")
{
    public TrustGovernanceServiceModel TrustGovernance { get; set; } = default!;
    public TableModel TrusteeTable { get; set; } = default!;
    public TableModel HistoricMembersTable { get; set; } = default!;
    public TableModel TrustLeadershipTable { get; set; } = default!;
    public TableModel MembersTable { get; set; } = default!;

    public override async Task<IActionResult> OnGetAsync()
    {
        var pageResult = await base.OnGetAsync();

        if (pageResult.GetType() == typeof(NotFoundResult)) return pageResult;

        TrustGovernance = await TrustService.GetTrustGoverenaceAsync(Uid);
        SetupTables();
        DataSources.Add(new DataSourceListEntry(await DataSourceService.GetAsync(Source.Gias),
            new List<string> { "Governance" }));

        return pageResult;
    }

    private void SetupTables()
    {
        SetupTrustLeadershipTable();
        SetupTrusteeTable();
        SetupMembersTable();
        SetupHistoricMembersTable();
    }

    private void SetupTrustLeadershipTable()
    {
        ColumnValue[] columns =
        [
            new ColumnValue("Name", "trust-leadership-name"),
            new ColumnValue("Role", "trust-leadership-role"),
            new ColumnValue("From", "trust-leadership-from-date", true),
            new ColumnValue("To", "trust-leadership-to-date", true)
        ];
        var rows = TrustGovernance.TrustLeadership.Select(governor =>
                new[]
                {
                    new RowValue(governor.FullName),
                    new RowValue(governor.Role),
                    new RowValue(governor.DateOfAppointment.ShowDateStringOrReplaceWithText(),
                        governor.DateOfAppointment?.ToString(StringFormatConstants.SortDate)),
                    new RowValue(governor.DateOfTermEnd.ShowDateStringOrReplaceWithText(),
                        governor.DateOfTermEnd?.ToString(StringFormatConstants.SortDate))
                })
            .ToArray();
        TrustLeadershipTable = new TableModel("Trust Leadership", rows, columns);
    }

    private void SetupTrusteeTable()
    {
        ColumnValue[] columns =
        [
            new ColumnValue("Name", "trustee-name"),
            new ColumnValue("Appointed by", "trustee-appointed-by"),
            new ColumnValue("From", "trustee-from-date", true),
            new ColumnValue("To", "trustee-to-date", true)
        ];
        var rows = TrustGovernance.Trustees.Select(governor =>
                new[]
                {
                    new RowValue(governor.FullName),
                    new RowValue(governor.AppointingBody),
                    new RowValue(governor.DateOfAppointment.ShowDateStringOrReplaceWithText(),
                        governor.DateOfAppointment?.ToString(StringFormatConstants.SortDate)),
                    new RowValue(governor.DateOfTermEnd.ShowDateStringOrReplaceWithText(),
                        governor.DateOfTermEnd?.ToString(StringFormatConstants.SortDate))
                })
            .ToArray();
        TrusteeTable = new TableModel("Trustees", rows, columns);
    }

    private void SetupMembersTable()
    {
        ColumnValue[] columns =
        [
            new ColumnValue("Name", "trustee-name"),
            new ColumnValue("Appointed by", "trustee-appointed-by"),
            new ColumnValue("From", "trustee-from-date", true),
            new ColumnValue("To", "trustee-to-date", true)
        ];
        var rows = TrustGovernance.Members.Select(governor =>
                new[]
                {
                    new RowValue(governor.FullName),
                    new RowValue(governor.AppointingBody),
                    new RowValue(governor.DateOfAppointment.ShowDateStringOrReplaceWithText(),
                        governor.DateOfAppointment?.ToString(StringFormatConstants.SortDate)),
                    new RowValue(governor.DateOfTermEnd.ShowDateStringOrReplaceWithText(),
                        governor.DateOfTermEnd?.ToString(StringFormatConstants.SortDate))
                })
            .ToArray();
        MembersTable = new TableModel("Members", rows, columns);
    }

    private void SetupHistoricMembersTable()
    {
        ColumnValue[] columns =
        [
            new ColumnValue("Name", "trustee-name"),
            new ColumnValue("Role", "trustee-appointed-by"),
            new ColumnValue("Appointed by", "trustee-appointed-by"),
            new ColumnValue("From", "trustee-from-date", true),
            new ColumnValue("To", "trustee-to-date", true)
        ];
        var rows = TrustGovernance.HistoricMembers.Select(governor =>
                new[]
                {
                    new RowValue(governor.FullName),
                    new RowValue(governor.Role),
                    new RowValue(governor.AppointingBody),
                    new RowValue(governor.DateOfAppointment.ShowDateStringOrReplaceWithText(),
                        governor.DateOfAppointment?.ToString(StringFormatConstants.SortDate)),
                    new RowValue(governor.DateOfTermEnd.ShowDateStringOrReplaceWithText(),
                        governor.DateOfTermEnd?.ToString(StringFormatConstants.SortDate))
                })
            .ToArray();
        HistoricMembersTable = new TableModel("Historic Members", rows, columns);
    }
}

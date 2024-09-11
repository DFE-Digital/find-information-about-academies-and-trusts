using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data;

public record Governor(
    string GID,
    string UID,
    string FullName,
    GovernanceRole Role,
    string AppointingBody,
    DateTime? DateOfAppointment,
    DateTime? DateOfTermEnd,
    string? Email) : Person(FullName, Email)
{
    public bool IsCurrentGovernor => DateOfTermEnd == null || DateOfTermEnd >= DateTime.Today;

    public bool HasRoleLeadership =>
        HasRoleAccountingOfficer || HasRoleChiefFinancialOfficer ||
        HasRoleChairOfTrustees;

    public bool HasRoleMemeber => Role == GovernanceRole.Member;

    public bool HasRoleTrustee => Role == GovernanceRole.Trustee;

    private bool HasRoleAccountingOfficer => Role == GovernanceRole.AccountingOfficer;

    private bool HasRoleChiefFinancialOfficer => Role == GovernanceRole.ChiefFinancialOfficer;

    private bool HasRoleChairOfTrustees => Role == GovernanceRole.ChairOfTrustees;
}

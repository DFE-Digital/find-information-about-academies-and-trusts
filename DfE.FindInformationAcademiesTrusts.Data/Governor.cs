namespace DfE.FindInformationAcademiesTrusts.Data;

public record Governor(
    string GID,
    string UID,
    string FullName,
    string Role,
    string AppointingBody,
    DateTime? DateOfAppointment,
    DateTime? DateOfTermEnd,
    string? Email) : Person(FullName, Email)
{
    public bool IsCurrentGovernor => DateOfTermEnd == null || DateOfTermEnd >= DateTime.Today;

    public bool HasRoleLeadership =>
        HasRoleAccountingOfficer || HasRoleChiefFinancialOfficer ||
        HasRoleChairOfTrustees;

    public bool HasRoleMember => Role == "Member";

    public bool HasRoleTrustee => Role == "Trustee";

    private bool HasRoleAccountingOfficer => Role == "Accounting Officer";

    private bool HasRoleChiefFinancialOfficer => Role == "Chief Financial Officer";

    private bool HasRoleChairOfTrustees => Role == "Chair of Trustees";
}

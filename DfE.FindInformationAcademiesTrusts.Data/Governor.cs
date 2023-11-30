namespace DfE.FindInformationAcademiesTrusts.Data;

public record Governor(
    string GID,
    string UID,
    string? FullName,
    string? Role,
    string? AppointingBody,
    DateTime? DateOfAppointment,
    DateTime? DateOfTermEnd,
    string? Email) : Person(FullName, Email)
{
    public bool IsCurrentGovernor => DateOfTermEnd == null || DateOfTermEnd >= DateTime.Today;
}

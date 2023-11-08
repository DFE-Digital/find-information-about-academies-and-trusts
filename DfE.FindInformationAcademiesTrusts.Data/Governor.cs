namespace DfE.FindInformationAcademiesTrusts.Data;

public record Governor(
    string GID,
    string UID,
    string? FullName,
    string? Role,
    string? AppointingBody,
    DateTime? DateOfAppointment,
    string? Email) : Person(FullName, Email);

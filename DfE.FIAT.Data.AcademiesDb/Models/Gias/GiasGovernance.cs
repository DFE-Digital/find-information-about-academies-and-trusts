using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Models.Gias;

[ExcludeFromCodeCoverage] // Database model POCO
public class GiasGovernance
{
    public string? Gid { get; set; }

    public string? Urn { get; set; }

    public string? Uid { get; set; }

    public string? CompaniesHouseNumber { get; set; }

    public string? Role { get; set; }

    public string? Title { get; set; }

    public string? Forename1 { get; set; }

    public string? Forename2 { get; set; }

    public string? Surname { get; set; }

    public string? DateOfAppointment { get; set; }

    public string? DateTermOfOfficeEndsEnded { get; set; }

    public string? AppointingBody { get; set; }
}

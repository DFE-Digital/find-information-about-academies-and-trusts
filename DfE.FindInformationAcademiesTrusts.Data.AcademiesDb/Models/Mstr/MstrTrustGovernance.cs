using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrTrustGovernance
{
    public long Sk { get; set; }

    public long? FkTrust { get; set; }

    public long? FkGovernanceRoleType { get; set; }

    public string Gid { get; set; } = null!;

    public string? Title { get; set; }

    public string? Forename1 { get; set; }

    public string? Forename2 { get; set; }

    public string? Surname { get; set; }

    public string? Email { get; set; }

    public string? DateOfAppointment { get; set; }

    public string? DateTermOfOfficeEndsEnded { get; set; }

    public string? AppointingBody { get; set; }

    public DateTime? Modified { get; set; }

    public string? ModifiedBy { get; set; }
}

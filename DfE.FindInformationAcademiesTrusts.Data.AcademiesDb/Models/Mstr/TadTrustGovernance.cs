using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class TadTrustGovernance
{
    public string? Gid { get; set; }

    public string? Email { get; set; }
}

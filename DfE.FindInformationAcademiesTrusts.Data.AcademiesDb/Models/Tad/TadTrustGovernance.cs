using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;

[ExcludeFromCodeCoverage] // Database model POCO
public class TadTrustGovernance
{
    public string? Gid { get; set; }

    public string? Email { get; set; }
}

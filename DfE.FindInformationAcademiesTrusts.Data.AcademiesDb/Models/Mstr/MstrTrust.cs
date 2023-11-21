using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrTrust
{
    public string? GORregion { get; set; }
    public required string GroupUid { get; set; }
}

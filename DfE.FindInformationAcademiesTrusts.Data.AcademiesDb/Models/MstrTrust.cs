using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrTrust
{
    public string? GORregion { get; set; }
    public string GroupUid { get; set; }
}

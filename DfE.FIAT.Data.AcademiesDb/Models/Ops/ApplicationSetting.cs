using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Models.Ops;

[ExcludeFromCodeCoverage] // Database model POCO
public class ApplicationSetting
{
    public required string Key { get; set; }
    public string? Value { get; set; }
    public DateTime? Created { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? Modified { get; set; }
    public string? ModifiedBy { get; set; }
}

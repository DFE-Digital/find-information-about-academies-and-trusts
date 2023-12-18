using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;

[ExcludeFromCodeCoverage] // Database model POCO
public class ApplicationEvent
{
    public int Id { get; set; }
    public DateTime? DateTime { get; set; }
    public string? Source { get; set; }
    public string? UserName { get; set; }
    public char? EventType { get; set; }
    public int? Level { get; set; }
    public int? Code { get; set; }
    public char? Severity { get; set; }
    public string? Description { get; set; }
    public string? Message { get; set; }
    public string? Trace { get; set; }
    public int? ProcessID { get; set; }
    public int? LineNumber { get; set; }
}

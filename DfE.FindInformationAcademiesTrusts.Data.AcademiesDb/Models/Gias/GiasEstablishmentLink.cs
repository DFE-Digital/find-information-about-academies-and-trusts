using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

[ExcludeFromCodeCoverage] // Database model POCO

public class GiasEstablishmentLink
{
    public string? Urn { get; set; }
    
    public string? LinkUrn { get; set; }
    
    public string? LinkName { get; set; }
    
    public string? LinkType { get; set; }
    
    public string? LinkEstablishedDate { get; set; }
}
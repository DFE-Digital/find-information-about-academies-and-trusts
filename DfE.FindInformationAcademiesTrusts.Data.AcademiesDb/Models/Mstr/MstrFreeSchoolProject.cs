using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrFreeSchoolProject
{
    public int SK { get; set; }

    public string? ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public string? ProjectApplicationType { get; set; }
    public string? LocalAuthority { get; set; }
    public required string Stage { get; set; }
    public required string RouteOfProject { get; set; }
    public int? StatutoryLowestAge { get; set; }
    public int? StatutoryHighestAge { get; set; }
    public int? NewURN { get; set; }
    public string? EstablishmentName { get; set; }
    public DateTime? ActualDateOpened { get; set; }
    public string? TrustID { get; set; }
    public required string DateSource { get; set; }
    public DateTime? LastDataRefresh { get; set; }
}
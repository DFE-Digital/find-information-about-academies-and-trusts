using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;

[ExcludeFromCodeCoverage] // Database model POCO
public class MstrAcademyConversion : IInComplete, IInPrepare
{
    public int SK { get; set; }
    public string? ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public int? URN { get; set; }
    public int? StatutoryLowestAge { get; set; }
    public int? StatutoryHighestAge { get; set; }
    public string? LocalAuthority { get; set; }
    public string? ProjectApplicationType { get; set; }
    public string? ProjectStatus { get; set; }
    public string? RouteOfProject { get; set; }
    public string? DaoProgress { get; set; }
    public string? EstablishmentName { get; set; }
    public DateTime? ExpectedOpeningDate { get; set; }
    public string? TrustID { get; set; }
    public DateTime? LastDataRefresh { get; set; }
    public bool? InPrepare { get; set; }
    public bool? InComplete { get; set; }
}

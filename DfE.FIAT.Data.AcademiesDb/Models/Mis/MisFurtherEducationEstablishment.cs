using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Models.Mis;

[ExcludeFromCodeCoverage] // Database model POCO
public class MisFurtherEducationEstablishment
{
    public int ProviderUrn { get; set; }

    public int? ProviderUkprn { get; set; }

    public string? ProviderName { get; set; }

    public string? ProviderType { get; set; }

    public string? ProviderGroup { get; set; }

    public string? LocalAuthority { get; set; }

    public string? Region { get; set; }

    public string? OfstedRegion { get; set; }

    public string? DateOfLatestShortInspection { get; set; }

    public string? NumberOfShortInspectionsSinceLastFullInspectionRaw { get; set; }

    public int? NumberOfShortInspectionsSinceLastFullInspection { get; set; }

    public string? InspectionNumber { get; set; }

    public string? InspectionType { get; set; }

    public string? FirstDayOfInspection { get; set; }

    public string? LastDayOfInspection { get; set; }

    public string? DatePublished { get; set; }

    public string? OverallEffectivenessRaw { get; set; }

    public int? OverallEffectiveness { get; set; }

    public string? QualityOfEducationRaw { get; set; }

    public int? QualityOfEducation { get; set; }

    public string? BehaviourAndAttitudesRaw { get; set; }

    public int? BehaviourAndAttitudes { get; set; }

    public string? PersonalDevelopmentRaw { get; set; }

    public int? PersonalDevelopment { get; set; }

    public string? EffectivenessOfLeadershipAndManagementRaw { get; set; }

    public int? EffectivenessOfLeadershipAndManagement { get; set; }

    public string? IsSafeguardingEffective { get; set; }

    public string? PreviousInspectionNumber { get; set; }

    public string? PreviousLastDayOfInspection { get; set; }

    public string? PreviousOverallEffectivenessRaw { get; set; }

    public int? PreviousOverallEffectiveness { get; set; }

    public string? PreviousQualityOfEducationRaw { get; set; }

    public int? PreviousQualityOfEducation { get; set; }

    public string? PreviousBehaviourAndAttitudesRaw { get; set; }

    public int? PreviousBehaviourAndAttitudes { get; set; }

    public string? PreviousPersonalDevelopmentRaw { get; set; }

    public int? PreviousPersonalDevelopment { get; set; }

    public string? PreviousEffectivenessOfLeadershipAndManagementRaw { get; set; }

    public int? PreviousEffectivenessOfLeadershipAndManagement { get; set; }

    public string? PreviousSafeguarding { get; set; }

    public string? ImprovedDeclinedStayedTheSame { get; set; }
}

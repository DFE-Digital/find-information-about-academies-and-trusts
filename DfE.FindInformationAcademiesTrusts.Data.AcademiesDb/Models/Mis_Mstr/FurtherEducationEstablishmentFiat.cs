namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;

public class FurtherEducationEstablishmentFiat
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
    public int? NumberOfShortInspectionsSinceLastFullInspection { get; set; }
    public string? InspectionNumber { get; set; }
    public string? InspectionType { get; set; }
    public string? FirstDayOfInspection { get; set; }
    public string? LastDayOfInspection { get; set; }
    public string? DatePublished { get; set; }
    public string? OverallEffectiveness { get; set; }
    public int? QualityOfEducation { get; set; }
    public int? BehaviourAndAttitudes { get; set; }
    public int? PersonalDevelopment { get; set; }
    public int? EffectivenessOfLeadershipAndManagement { get; set; }
    public string? IsSafeguardingEffective { get; set; }
    public string? PreviousInspectionNumber { get; set; }
    public string? PreviousLastDayOfInspection { get; set; }
    public string? PreviousOverallEffectiveness { get; set; }
    public int? PreviousQualityOfEducation { get; set; }
    public int? PreviousBehaviourAndAttitudes { get; set; }
    public int? PreviousPersonalDevelopment { get; set; }
    public int? PreviousEffectivenessOfLeadershipAndManagement { get; set; }
    public string? PreviousSafeguarding { get; set; }
    public string? ImprovedDeclinedStayedTheSame { get; set; }
    public DateTime? MetaIngestionDatetime { get; set; }
    public string? MetaSourceFilename { get; set; }
}

using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class MISEstablishmentResponse
{
    public string? SiteName { get; set; }
    public string? WebLink { get; set; }
    public string? LAESTAB { get; set; }
    public string? SchoolName { get; set; }
    public string? OfstedPhase { get; set; }
    public string? TypeOfEducation { get; set; }
    public string? SchoolOpenDate { get; set; }
    public string? SixthForm { get; set; }
    public string? DesignatedReligiousCharacter { get; set; }
    public string? ReligiousEthos { get; set; }
    public string? FaithGrouping { get; set; }
    public string? OfstedRegion { get; set; }
    public string? Region { get; set; }
    public string? LocalAuthority { get; set; }
    public string? ParliamentaryConstituency { get; set; }
    public string? Postcode { get; set; }
    public string? IncomeDeprivationAffectingChildrenIndexQuintile { get; set; }
    public string? TotalNumberOfPupils { get; set; }
    public string? LatestSection8InspectionNumberSinceLastFullInspection { get; set; }
    public string? Section8InspectionRelatedToCurrentSchoolUrn { get; set; }
    public string? UrnAtTimeOfSection8Inspection { get; set; }
    public string? SchoolNameAtTimeOfSection8Inspection { get; set; }
    public string? SchoolTypeAtTimeOfSection8Inspection { get; set; }
    public string? NumberOfSection8InspectionsSinceLastFullInspection { get; set; }
    public string? DateOfLatestSection8Inspection { get; set; }
    public string? Section8InspectionPublicationDate { get; set; }
    public string? LatestSection8InspectionConvertedToFullInspection { get; set; }
    public string? Section8InspectionOverallOutcome { get; set; }
    public string? InspectionNumberOfLatestFullInspection { get; set; }
    public string? InspectionType { get; set; }
    public string? InspectionTypeGrouping { get; set; }
    public string? InspectionStartDate { get; set; }
    public string? InspectionEndDate { get; set; }
    public string? PublicationDate { get; set; }
    public string? LatestFullInspectionRelatesToCurrentSchoolUrn { get; set; }
    public string? SchoolUrnAtTimeOfLastFullInspection { get; set; }
    public string? LAESTABAtTimeOfLastFullInspection { get; set; }
    public string? SchoolNameAtTimeOfLastFullInspection { get; set; }
    public string? SchoolTypeAtTimeOfLastFullInspection { get; set; }
    public string? OverallEffectiveness { get; set; }
    public string? CategoryOfConcern { get; set; }
    public string? QualityOfEducation { get; set; }
    public string? BehaviourAndAttitudes { get; set; }
    public string? PersonalDevelopment { get; set; }
    public string? EffectivenessOfLeadershipAndManagement { get; set; }
    public string? SafeguardingIsEffective { get; set; }
    public string? EarlyYearsProvision { get; set; }
    public string? SixthFormProvision { get; set; }
    public string? PreviousFullInspectionNumber { get; set; }
    public string? PreviousInspectionStartDate { get; set; }
    public string? PreviousInspectionEndDate { get; set; }
    public string? PreviousPublicationDate { get; set; }
    public string? PreviousFullInspectionRelatesToUrnOfCurrentSchool { get; set; }
    public string? UrnAtTheTimeOfPreviousFullInspection { get; set; }
    public string? LAESTABAtTheTimeOfPreviousFullInspection { get; set; }
    public string? SchoolNameAtTheTimeOfPreviousFullInspection { get; set; }
    public string? SchoolTypeAtTheTimeOfPreviousFullInspection { get; set; }
    public string? PreviousFullInspectionOverallEffectiveness { get; set; }
    public string? PreviousCategoryOfConcern { get; set; }
    public string? PreviousQualityOfEducation { get; set; }
    public string? PreviousBehaviourAndAttitudes { get; set; }
    public string? PreviousPersonalDevelopment { get; set; }
    public string? PreviousEffectivenessOfLeadershipAndManagement { get; set; }
    public string? PreviousIsSafeguardingEffective { get; set; }
    public string? PreviousEarlyYearsProvision { get; set; }
    public string? PreviousSixthFormProvision { get; set; }
}

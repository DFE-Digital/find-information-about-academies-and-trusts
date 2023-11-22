﻿using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

[ExcludeFromCodeCoverage] // Database model POCO
public class MisEstablishment
{
    public string? WebLink { get; set; }

    public int? Urn { get; set; }

    public int? Laestab { get; set; }

    public string? SchoolName { get; set; }

    public string? OfstedPhase { get; set; }

    public string? TypeOfEducation { get; set; }

    public string? SchoolOpenDate { get; set; }

    public string? AdmissionsPolicy { get; set; }

    public string? SixthForm { get; set; }

    public string? DesignatedReligiousCharacter { get; set; }

    public string? ReligiousEthos { get; set; }

    public string? FaithGrouping { get; set; }

    public string? OfstedRegion { get; set; }

    public string? Region { get; set; }

    public string? LocalAuthority { get; set; }

    public string? ParliamentaryConstituency { get; set; }

    public string? Postcode { get; set; }

    public int? TheIncomeDeprivationAffectingChildrenIndexIdaciQuintile { get; set; }

    public int? TotalNumberOfPupils { get; set; }

    public string? LatestSection8InspectionNumberSinceLastFullInspection { get; set; }

    public string? DoesTheSection8InspectionRelateToTheUrnOfTheCurrentSchool { get; set; }

    public int? UrnAtTimeOfTheSection8Inspection { get; set; }

    public int? LaestabAtTimeOfTheSection8Inspection { get; set; }

    public string? SchoolNameAtTimeOfTheLatestSection8Inspection { get; set; }

    public string? SchoolTypeAtTimeOfTheLatestSection8Inspection { get; set; }

    public int? NumberOfSection8InspectionsSinceTheLastFullInspection { get; set; }

    public string? DateOfLatestSection8Inspection { get; set; }

    public string? Section8InspectionPublicationDate { get; set; }

    public string? DidTheLatestSection8InspectionConvertToAFullInspection { get; set; }

    public string? Section8InspectionOverallOutcome { get; set; }

    public int? NumberOfOtherSection8InspectionsSinceLastFullInspection { get; set; }

    public string? InspectionNumberOfLatestFullInspection { get; set; }

    public string? InspectionType { get; set; }

    public string? InspectionTypeGrouping { get; set; }

    public string? EventTypeGrouping { get; set; }

    public string? InspectionStartDate { get; set; }

    public string? InspectionEndDate { get; set; }

    public string? PublicationDate { get; set; }

    public string? DoesTheLatestFullInspectionRelateToTheUrnOfTheCurrentSchool { get; set; }

    public int? UrnAtTimeOfLatestFullInspection { get; set; }

    public int? LaestabAtTimeOfLatestFullInspection { get; set; }

    public string? SchoolNameAtTimeOfLatestFullInspection { get; set; }

    public string? SchoolTypeAtTimeOfLatestFullInspection { get; set; }

    public int? OverallEffectiveness { get; set; }

    public string? CategoryOfConcern { get; set; }

    public int? QualityOfEducation { get; set; }

    public int? BehaviourAndAttitudes { get; set; }

    public int? PersonalDevelopment { get; set; }

    public int? EffectivenessOfLeadershipAndManagement { get; set; }

    public string? SafeguardingIsEffective { get; set; }

    public int? EarlyYearsProvisionWhereApplicable { get; set; }

    public int? SixthFormProvisionWhereApplicable { get; set; }

    public string? PreviousFullInspectionNumber { get; set; }

    public string? PreviousInspectionStartDate { get; set; }

    public string? PreviousInspectionEndDate { get; set; }

    public string? PreviousPublicationDate { get; set; }

    public string? DoesThePreviousFullInspectionRelateToTheUrnOfTheCurrentSchool { get; set; }

    public int? UrnAtTimeOfPreviousFullInspection { get; set; }

    public int? LaestabAtTimeOfPreviousFullInspection { get; set; }

    public string? SchoolNameAtTimeOfPreviousFullInspection { get; set; }

    public string? SchoolTypeAtTimeOfPreviousFullInspection { get; set; }

    public string? PreviousFullInspectionOverallEffectiveness { get; set; }

    public string? PreviousCategoryOfConcern { get; set; }

    public int? PreviousQualityOfEducation { get; set; }

    public int? PreviousBehaviourAndAttitudes { get; set; }

    public int? PreviousPersonalDevelopment { get; set; }

    public int? PreviousEffectivenessOfLeadershipAndManagement { get; set; }

    public string? PreviousSafeguardingIsEffective { get; set; }

    public int? PreviousEarlyYearsProvisionWhereApplicable { get; set; }

    public string? PreviousSixthFormProvisionWhereApplicable { get; set; }
}

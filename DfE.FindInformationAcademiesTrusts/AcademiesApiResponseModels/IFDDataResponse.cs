using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class IfdDataResponse
{
    public string? TrustOpenDate { get; set; }
    public string? LeadRscRegion { get; set; }
    public string? TrustContactPhoneNumber { get; set; }
    public string? PerformanceAndRiskDateOfMeeting { get; set; }
    public string? PrioritisedAreaOfReview { get; set; }
    public string? CurrentSingleListGrouping { get; set; }
    public string? DateOfGroupingDecision { get; set; }
    public string? DateEnteredOntoSingleList { get; set; }
    public string? TrustReviewWriteup { get; set; }
    public string? DateOfTrustReviewMeeting { get; set; }
    public string? FollowupLetterSent { get; set; }
    public string? DateActionPlannedFor { get; set; }
    public string? WipSummaryGoesToMinister { get; set; }
    public string? ExternalGovernanceReviewDate { get; set; }
    public string? EfficiencyIcfPreviewCompleted { get; set; }
    public string? EfficiencyIcfPreviewOther { get; set; }
    public string? LinkToWorkplaceForEfficiencyIcfReview { get; set; }
    public string? NumberInTrust { get; set; }
    public string? TrustType { get; set; }
    public AddressResponse? TrustAddress { get; set; }
}

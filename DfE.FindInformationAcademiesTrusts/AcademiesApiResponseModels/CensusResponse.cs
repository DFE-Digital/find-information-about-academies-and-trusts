using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class CensusResponse
{
    public string? CensusDate { get; set; }
    public string? NumberOfPupils { get; set; }
    public string? NumberOfBoys { get; set; }
    public string? NumberOfGirls { get; set; }
    public string? PercentageSen { get; set; }
    public string? PercentageFsm { get; set; }
    public string? PercentageEnglishNotFirstLanguage { get; set; }
    public string? PerceantageEnglishFirstLanguage { get; set; }
    public string? PercentageFirstLanguageUnclassified { get; set; }
    public string? NumberEligableForFsm { get; set; }
    public string? NumberEligableForFsm6Years { get; set; }
    public string? PercentageEligableForFsm6Years { get; set; }
}

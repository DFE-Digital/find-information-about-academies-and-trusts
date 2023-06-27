using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

[ExcludeFromCodeCoverage] // API response model POCO
public class GIASDataResponse
{
    public string? GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? GroupType { get; set; }
    public string? CompaniesHouseNumber { get; set; }
    public AddressResponse? GroupContactAddress { get; set; }
    public string? Ukprn { get; set; }
}

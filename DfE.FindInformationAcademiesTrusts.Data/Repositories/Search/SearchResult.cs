namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public class SearchResult
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Street { get; set; }
    public string? Locality { get; set; }
    public string? Town { get; set; }
    public string? PostCode { get; set; }
    public string? TrustGroupId { get; set; }
    public string Type { get; set; } = null!;
    public bool IsTrust { get; set; }
}

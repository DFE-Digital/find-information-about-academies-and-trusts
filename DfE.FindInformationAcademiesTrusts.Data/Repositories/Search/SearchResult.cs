namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public class SearchResult
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string? TrustGroupId { get; set; }
    public string Type { get; set; } = null!;
    public bool IsTrust { get; set; }
}

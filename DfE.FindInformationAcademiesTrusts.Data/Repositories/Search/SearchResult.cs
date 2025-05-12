namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public class SearchResult
{
    public string Name { get; set; } = null!;
    public string? Street { get; set; }
    public string? Locality { get; set; }
    public string? Town { get; set; }
    public string? PostCode { get; set; }
    public string Identifier { get; set; } = null!;
    public bool IsTrust { get; set; }
}

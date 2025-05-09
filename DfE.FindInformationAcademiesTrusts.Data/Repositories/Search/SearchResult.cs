namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Search;

public class SearchResult
{
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string Identifier { get; set; } = null!;
    public bool IsTrust { get; set; }
}

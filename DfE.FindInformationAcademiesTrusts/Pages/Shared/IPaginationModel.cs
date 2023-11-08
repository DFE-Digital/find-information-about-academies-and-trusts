using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public interface IPaginationModel
{
    public string PageName { get; }
    public IPageStatus PageStatus { get; }
    public int PageNumber { get; }
    public Dictionary<string, string> PaginationRouteData { get; }
}

using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    Trust Trust { get; set; }
    string PageName { get; }
    string Section { get; }
}

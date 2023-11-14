using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public interface ITrustsAreaModel
{
    Trust Trust { get; set; }

    /// <summary>
    /// The name of the page as displayed in the page h1
    /// </summary>
    string PageName { get; init; }

    /// <summary>
    /// The name of the section the page sits under in side navigation
    /// </summary>
    string Section { get; }
}

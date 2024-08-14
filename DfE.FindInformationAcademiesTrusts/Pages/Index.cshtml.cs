using DfE.FindInformationAcademiesTrusts.Pages.Shared;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : BasePageModel, IPageSearchFormModel
{
    public IndexModel()
    {
        ShowHeaderSearch = false;
    }

    public string PageSearchFormInputId => "home-search";
}

using DfE.FindInformationAcademiesTrusts.Pages.Shared;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : BasePageModel, IPageSearchFormModel
{
    public IndexModel()
    {
        ShowHeaderSearch = false;
    }
// fake change to test codeowners
    public string PageSearchFormInputId => "home-search-test-fake-codeowners";
}

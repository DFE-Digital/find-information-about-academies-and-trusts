using DfE.FIAT.Web.Pages.Shared;

namespace DfE.FIAT.Web.Pages;

public class IndexModel : BasePageModel, IPageSearchFormModel
{
    public IndexModel()
    {
        ShowHeaderSearch = false;
    }

    public string PageSearchFormInputId => "home-search";
}

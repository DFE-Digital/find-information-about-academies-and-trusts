using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.Api;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : BasePageModel, IPageSearchFormModel
{
    public IndexModel(IApiService service)
    {
        ShowHeaderSearch = false;

        // TODO: Remove this, only kept in for debugging
        service.Get("todos/1");
    }

    public string PageSearchFormInputId => "home-search";
}

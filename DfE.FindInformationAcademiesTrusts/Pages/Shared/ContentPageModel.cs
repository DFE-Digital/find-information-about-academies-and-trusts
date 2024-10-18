using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Shared;

public abstract class ContentPageModel : BasePageModel
{
    private bool _showBreadcrumb = true;

    public bool ShowBreadcrumb
    {
        get => _showBreadcrumb && User.HasAccessToFiat();
        set => _showBreadcrumb = value;
    }
}

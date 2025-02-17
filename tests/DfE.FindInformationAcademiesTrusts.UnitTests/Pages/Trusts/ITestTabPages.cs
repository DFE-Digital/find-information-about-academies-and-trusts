namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public interface ITestTabPages
{
    Task OnGetAsync_should_set_active_TabList_to_current_tab();
    Task OnGetAsync_should_populate_TabList_to_tabs();
    Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName();
}

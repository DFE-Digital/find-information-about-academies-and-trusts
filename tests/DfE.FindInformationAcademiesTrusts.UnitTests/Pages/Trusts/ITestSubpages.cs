namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public interface ITestSubpages
{
    Task OnGetAsync_should_set_active_SubNavigationLink_to_current_subpage();
    Task OnGetAsync_should_populate_SubNavigationLinks_to_subpages();
    Task OnGetAsync_should_configure_TrustPageMetadata_SubPageName();
}

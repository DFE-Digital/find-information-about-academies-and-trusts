using DfE.FindInformationAcademiesTrusts.Pages.Schools;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools;

public abstract class BaseSchoolPageTests<T> where T : SchoolAreaModel
{
    protected T Sut = null!;

    [Fact]
    public void ShowHeaderSearch_should_be_true()
    {
        Sut.ShowHeaderSearch.Should().Be(true);
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_PageMetadata_PageName();

    [Fact]
    public abstract Task OnGetAsync_should_configure_PageMetadata_SubPageName();

    [Fact]
    public abstract Task OnGetAsync_sets_correct_data_source_list();
}

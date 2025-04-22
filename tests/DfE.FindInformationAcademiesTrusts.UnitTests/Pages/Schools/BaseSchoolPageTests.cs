using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools;
using DfE.FindInformationAcademiesTrusts.Services.School;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools;

public abstract class BaseSchoolPageTests<T> where T : SchoolAreaModel
{
    protected T Sut = null!;
    protected readonly ISchoolService MockSchoolService = Substitute.For<ISchoolService>();
    protected const int Urn = 123456;

    protected readonly SchoolSummaryServiceModel DummySchoolSummary =
        new(Urn, "Cool school", "Community school", SchoolCategory.LaMaintainedSchool);

    protected BaseSchoolPageTests()
    {
        MockSchoolService.GetSchoolSummaryAsync(Urn).Returns(DummySchoolSummary);
    }

    [Fact]
    public abstract Task OnGetAsync_should_configure_PageMetadata_PageName();

    [Fact]
    public abstract Task OnGetAsync_should_configure_PageMetadata_SubPageName();

    [Fact]
    public abstract Task OnGetAsync_sets_correct_data_source_list();

    [Fact]
    public void ShowHeaderSearch_should_be_true()
    {
        Sut.ShowHeaderSearch.Should().Be(true);
    }

    [Fact]
    public async Task OnGetAsync_should_fetch_a_SchoolSummary_by_urn()
    {
        Sut.Urn = DummySchoolSummary.Urn;

        await Sut.OnGetAsync();
        Sut.SchoolSummary.Should().Be(DummySchoolSummary);
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_school_is_not_found()
    {
        MockSchoolService.GetSchoolSummaryAsync(111111).Returns((SchoolSummaryServiceModel?)null);

        Sut.Urn = 111111;
        var result = await Sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_return_not_found_result_if_urn_is_not_provided()
    {
        MockSchoolService.GetSchoolSummaryAsync(0).Returns((SchoolSummaryServiceModel?)null);

        Sut.Urn = 0;
        var result = await Sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_should_return_page_result_if_urn_exists()
    {
        var result = await Sut.OnGetAsync();
        result.Should().BeOfType<PageResult>();
    }
}

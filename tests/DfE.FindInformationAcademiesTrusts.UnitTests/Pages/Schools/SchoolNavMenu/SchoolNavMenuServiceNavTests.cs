using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using Sut = DfE.FindInformationAcademiesTrusts.Pages.Schools.SchoolNavMenu;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.SchoolNavMenu;

public class SchoolNavMenuServiceNavTests : SchoolNavMenuTestsBase
{
    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public void GetServiceNavLinks_should_set_route_data_to_urn(string expectedUrn)
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel), expectedUrn);

        var results = Sut.GetServiceNavLinks(activePage);

        results.Should().AllSatisfy(link =>
        {
            var route = link.AspAllRouteData.Should().ContainSingle().Subject;
            route.Key.Should().Be("urn");
            route.Value.Should().Be(expectedUrn);
        });
    }

    [Theory]
    [InlineData(SchoolCategory.LaMaintainedSchool, "School")]
    [InlineData(SchoolCategory.Academy, "Academy")]
    public void GetServiceNavLinks_should_set_hidden_text_to_school_type(SchoolCategory schoolCategory,
        string expectedHiddenText)
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel), schoolCategory: schoolCategory);

        var results = Sut.GetServiceNavLinks(activePage);

        results.Should().AllSatisfy(link => { link.VisuallyHiddenLinkText.Should().Be(expectedHiddenText); });
    }

    [Fact]
    public void GetServiceNavLinks_should_return_expected_links()
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel));

        var results = Sut.GetServiceNavLinks(activePage);

        results.Should().SatisfyRespectively(
            l =>
            {
                l.LinkDisplayText.Should().Be("Overview");
                l.AspPage.Should().Be("/Schools/Overview/Details");
                l.TestId.Should().Be("overview-nav");
            }
        );
    }

    [Theory]
    [MemberData(nameof(SubPageTypes))]
    public void GetServiceNavLinks_should_set_active_page_link_when_on_any_subpage(Type activePageType)
    {
        var activePage = GetMockSchoolPage(activePageType);
        var expectedActivePageLink = GetExpectedServiceNavAspLink(activePageType);

        var results = Sut.GetServiceNavLinks(activePage);

        results.Should().ContainSingle(l => l.LinkIsActive).Which.AspPage.Should().Be(expectedActivePageLink);
    }

    private static string GetExpectedServiceNavAspLink(Type pageType)
    {
        return pageType.Name switch
        {
            nameof(DetailsModel) => "/Schools/Overview/Details",
            _ => throw new ArgumentException("Couldn't get expected service nav asp link for given page type",
                nameof(pageType))
        };
    }
}

using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Schools;
using DfE.FindInformationAcademiesTrusts.Pages.Schools.Overview;
using Sut = DfE.FindInformationAcademiesTrusts.Pages.Schools.SchoolNavMenu;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Schools.SchoolNavMenu;

public class SchoolNavMenuSubNavTests : SchoolNavMenuTestsBase
{
    [Theory]
    [InlineData("1234")]
    [InlineData("5678")]
    public void GetSubNavLinks_should_set_route_data_to_urn(string expectedUrn)
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel), expectedUrn);

        var results = Sut.GetSubNavLinks(activePage);

        results.Should().AllSatisfy(link =>
        {
            var route = link.AspAllRouteData.Should().ContainSingle().Subject;
            route.Key.Should().Be("urn");
            route.Value.Should().Be(expectedUrn);
        });
    }

    [Theory]
    [MemberData(nameof(SubPageTypes))]
    public void GetSubNavLinks_should_set_hidden_text_to_page_name(Type activePageType)
    {
        var activePage = GetMockSchoolPage(activePageType);
        var expectedPageName = GetExpectedPageName(activePageType);

        var results = Sut.GetSubNavLinks(activePage);

        results.Should().AllSatisfy(link => { link.VisuallyHiddenLinkText.Should().Be(expectedPageName); });
    }

    private static string GetExpectedPageName(Type pageType)
    {
        return pageType.Name switch
        {
            nameof(DetailsModel) => "Overview",
            _ => throw new ArgumentException("Couldn't get expected name for given page type", nameof(pageType))
        };
    }

    [Theory]
    [MemberData(nameof(SubPageTypes))]
    public void GetSubNavLinks_should_set_active_sub_page_link(Type activePageType)
    {
        var activePage = GetMockSchoolPage(activePageType);
        var expectedActiveSubPageLink = GetSubPageLinkTo(activePageType);

        var results = Sut.GetSubNavLinks(activePage);

        results.Should().ContainSingle(l => l.LinkIsActive).Which.AspPage.Should().Be(expectedActiveSubPageLink);
    }

    private static string GetSubPageLinkTo(Type pageType)
    {
        return pageType.Name switch
        {
            nameof(DetailsModel) => "/Schools/Overview/Details",
            _ => throw new ArgumentException("Couldn't get expected sub page nav asp link for given page type",
                nameof(pageType))
        };
    }

    [Fact]
    public void GetSubNavLinks_should_return_expected_links_for_overview_when_school()
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel), schoolCategory: SchoolCategory.LaMaintainedSchool);

        var results = Sut.GetSubNavLinks(activePage);

        results.Should().SatisfyRespectively(
            l =>
            {
                l.LinkDisplayText.Should().Be("School details");
                l.AspPage.Should().Be("/Schools/Overview/Details");
                l.TestId.Should().Be("overview-details-subnav");
            }
        );
    }

    [Fact]
    public void GetSubNavLinks_should_return_expected_links_for_overview_when_academy()
    {
        var activePage = GetMockSchoolPage(typeof(DetailsModel), schoolCategory: SchoolCategory.Academy);

        var results = Sut.GetSubNavLinks(activePage);

        results.Should().SatisfyRespectively(
            l =>
            {
                l.LinkDisplayText.Should().Be("Academy details");
                l.AspPage.Should().Be("/Schools/Overview/Details");
                l.TestId.Should().Be("overview-details-subnav");
            }
        );
    }

    [Fact]
    public void GetSubNavLinks_should_throw_if_page_not_supported()
    {
        var activePage = Substitute.For<ISchoolAreaModel>();

        var action = () => Sut.GetSubNavLinks(activePage);

        action.Should().Throw<ArgumentOutOfRangeException>()
            .Which.Message.Should().StartWith("Page type is not supported.");
    }
}

using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Shared;

public class ContentPageModelTests
{
    private readonly ContentPageModel _sut;
    private readonly MockHttpContext _mockHttpContext = new();

    private class ContentPageModelImplementation : ContentPageModel;

    public ContentPageModelTests()
    {
        _sut = new ContentPageModelImplementation
        {
            PageContext = new PageContext
            {
                HttpContext = _mockHttpContext.Object
            }
        };
    }

    [Fact]
    public void ShowBreadcrumb_should_default_to_true_for_authorized_user()
    {
        _sut.ShowBreadcrumb.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowBreadcrumb_should_retain_setting_when_set_to_true_for_authorized_user(bool value)
    {
        _sut.ShowBreadcrumb = value;
        _sut.ShowBreadcrumb.Should().Be(value);
    }

    [Fact]
    public void ShowBreadcrumb_should_default_to_false_for_unauthorized_user()
    {
        _mockHttpContext.SetUserTo(MockHttpContext.UserAuthState.Unauthorised);
        _sut.ShowBreadcrumb.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowBreadcrumb_should_be_always_be_false_for_unauthorized_user(bool value)
    {
        _mockHttpContext.SetUserTo(MockHttpContext.UserAuthState.Unauthorised);
        _sut.ShowBreadcrumb = value;
        _sut.ShowBreadcrumb.Should().BeFalse();
    }
}

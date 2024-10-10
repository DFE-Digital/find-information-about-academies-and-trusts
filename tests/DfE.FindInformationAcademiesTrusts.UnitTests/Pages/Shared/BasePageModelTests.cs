using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Shared;

public class BasePageModelTests
{
    private readonly MockHttpContext _mockHttpContext = new();
    private readonly PageContext _pageContext;

    private class BasePageModelImplementation : BasePageModel;

    public BasePageModelTests()
    {
        _pageContext = new PageContext
        {
            HttpContext = _mockHttpContext.Object
        };
    }

    [Fact]
    public void ShowHeaderSearch_should_default_to_true_for_authorized_user()
    {
        var sut = new BasePageModelImplementation
        {
            PageContext = _pageContext
        };

        sut.ShowHeaderSearch.Should().BeTrue();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowHeaderSearch_should_retain_setting_for_authorized_user(bool value)
    {
        var sut = new BasePageModelImplementation
        {
            PageContext = _pageContext,
            ShowHeaderSearch = value
        };

        sut.ShowHeaderSearch.Should().Be(value);
    }

    [Fact]
    public void ShowHeaderSearch_should_default_to_false_for_unauthorized_user()
    {
        _mockHttpContext.SetupUnauthorizedUser();
        var sut = new BasePageModelImplementation
        {
            PageContext = _pageContext,
            ShowHeaderSearch = true
        };

        sut.ShowHeaderSearch.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowHeaderSearch_should_be_always_be_false_for_unauthorized_user(bool value)
    {
        _mockHttpContext.SetupUnauthorizedUser();
        var sut = new BasePageModelImplementation
        {
            PageContext = _pageContext,
            ShowHeaderSearch = value
        };

        sut.ShowHeaderSearch.Should().BeFalse();
    }
}

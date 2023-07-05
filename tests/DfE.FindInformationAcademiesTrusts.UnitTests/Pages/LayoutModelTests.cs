using DfE.FindInformationAcademiesTrusts.Pages.Shared;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class LayoutModelTests
{
    [Fact]
    public void GetPageWidthClass_should_return_string_parameter_as_default()
    {
        var className = "width-class";
        var sut = new LayoutModel();
        var result = sut.GetPageWidthClass(className);
        result.Should().Be(className);
    }

    [Fact]
    public void GetPageWidthClass_should_return_null_if_UsePageWidth_is_false()
    {
        var className = "width-class";
        var sut = new LayoutModel
        {
            UsePageWidthContainer = false
        };
        var result = sut.GetPageWidthClass(className);
        result.Should().Be(null);
    }
}

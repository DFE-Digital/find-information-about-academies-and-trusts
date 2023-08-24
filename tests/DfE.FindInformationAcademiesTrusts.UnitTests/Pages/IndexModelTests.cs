using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class IndexModelTests
{
    [Fact]
    public void KeyWords_property_is_empty_by_default()
    {
        var sut = new IndexModel();
        sut.KeyWords.Should().Be("");
    }

    [Fact]
    public void UsePageWidthContainer_should_be_false()
    {
        var sut = new IndexModel();

        sut.UsePageWidthContainer.Should().Be(false);
    }

    [Fact]
    public void InputId_should_have_a_fixed_value()
    {
        var sut = new IndexModel();

        sut.InputId.Should().Be("home-search");
    }
}

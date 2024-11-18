using DfE.FIAT.Web.Pages;

namespace DfE.FIAT.UnitTests.Pages;

public class IndexModelTests
{
    [Fact]
    public void KeyWords_property_is_empty_by_default()
    {
        var sut = new IndexModel();
        sut.KeyWords.Should().Be("");
    }

    [Fact]
    public void InputId_should_have_a_fixed_value()
    {
        var sut = new IndexModel();

        sut.PageSearchFormInputId.Should().Be("home-search");
    }
}

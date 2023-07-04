using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class IndexModelTests
{
    [Fact]
    public void Query_property_is_empty_by_default()
    {
        var sut = new IndexModel();
        sut.Query.Should().Be("");
    }

    [Fact]
    public void UsePageWidthContainer_should_be_false()
    {
        var sut = new IndexModel();

        sut.UsePageWidthContainer.Should().Be(false);
    }
}

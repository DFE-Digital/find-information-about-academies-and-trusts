using DfE.FindInformationAcademiesTrusts.Pages;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class IndexModelTests
{
    [Theory]
    [InlineData("test")]
    [InlineData("")]
    [InlineData("multiple query terms")]
    [InlineData("Strings with Capitals")]
    public void OnPost_redirects_to_search_with_query(string query)
    {
        var sut = new IndexModel
        {
            Query = query
        };

    [Fact]
    public void UsePageWidthContainer_should_be_false()
    {
        var sut = new IndexModel();

        sut.UsePageWidthContainer.Should().Be(false);
    }
}

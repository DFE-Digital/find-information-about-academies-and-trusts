using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Http;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class ErrorModelTests
{
    private readonly MockHttpContext _mockHttpContext = new();
    private readonly ErrorModel _sut;

    public ErrorModelTests()
    {
        Mock<IHttpContextAccessor> mockHttpAccessor = new();
        mockHttpAccessor.Setup(m => m.HttpContext).Returns(_mockHttpContext.Object);

        _sut = new ErrorModel(mockHttpAccessor.Object);
    }

    [Fact]
    public void Is404Result_should_be_false_by_default()
    {
        _sut.Is404Result.Should().BeFalse();
    }

    [Fact]
    public void Is404Result_should_be_true_if_404_Status_Code()
    {
        _sut.OnGet("404");

        _sut.Is404Result.Should().BeTrue();
    }

    [Theory]
    [InlineData("500")]
    [InlineData("400")]
    [InlineData("403")]
    public void Is404Result_should_be_false_if_not_Status_Code(string code)
    {
        _sut.OnGet(code);
        _sut.Is404Result.Should().BeFalse();
    }

    [Fact]
    public void OriginalPathAndQuery_should_be_unknown_by_default()
    {
        _sut.OriginalPathAndQuery.Should().Be("Unknown");
    }

    [Fact]
    public void OriginalPathAndQuery_should_be_set_when_404()
    {
        _mockHttpContext.SetNotFoundUrl("my.fiat.host", "/notfound", "?var=something");

        _sut.OnGet("404");

        _sut.OriginalPathAndQuery.Should().Be("my.fiat.host/notfound?var=something");
    }
}

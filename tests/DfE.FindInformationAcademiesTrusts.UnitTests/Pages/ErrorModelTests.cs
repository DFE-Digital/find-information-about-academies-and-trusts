using DfE.FindInformationAcademiesTrusts.Pages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages;

public class ProblemWithProductModelTests
{
    [Fact]
    public void Is404Result_should_be_false_by_default()
    {
        var sut = new ProblemWithProduct();
        sut.Is404Result.Should().BeFalse();
    }

    [Fact]
    public void Is404Result_should_be_true_if_404_Status_Code()
    {
        var sut = new ProblemWithProduct();
        sut.OnGet("404");

        sut.Is404Result.Should().BeTrue();
    }

    [Theory]
    [InlineData("500")]
    [InlineData("400")]
    [InlineData("403")]
    public void Is404Result_should_be_false_if_not_Status_Code(string code)
    {
        var sut = new ProblemWithProduct();
        sut.OnGet(code);
        sut.Is404Result.Should().BeFalse();
    }
}

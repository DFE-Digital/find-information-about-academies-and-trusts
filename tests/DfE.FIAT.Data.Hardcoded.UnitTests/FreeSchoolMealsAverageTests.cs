namespace DfE.FIAT.Data.Hardcoded.UnitTests;

public class FreeSchoolMealsAverageTests
{
    private readonly FreeSchoolMealsAverage _sut = new(1, "test", "test");

    [Fact]
    public void OldLaCode_should_be_initialised_with_constructor()
    {
        _sut.OldLaCode.Should().Be(1);
    }

    [Fact]
    public void LaName_should_be_initialised_with_constructor()
    {
        _sut.LaName.Should().Be("test");
    }

    [Fact]
    public void NewLaCode_should_be_initialised_with_constructor()
    {
        _sut.NewLaCode.Should().Be("test");
    }

    [Fact]
    public void NewLaCode_is_not_required_constructor_parameter()
    {
        var sut = new FreeSchoolMealsAverage(2, "test");
        sut.NewLaCode.Should().BeNull();
    }
}

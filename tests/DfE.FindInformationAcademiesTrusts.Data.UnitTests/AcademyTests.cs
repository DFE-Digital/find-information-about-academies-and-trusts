using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class AcademyTests
{
    [Theory]
    [InlineData(25, 100, 25F)]
    [InlineData(1, 4, 25F)]
    [InlineData(816, 1074, 76F)]
    [InlineData(100, 100, 100F)]
    [InlineData(101, 100, 101F)]
    [InlineData(0, 100, 0F)]
    public void PercentageFull_should_return_the_correct_percentage_calculation(int numberOfPupils,
        int capacity, float expected)
    {
        var sut =
            DummyAcademyFactory.GetDummyAcademy(111, numberOfPupils: numberOfPupils, schoolCapacity: capacity);
        var result = sut.PercentageFull;
        result.Should().BeApproximately(expected, 0.01F);
    }

    [Fact]
    public void PercentageFull_should_return_null_string_if_number_of_pupils_has_no_value()
    {
        var sut =
            DummyAcademyFactory.GetDummyAcademy(111, numberOfPupils: null, schoolCapacity: 12);
        var result = sut.PercentageFull;
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0)]
    public void PercentageFull_should_return_null_if_school_capacity_has_no_value(int? schoolCapacity)
    {
        var sut =
            DummyAcademyFactory.GetDummyAcademy(111, numberOfPupils: 100, schoolCapacity: schoolCapacity);
        var result = sut.PercentageFull;
        result.Should().BeNull();
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    public void PercentageFull_should_return_null_if_both_values_are_null_or_capacity_0(int? numberOfPupils,
        int? capacity)
    {
        var sut =
            DummyAcademyFactory.GetDummyAcademy(111, numberOfPupils: numberOfPupils, schoolCapacity: capacity);
        var result = sut.PercentageFull;
        result.Should().BeNull();
    }
}

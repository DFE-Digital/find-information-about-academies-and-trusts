using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class TrustOverviewServiceModelTests
{
    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new("1234", "", "", "", "", "", "", null, null, 0, new Dictionary<string, int>(), 0, 0);

    [Theory]
    [InlineData(100, 100, 100)]
    [InlineData(0, 100, null)]
    [InlineData(0, 0, null)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 50, 50)]
    [InlineData(100, 30, 30)]
    [InlineData(4, 1, 25)]
    [InlineData(3, 1, 33)]
    public void PercentageFull_ReturnsCorrectValue(int totalCapacity, int totalPupilNumbers, int? expectedPercentage)
    {
        // Arrange
        var model = BaseTrustOverviewServiceModel with
        {
            TotalCapacity = totalCapacity,
            TotalPupilNumbers = totalPupilNumbers
        };

        // Act
        var percentageFull = model.PercentageFull;

        // Assert
        percentageFull.Should().Be(expectedPercentage);
    }

    [Fact]
    public void IsMultiAcademyTrust_should_return_true_if_trust_has_type_multiacademytrust()
    {
        var sut = BaseTrustOverviewServiceModel with { Type = "Multi-academy trust" };
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Single-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsMultiAcademyTrust_should_return_false_if_trust_does_not_have_type_multiacademytrust(string type)
    {
        var sut = BaseTrustOverviewServiceModel with { Type = type };
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeFalse();
    }

    [Fact]
    public void IsSingleAcademyTrust_should_return_true_if_trust_has_type_singleacademytrust()
    {
        var sut = BaseTrustOverviewServiceModel with { Type = "Single-academy trust" };
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Multi-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsSingleAcademyTrust_should_return_false_if_trust_does_not_have_type_singleacademytrust(string type)
    {
        var sut = BaseTrustOverviewServiceModel with { Type = type };
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeFalse();
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class OfstedRatingScoreExtensionsTests
{
    [Theory]
    [InlineData(OfstedRatingScore.Outstanding, "Outstanding")]
    [InlineData(OfstedRatingScore.Good, "Good")]
    [InlineData(OfstedRatingScore.RequiresImprovement, "Requires improvement")]
    [InlineData(OfstedRatingScore.Inadequate, "Inadequate")]
    [InlineData(OfstedRatingScore.InsufficientEvidence, "Insufficient evidence")]
    [InlineData(OfstedRatingScore.NoJudgement, "No judgement")]
    [InlineData(OfstedRatingScore.DoesNotApply, "Does not apply")]
    [InlineData(OfstedRatingScore.NotInspected, "Not yet inspected")]
    public void ToDisplayString_ReturnsCorrectString_ForDefinedEnumValues(OfstedRatingScore rating, string expected)
    {
        // Act
        var result = rating.ToDisplayString();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToDisplayString_ReturnsUnknown_ForUndefinedEnumValue()
    {
        // Arrange
        // Cast an undefined integer value to the enum
        var undefinedRating = (OfstedRatingScore)999;

        // Act
        var result = undefinedRating.ToDisplayString();

        // Assert
        result.Should().Be("Unknown");
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding, 1)]
    [InlineData(OfstedRatingScore.Good, 2)]
    [InlineData(OfstedRatingScore.RequiresImprovement, 3)]
    [InlineData(OfstedRatingScore.Inadequate, 4)]
    [InlineData(OfstedRatingScore.InsufficientEvidence, 5)]
    [InlineData(OfstedRatingScore.NoJudgement, 6)]
    [InlineData(OfstedRatingScore.DoesNotApply, 7)]
    [InlineData(OfstedRatingScore.NotInspected, 8)]
    public void ToDataSortValue_ReturnsCorrectValue_ForDefinedEnumValues(OfstedRatingScore rating, int expected)
    {
        // Act
        var result = rating.ToDataSortValue();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ToDataSortValue_ReturnsUnknown_ForUndefinedEnumValue()
    {
        // Arrange
        // Cast an undefined integer value to the enum
        var undefinedRating = (OfstedRatingScore)999;

        // Act
        var result = undefinedRating.ToDataSortValue();

        // Assert
        result.Should().Be(-1);
    }
}

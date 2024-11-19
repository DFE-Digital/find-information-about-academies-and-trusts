using DfE.FIAT.Data;
using DfE.FIAT.Web.Pages.Trusts.Academies;

namespace DfE.FIAT.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingCellModelTests
{
    [Fact]
    public void IsAfterJoining_returns_true_if_RatingDate_is_after_AcademyJoinedDate()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();

        var result = sut.IsAfterJoining;
        result.Should().Be(true);
    }

    [Fact]
    public void IsAfterJoining_returns_true_if_RatingDate_is_same_as_AcademyJoinedDate()
    {
        var sut = new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(),
            OfstedRating = new OfstedRating(2, new DateTime())
        };

        sut.IsAfterJoining.Should().Be(true);
    }

    [Fact]
    public void IsAfterJoining_returns_false_if_RatingDate_is_before_AcademyJoinedDate()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();
        sut.IsAfterJoining.Should().Be(false);
    }

    [Fact]
    public void IsAfterJoining_returns_false_if_RatingDate_is_null()
    {
        var sut = GetSutWithNotYetInspectedRating();

        sut.IsAfterJoining.Should().Be(false);
    }

    [Fact]
    public void TagClasses_returns_default_tag_class_if_IsAfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();

        sut.TagClasses.Should().Be("govuk-tag");
    }

    [Fact]
    public void TagClasses_returns_grey_tag_class_variant_if_IsAfterJoining_is_false()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();

        sut.TagClasses.Should().Be("govuk-tag govuk-tag--grey");
    }

    [Fact]
    public void TagText_returns_AfterJoining_If_Rating_Is_AfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();
        sut.TagText.Should().Be("After joining");
    }

    [Fact]
    public void TagText_returns_BeforeJoining_If_Rating_Is_Not_AfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();

        sut.TagText.Should().Be("Before joining");
    }

    [Theory]
    [InlineData(OfstedRatingScore.Good, "Good")]
    [InlineData(OfstedRatingScore.None, "Not yet inspected")]
    [InlineData(OfstedRatingScore.Inadequate, "Inadequate")]
    [InlineData(OfstedRatingScore.Outstanding, "Outstanding")]
    [InlineData(OfstedRatingScore.RequiresImprovement, "Requires improvement")]
    public void OfstedRatingDescription_returns_right_description(OfstedRatingScore score, string expected)
    {
        var sut = new OfstedRatingCellModel
        {
            OfstedRating = new OfstedRating((int)score, new DateTime()),
            AcademyJoinedDate = new DateTime()
        };

        sut.OfstedRatingDescription.Should().Be(expected);
    }

    [Fact]
    public void OfstedRatingDescription_returns_empty_if_score_not_in_range()
    {
        var sut = new OfstedRatingCellModel
        {
            OfstedRating = new OfstedRating(0, new DateTime()),
            AcademyJoinedDate = new DateTime()
        };

        sut.OfstedRatingDescription.Should().BeEmpty();
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding, 1)]
    [InlineData(OfstedRatingScore.Good, 2)]
    [InlineData(OfstedRatingScore.RequiresImprovement, 3)]
    [InlineData(OfstedRatingScore.Inadequate, 4)]
    [InlineData(OfstedRatingScore.None, 5)]
    public void OfstedRatingSortValue_returns_right_value(OfstedRatingScore score, int expected)
    {
        var sut = new OfstedRatingCellModel
        {
            OfstedRating = new OfstedRating((int)score, new DateTime()),
            AcademyJoinedDate = new DateTime()
        };

        sut.OfstedRatingSortValue.Should().Be(expected);
    }

    private static OfstedRatingCellModel GetSutWithOfstedRatingDateAfterJoining()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2020, 11, 1),
            OfstedRating = new OfstedRating((int)OfstedRatingScore.Good, new DateTime(2022, 3, 2))
        };
    }

    private static OfstedRatingCellModel GetSutWithOfstedRatingDateBeforeJoining()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2022, 3, 2),
            OfstedRating = new OfstedRating((int)OfstedRatingScore.Good, new DateTime(2020, 11, 1)),
            IdPrefix = ""
        };
    }

    private static OfstedRatingCellModel GetSutWithNotYetInspectedRating()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2022, 3, 2),
            OfstedRating = new OfstedRating((int)OfstedRatingScore.None, null)
        };
    }
}

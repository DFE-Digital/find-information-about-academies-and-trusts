using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class OfstedRatingCellModelTests
{
    private readonly OfstedRatingCellModel _sut;

    public OfstedRatingCellModelTests()
    {
        _sut = new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(),
            Rating = "Good",
            RatingDate = new DateTime()
        };
    }

    [Fact]
    public void HasRating_returns_true_if_rating_is_not_NotYetInspected()
    {
        var result = _sut.HasRating();
        result.Should().Be(true);
    }

    [Fact]
    public void HasRating_returns_false_if_rating_is_NotYetInspected()
    {
        var sut = GetSutWithNotYetInspectedRating();

        var result = sut.HasRating();
        result.Should().Be(false);
    }

    [Fact]
    public void IsAfterJoining_returns_true_if_RatingDate_is_after_AcademyJoinedDate()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();

        var result = sut.IsAfterJoining();
        result.Should().Be(true);
    }

    [Fact]
    public void IsAfterJoining_returns_true_if_RatingDate_is_same_as_AcademyJoinedDate()
    {
        var result = _sut.IsAfterJoining();
        result.Should().Be(true);
    }

    [Fact]
    public void IsAfterJoining_returns_false_if_RatingDate_is_before_AcademyJoinedDate()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();

        var result = sut.IsAfterJoining();
        result.Should().Be(false);
    }

    [Fact]
    public void IsAfterJoining_returns_false_if_RatingDate_is_null()
    {
        var sut = GetSutWithNotYetInspectedRating();

        var result = sut.IsAfterJoining();
        result.Should().Be(false);
    }

    [Fact]
    public void GetTagClasses_returns_default_tag_class_if_IsAfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();

        var result = sut.GetTagClasses();
        result.Should().Be("govuk-tag");
    }

    [Fact]
    public void GetTagClasses_returns_grey_tag_class_variant_if_IsAfterJoining_is_false()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();

        var result = sut.GetTagClasses();
        result.Should().Be("govuk-tag govuk-tag--grey");
    }

    [Fact]
    public void GetTagText_returns_AfterJoining_If_Rating_Is_AfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateAfterJoining();

        var result = sut.GetTagText();
        result.Should().Be("After joining");
    }

    [Fact]
    public void GetTagText_returns_BeforeJoining_If_Rating_Is_Not_AfterJoining()
    {
        var sut = GetSutWithOfstedRatingDateBeforeJoining();

        var result = sut.GetTagText();
        result.Should().Be("Before joining");
    }

    private static OfstedRatingCellModel GetSutWithOfstedRatingDateAfterJoining()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2020, 11, 1),
            Rating = "Good",
            RatingDate = new DateTime(2022, 3, 2)
        };
    }

    private static OfstedRatingCellModel GetSutWithOfstedRatingDateBeforeJoining()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2022, 3, 2),
            Rating = "Good",
            RatingDate = new DateTime(2020, 11, 1)
        };
    }

    private static OfstedRatingCellModel GetSutWithNotYetInspectedRating()
    {
        return new OfstedRatingCellModel
        {
            AcademyJoinedDate = new DateTime(2022, 3, 2),
            Rating = "Not yet inspected",
            RatingDate = null
        };
    }
}

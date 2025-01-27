using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Ofsted;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Ofsted;

public class SingleHeadlineGradeCellsModelTests
{
    private readonly SingleHeadlineGradeCellsModel _singleHeadlineGradeCellsModel =
        new(GetOfstedRatingWith(OfstedRatingScore.Good), BeforeOrAfterJoining.After, true);

    private static OfstedRating GetOfstedRatingWith(OfstedRatingScore singleHeadlineGradeRating)
    {
        return GetOfstedRatingWith(singleHeadlineGradeRating, new DateTime(2022, 02, 25));
    }

    private static OfstedRating GetOfstedRatingWith(OfstedRatingScore singleHeadlineGradeRating,
        DateTime inspectionDate)
    {
        return singleHeadlineGradeRating switch
        {
            OfstedRatingScore.Unknown => OfstedRating.Unknown,
            OfstedRatingScore.NotInspected => OfstedRating.NotInspected,
            _ => new OfstedRating(singleHeadlineGradeRating, OfstedRatingScore.Good, OfstedRatingScore.Good,
                OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
                CategoriesOfConcern.NoConcerns, SafeguardingScore.Yes, inspectionDate)
        };
    }

    [Fact]
    public void HasInspection_should_return_false_when_not_inspected()
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = OfstedRating.NotInspected
        };

        sut.HasInspection.Should().BeFalse();
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding)]
    [InlineData(OfstedRatingScore.Good)]
    [InlineData(OfstedRatingScore.RequiresImprovement)]
    [InlineData(OfstedRatingScore.Inadequate)]
    [InlineData(OfstedRatingScore.NoJudgement)]
    public void HasInspection_should_return_true_when_inspected(OfstedRatingScore overallEffectiveness)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(overallEffectiveness)
        };

        sut.HasInspection.Should().BeTrue();
    }

    [Theory]
    [InlineData(BeforeOrAfterJoining.Before, "Before joining")]
    [InlineData(BeforeOrAfterJoining.After, "After joining")]
    [InlineData(BeforeOrAfterJoining.NotYetInspected, "Unknown")]
    [InlineData((BeforeOrAfterJoining)999, "Unknown")]
    public void TagLabel_should_return_expected_label_for_BeforeOrAfterJoining(BeforeOrAfterJoining beforeOrAfter,
        string expected)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            BeforeOrAfterJoining = beforeOrAfter
        };

        sut.TagLabel.Should().Be(expected);
    }

    [Theory]
    [InlineData(BeforeOrAfterJoining.Before, true)]
    [InlineData(BeforeOrAfterJoining.After, false)]
    [InlineData(BeforeOrAfterJoining.NotYetInspected, false)]
    [InlineData((BeforeOrAfterJoining)999, false)]
    public void IsBeforeJoining_should_return_expected_bool_for_BeforeOrAfterJoining(BeforeOrAfterJoining beforeOrAfter,
        bool expected)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            BeforeOrAfterJoining = beforeOrAfter
        };

        sut.IsBeforeJoining.Should().Be(expected);
    }

    [Theory]
    [InlineData(OfstedRatingScore.Outstanding)]
    [InlineData(OfstedRatingScore.Good)]
    [InlineData(OfstedRatingScore.RequiresImprovement)]
    [InlineData(OfstedRatingScore.Inadequate)]
    public void RatingShouldBeBold_should_return_true_when_valid_shg(OfstedRatingScore overallEffectiveness)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(overallEffectiveness)
        };

        sut.RatingShouldBeBold.Should().BeTrue();
    }

    [Theory]
    [InlineData(OfstedRatingScore.NotInspected)]
    [InlineData(OfstedRatingScore.Unknown)]
    [InlineData(OfstedRatingScore.NoJudgement)]
    public void RatingShouldBeBold_should_return_false_when_no_shg(OfstedRatingScore overallEffectiveness)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(overallEffectiveness)
        };

        sut.RatingShouldBeBold.Should().BeFalse();
    }

    [Theory]
    [InlineData(true, "Not yet inspected")]
    [InlineData(false, "Not inspected")]
    public void InspectionDateText_should_return_not_inspected_when_no_inspection(bool isCurrent, string expected)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = OfstedRating.NotInspected,
            IsCurrent = isCurrent
        };

        sut.InspectionDateText.Should().Be(expected);
    }

    [Fact]
    public void InspectionDateText_should_return_date_when_has_inspection()
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(OfstedRatingScore.Good, new DateTime(2023, 10, 23))
        };

        sut.InspectionDateText.Should().Be("23 Oct 2023");
    }

    [Fact]
    public void InspectionDateSort_should_return_not_inspected_when_no_inspection()
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = OfstedRating.NotInspected
        };

        sut.InspectionDateSort.Should().Be("0");
    }

    [Fact]
    public void InspectionDateSort_should_return_date_when_has_inspection()
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(OfstedRatingScore.Good, new DateTime(2023, 10, 23))
        };

        sut.InspectionDateSort.Should().Be("20231023");
    }

    [Theory]
    [InlineData(OfstedRatingScore.NotInspected, true, "Not yet inspected")]
    [InlineData(OfstedRatingScore.NotInspected, false, "Not inspected")]
    [InlineData(OfstedRatingScore.Outstanding, true, "Outstanding")]
    [InlineData(OfstedRatingScore.Good, false, "Good")]
    public void SingleHeadlineGradeText_should_return_ToDisplayString(OfstedRatingScore ofstedRatingScore,
        bool isCurrent, string expected)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(ofstedRatingScore),
            IsCurrent = isCurrent
        };

        sut.SingleHeadlineGradeText.Should().Be(expected);
    }

    [Theory]
    [InlineData(OfstedRatingScore.NotInspected, 8)]
    [InlineData(OfstedRatingScore.Outstanding, 1)]
    [InlineData(OfstedRatingScore.Good, 2)]
    public void SingleHeadlineGradeSort_should_return_ToDataSortValue(OfstedRatingScore ofstedRatingScore, int expected)
    {
        var sut = _singleHeadlineGradeCellsModel with
        {
            SingleHeadlineGradeRating = GetOfstedRatingWith(ofstedRatingScore)
        };

        sut.SingleHeadlineGradeSort.Should().Be(expected);
    }
}

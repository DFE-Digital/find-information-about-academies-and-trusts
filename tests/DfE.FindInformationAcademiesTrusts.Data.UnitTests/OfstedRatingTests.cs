namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class OfstedRatingTests
{
    [Fact]
    public void NotInspected_should_be_default_all_values_to_not_inspected_and_have_null_date()
    {
        var result = OfstedRating.NotInspected;

        result.OverallEffectiveness.Should().Be(OfstedRatingScore.NotInspected);
        result.QualityOfEducation.Should().Be(OfstedRatingScore.NotInspected);
        result.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.NotInspected);
        result.PersonalDevelopment.Should().Be(OfstedRatingScore.NotInspected);
        result.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.NotInspected);
        result.EarlyYearsProvision.Should().Be(OfstedRatingScore.NotInspected);
        result.SixthFormProvision.Should().Be(OfstedRatingScore.NotInspected);
        result.CategoryOfConcern.Should().Be(CategoriesOfConcern.NotInspected);
        result.SafeguardingIsEffective.Should().Be(SafeguardingScore.NotInspected);
        result.InspectionDate.Should().BeNull();
    }

    [Fact]
    public void Unknown_should_be_default_all_values_to_unknown_and_have_null_date()
    {
        var result = OfstedRating.Unknown;

        result.OverallEffectiveness.Should().Be(OfstedRatingScore.Unknown);
        result.QualityOfEducation.Should().Be(OfstedRatingScore.Unknown);
        result.BehaviourAndAttitudes.Should().Be(OfstedRatingScore.Unknown);
        result.PersonalDevelopment.Should().Be(OfstedRatingScore.Unknown);
        result.EffectivenessOfLeadershipAndManagement.Should().Be(OfstedRatingScore.Unknown);
        result.EarlyYearsProvision.Should().Be(OfstedRatingScore.Unknown);
        result.SixthFormProvision.Should().Be(OfstedRatingScore.Unknown);
        result.CategoryOfConcern.Should().Be(CategoriesOfConcern.Unknown);
        result.SafeguardingIsEffective.Should().Be(SafeguardingScore.Unknown);
        result.InspectionDate.Should().BeNull();
    }

    [Fact]
    public void HasAnyUnknownRating_should_return_false_when_not_inspected()
    {
        OfstedRating.NotInspected.HasAnyUnknownRating.Should().BeFalse();
    }

    [Fact]
    public void HasAnyUnknownRating_should_return_false_when_all_ratings_are_set()
    {
        var sut = new OfstedRating(OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            CategoriesOfConcern.NoConcerns, SafeguardingScore.No, null);
        sut.HasAnyUnknownRating.Should().BeFalse();
    }

    [Fact]
    public void HasAnyUnknownRating_should_return_true_when_all_ratings_are_unknown()
    {
        OfstedRating.Unknown.HasAnyUnknownRating.Should().BeTrue();
    }

    [Fact]
    public void HasAnyUnknownRating_should_return_true_when_any_rating_is_unknown()
    {
        var baseRating = new OfstedRating(OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good, OfstedRatingScore.Good,
            CategoriesOfConcern.NoConcerns, SafeguardingScore.No, null);

        (baseRating with { OverallEffectiveness = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { QualityOfEducation = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { BehaviourAndAttitudes = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { PersonalDevelopment = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { EffectivenessOfLeadershipAndManagement = OfstedRatingScore.Unknown })
            .HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { EarlyYearsProvision = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { SixthFormProvision = OfstedRatingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { CategoryOfConcern = CategoriesOfConcern.Unknown }).HasAnyUnknownRating.Should().BeTrue();
        (baseRating with { SafeguardingIsEffective = SafeguardingScore.Unknown }).HasAnyUnknownRating.Should().BeTrue();
    }
}

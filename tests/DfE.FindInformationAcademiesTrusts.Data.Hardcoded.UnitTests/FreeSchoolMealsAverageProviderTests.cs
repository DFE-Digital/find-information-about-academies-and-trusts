using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;
using static DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks.DummyAcademyFactory;

namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded.UnitTests;

public class FreeSchoolMealsAverageProviderTests
{
    private readonly FreeSchoolMealsAverageProvider _sut = new();

    [Theory]
    [InlineData("Secondary", "Pupil Referral Unit", ExploreEducationStatisticsPhaseType
        .StateFundedApSchool)]
    [InlineData("Middle Deemed Secondary", "Academy Alternative Provision Converter",
        ExploreEducationStatisticsPhaseType
            .StateFundedApSchool)]
    [InlineData("Primary", "Community School", ExploreEducationStatisticsPhaseType
        .StateFundedPrimary)]
    [InlineData("16 Plus", "Academy Converter", ExploreEducationStatisticsPhaseType.StateFundedSecondary)]
    [InlineData("Secondary", "Academy Converter", ExploreEducationStatisticsPhaseType.StateFundedSecondary)]
    [InlineData("Middle Deemed Secondary", "Academy Converter",
        ExploreEducationStatisticsPhaseType.StateFundedSecondary)]
    [InlineData("Middle Deemed Secondary", "Voluntary Aided School",
        ExploreEducationStatisticsPhaseType.StateFundedSecondary)]
    [InlineData("Middle Deemed Secondary", "Foundation School",
        ExploreEducationStatisticsPhaseType.StateFundedSecondary)]
    [InlineData("All-through", "Free Schools Special", ExploreEducationStatisticsPhaseType
        .StateFundedSpecialSchool)]
    [InlineData("", "Academy Special Sponsor Led", ExploreEducationStatisticsPhaseType
        .StateFundedSpecialSchool)]
    [InlineData("Not applicable", "Foundation Special School", ExploreEducationStatisticsPhaseType
        .StateFundedSpecialSchool)]
    [InlineData("Primary", "Community Special School", ExploreEducationStatisticsPhaseType
        .StateFundedSpecialSchool)]
    public void GetKey_should_return_enum_value(string phaseOfEducation,
        string establishmentType, ExploreEducationStatisticsPhaseType expected)
    {
        var dummyAcademy = GetDummyAcademy(111, phaseOfEducation: phaseOfEducation,
            typeOfEstablishment: establishmentType);
        var result = FreeSchoolMealsAverageProvider.GetPhaseTypeKey(dummyAcademy);
        result.Should().Be(expected);
    }

    [Fact]
    public void GetKey_should_throw_exception_if_values_are_not_in_data()
    {
        var dummyAcademy = GetDummyAcademy(111, phaseOfEducation: "Not a phase",
            typeOfEstablishment: "not an establishment type");
        var act = () => FreeSchoolMealsAverageProvider.GetPhaseTypeKey(dummyAcademy);

        act.Should().Throw<ArgumentOutOfRangeException>().WithMessage(
            "Can't get ExploreEducationStatisticsPhaseType for [PhaseOfEducation:Not a phase, TypeOfEstablishment:not an establishment type] (Parameter 'academy')");
    }

    [Fact]
    public void GetFsmUpdated_should_return_data_source()
    {
        var result = _sut.GetFreeSchoolMealsUpdated();
        result.Should().Be(new DataSource(Source.ExploreEducationStatistics, new DateTime(2024, 8, 6),
            UpdateFrequency.Annually));
    }
}

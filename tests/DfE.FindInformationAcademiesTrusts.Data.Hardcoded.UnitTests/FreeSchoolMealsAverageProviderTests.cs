using static DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks.DummyAcademyFactory;

namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded.UnitTests;

public class FreeSchoolMealsAverageProviderTests
{
    private readonly FreeSchoolMealsAverageProvider _sut = new();

    [Theory]
    [InlineData(334, "Secondary", "Pupil Referral Unit", 63.52941176F)]
    [InlineData(372, "Primary", "Community School", 26.26287001F)]
    [InlineData(929, "16 Plus", "Academy Converter", 20.62056176F)]
    [InlineData(931, "All-through", "Free Schools Special", 38.25095057F)]
    public void GetLaAverage_should_return_percentage_from_hardcoded_data(int laCode, string phaseOfEducation,
        string establishmentType, double expected)
    {
        var dummyAcademy = GetDummyAcademy(111, laCode: laCode, phaseOfEducation: phaseOfEducation,
            typeOfEstablishment: establishmentType);
        var result = _sut.GetLaAverage(dummyAcademy);
        result.Should().BeApproximately(expected, 0.01F);
    }

    [Theory]
    [InlineData("Secondary", "Pupil Referral Unit", 57.78940186F)]
    [InlineData("Primary", "Community School", 23.99569177F)]
    [InlineData("16 Plus", "Academy Converter", 22.69174097F)]
    [InlineData("All-through", "Free Schools Special", 45.98689461F)]
    public void GetNationalAverage_should_return_national_percentage_from_hardcoded_data(string phaseOfEducation,
        string establishmentType, double expected)
    {
        var dummyAcademy = GetDummyAcademy(111, phaseOfEducation: phaseOfEducation,
            typeOfEstablishment: establishmentType);
        var result = _sut.GetNationalAverage(dummyAcademy);
        result.Should().BeApproximately(expected, 0.01F);
    }

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
}

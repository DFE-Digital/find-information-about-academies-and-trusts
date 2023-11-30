using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class PupilNumbersModelTests
{
    private readonly PupilNumbersModel _sut;

    public PupilNumbersModelTests()
    {
        var mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new PupilNumbersModel(mockTrustProvider.Object);
    }

    [Fact]
    public void PageTitle_should_be_AcademiesPupilNumbers()
    {
        _sut.PageTitle.Should().Be("Academies pupil numbers");
    }


    [Fact]
    public void TabName_should_be_PupilNumbers()
    {
        _sut.TabName.Should().Be("Pupil numbers");
    }

    [Fact]
    public void PageName_should_be_AcademiesInThisTrust()
    {
        _sut.PageName.Should().Be("Academies in this trust");
    }

    [Theory]
    [InlineData("Primary", 5, 11, "Primary0511")]
    [InlineData("Primary", 5, 9, "Primary0509")]
    [InlineData("Primary", 0, 7, "Primary0007")]
    [InlineData("16 plus", 16, 19, "16 plus1619")]
    [InlineData("Secondary", 10, 18, "Secondary1018")]
    public void PhaseAndAgeRangeSortValue_should_be_amalgamation_of_Phase_and_age_range_properties(string phase,
        int minAge, int maxAge, string expected)
    {
        var ageRange = new AgeRange(minAge, maxAge);
        var dummyAcademy = DummyAcademyFactory.GetDummyAcademy(111, phaseOfEducation: phase, ageRange: ageRange);

        var result = _sut.PhaseAndAgeRangeSortValue(dummyAcademy);
        result.Should().Be(expected);
    }
}

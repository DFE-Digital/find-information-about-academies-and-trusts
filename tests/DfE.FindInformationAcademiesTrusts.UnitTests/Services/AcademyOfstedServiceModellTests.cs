using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Services.Academy;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services;

public class AcademyOfstedServiceModelTests
{
    [Fact]
    public void IsCurrentInspectionAfterJoining_should_be_false_when_CurrentInspection_is_Before_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today.AddYears(-1)), new OfstedRating(1, DateTime.Today.AddYears(-1)));

        sut.IsCurrentInspectionAfterJoining.Should().BeFalse();
    }

    [Fact]
    public void IsCurrentInspectionAfterJoining_should_be_true_when_CurrentInspection_is_After_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today.AddYears(1)), new OfstedRating(1, DateTime.Today.AddYears(1)));

        sut.IsCurrentInspectionAfterJoining.Should().BeTrue();
    }

    [Fact]
    public void IsCurrentInspectionAfterJoining_should_be_true_when_CurrentInspection_is_EqualTo_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today), new OfstedRating(1, DateTime.Today));

        sut.IsCurrentInspectionAfterJoining.Should().BeTrue();
    }

    [Fact]
    public void IsPreviousInspectionAfterJoining_should_be_false_when_PreviousInspection_is_Before_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today.AddYears(-1)), new OfstedRating(1, DateTime.Today.AddYears(-1)));

        sut.IsPreviousInspectionAfterJoining.Should().BeFalse();
    }

    [Fact]
    public void IsPreviousInspectionAfterJoining_should_be_true_when_PreviousInspection_is_After_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today.AddYears(1)), new OfstedRating(1, DateTime.Today.AddYears(1)));

        sut.IsPreviousInspectionAfterJoining.Should().BeTrue();
    }

    [Fact]
    public void IsPreviousInspectionAfterJoining_should_be_true_when_PreviousInspection_is_EqualTo_joining_date()
    {
        var sut = new AcademyOfstedServiceModel("1234", "test", DateTime.Today,
            new OfstedRating(1, DateTime.Today), new OfstedRating(1, DateTime.Today));

        sut.IsPreviousInspectionAfterJoining.Should().BeTrue();
    }
}

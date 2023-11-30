using DfE.FindInformationAcademiesTrusts.Data;
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
}

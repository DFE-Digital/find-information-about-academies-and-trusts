using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class OverviewModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly OverviewModel _sut;


    public OverviewModelTests()
    {
        _mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new OverviewModel(_mockTrustProvider.Object);
    }

    [Fact]
    public void PageName_should_be_Overview()
    {
        _sut.PageName.Should().Be("Overview");
    }
}

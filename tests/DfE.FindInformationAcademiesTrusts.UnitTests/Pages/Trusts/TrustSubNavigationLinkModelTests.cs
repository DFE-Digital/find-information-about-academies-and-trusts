using DfE.FindInformationAcademiesTrusts.Pages.Trusts;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class TrustSubNavigationLinkModelTests
{
    private readonly TrustSubNavigationLinkModel _sut = new("Link Text", "/page", "1234", "Service Name");

    [Fact]
    public void TestId_Should_BeExpected()
    {
        _sut.TestId.Should().Be("service-name-link-text-subnav");
    }
}

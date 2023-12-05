using DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class TrustTests
{
    [Fact]
    public void IsMultiAcademyTrust_should_return_true_if_trust_has_type_multiacademytrust()
    {
        var sut = DummyTrustFactory.GetDummyMultiAcademyTrust("1234");
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Single-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsMultiAcademyTrust_should_return_false_if_trust_does_not_have_type_multiacademytrust(string type)
    {
        var sut = DummyTrustFactory.GetDummyTrust("1234", type);
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeFalse();
    }

    [Fact]
    public void IsSingleAcademyTrust_should_return_true_if_trust_has_type_singleacademytrust()
    {
        var sut = DummyTrustFactory.GetDummySingleAcademyTrust("1234");
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Multi-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsSingleAcademyTrust_should_return_false_if_trust_does_not_have_type_singleacademytrust(string type)
    {
        var sut = DummyTrustFactory.GetDummyTrust("1234", type);
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeFalse();
    }

    [Fact]
    public void IsOpen_should_return_true_if_trust_has_status_open()
    {
        var sut = DummyTrustFactory.GetDummyTrust("1234", status: "Open");
        var result = sut.IsOpen();

        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Closed")]
    [InlineData("test")]
    [InlineData("")]
    public void IsOpen_should_return_false_if_trust_does_not_have_status_open(string status)
    {
        var sut = DummyTrustFactory.GetDummyTrust("1234", status: status);
        var result = sut.IsOpen();
        result.Should().BeFalse();
    }
}

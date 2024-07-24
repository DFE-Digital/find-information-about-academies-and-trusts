using DfE.FindInformationAcademiesTrusts.Data.Dto;

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Dto;

public class TrustDetailsDtoTests
{
    [Fact]
    public void IsMultiAcademyTrust_should_return_true_if_trust_has_type_multiacademytrust()
    {
        var sut = GetTrustDetailsDtoWithType("Multi-academy trust");
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Single-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsMultiAcademyTrust_should_return_false_if_trust_does_not_have_type_multiacademytrust(string type)
    {
        var sut = GetTrustDetailsDtoWithType(type);
        var result = sut.IsMultiAcademyTrust();
        result.Should().BeFalse();
    }

    [Fact]
    public void IsSingleAcademyTrust_should_return_true_if_trust_has_type_singleacademytrust()
    {
        var sut = GetTrustDetailsDtoWithType("Single-academy trust");
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("Multi-academy trust")]
    [InlineData("test")]
    [InlineData("")]
    public void IsSingleAcademyTrust_should_return_false_if_trust_does_not_have_type_singleacademytrust(string type)
    {
        var sut = GetTrustDetailsDtoWithType(type);
        var result = sut.IsSingleAcademyTrust();
        result.Should().BeFalse();
    }

    private static TrustDetailsDto GetTrustDetailsDtoWithType(string trustType)
    {
        return new TrustDetailsDto("", "", "", "", trustType, "", "", null, null);
    }
}

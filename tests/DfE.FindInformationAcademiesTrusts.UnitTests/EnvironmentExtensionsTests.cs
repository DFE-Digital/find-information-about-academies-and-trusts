using Microsoft.AspNetCore.Hosting;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class EnvironmentExtensionsTests
{
    [Fact]
    public void IsLocalDevelopment_should_return_true_when_given_local_development_environment()
    {
        var mockLocalEnv = CreateMockEnvironment("LocalDevelopment");

        mockLocalEnv.Object.IsLocalDevelopment().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Test")]
    [InlineData("Production")]
    [InlineData("CI")]
    public void IsLocalDevelopment_should_return_false_when_given_nonlocal_development_environment(
        string environmentName)
    {
        var mockLocalEnv = CreateMockEnvironment(environmentName);

        mockLocalEnv.Object.IsLocalDevelopment().Should().BeFalse();
    }

    [Fact]
    public void IsContinuousIntegration_should_return_true_when_given_CI_environment()
    {
        var mockLocalEnv = CreateMockEnvironment("CI");

        mockLocalEnv.Object.IsContinuousIntegration().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Test")]
    [InlineData("Production")]
    [InlineData("LocalDevelopment")]
    public void IsContinuousIntegration_should_return_false_when_given_non_CI_environment(
        string environmentName)
    {
        var mockLocalEnv = CreateMockEnvironment(environmentName);

        mockLocalEnv.Object.IsContinuousIntegration().Should().BeFalse();
    }

    private static Mock<IWebHostEnvironment> CreateMockEnvironment(string environmentName)
    {
        var mockLocalEnv = new Mock<IWebHostEnvironment>();
        mockLocalEnv.SetupGet(m => m.EnvironmentName).Returns(environmentName);
        return mockLocalEnv;
    }
}

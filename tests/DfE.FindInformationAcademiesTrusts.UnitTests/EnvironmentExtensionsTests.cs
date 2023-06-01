using Microsoft.AspNetCore.Hosting;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class EnvironmentExtensionsTests
{
    [Fact]
    public void IsLocalDevelopment_should_return_true_when_given_local_development_environment()
    {
        var mockLocalEnv = new Mock<IWebHostEnvironment>();
        mockLocalEnv.SetupGet(m => m.EnvironmentName).Returns("LocalDevelopment");

        mockLocalEnv.Object.IsLocalDevelopment().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Test")]
    [InlineData("Production")]
    public void IsLocalDevelopment_should_return_false_when_given_nonlocal_development_environment(
        string environmentName)
    {
        var mockLocalEnv = new Mock<IWebHostEnvironment>();
        mockLocalEnv.SetupGet(m => m.EnvironmentName).Returns(environmentName);

        mockLocalEnv.Object.IsLocalDevelopment().Should().BeFalse();
    }
}

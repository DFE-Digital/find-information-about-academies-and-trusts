using DfE.FindInformationAcademiesTrusts.Extensions;
using Microsoft.AspNetCore.Hosting;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Extensions;

public class EnvironmentExtensionsTests
{
    [Fact]
    public void IsLocalDevelopment_should_return_true_when_given_local_development_environment()
    {
        var mockWebHostEnvironment = CreateMockEnvironment("LocalDevelopment");

        mockWebHostEnvironment.IsLocalDevelopment().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Test")]
    [InlineData("Production")]
    [InlineData("CI")]
    public void IsLocalDevelopment_should_return_false_when_given_nonlocal_development_environment(
        string environmentName)
    {
        var mockWebHostEnvironment = CreateMockEnvironment(environmentName);

        mockWebHostEnvironment.IsLocalDevelopment().Should().BeFalse();
    }

    [Fact]
    public void IsContinuousIntegration_should_return_true_when_given_CI_environment()
    {
        var mockWebHostEnvironment = CreateMockEnvironment("CI");

        mockWebHostEnvironment.IsContinuousIntegration().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("Test")]
    [InlineData("Production")]
    [InlineData("LocalDevelopment")]
    public void IsContinuousIntegration_should_return_false_when_given_non_CI_environment(
        string environmentName)
    {
        var mockWebHostEnvironment = CreateMockEnvironment(environmentName);

        mockWebHostEnvironment.IsContinuousIntegration().Should().BeFalse();
    }

    [Fact]
    public void IsTest_should_return_true_when_given_Test_environment()
    {
        var mockWebHostEnvironment = CreateMockEnvironment("Test");

        mockWebHostEnvironment.IsTest().Should().BeTrue();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("CI")]
    [InlineData("Production")]
    [InlineData("LocalDevelopment")]
    public void IsTest_should_return_false_when_given_non_Test_environment(
        string environmentName)
    {
        var mockWebHostEnvironment = CreateMockEnvironment(environmentName);

        mockWebHostEnvironment.IsTest().Should().BeFalse();
    }

    [Theory]
    [InlineData("Development")]
    [InlineData("CI")]
    [InlineData("Test")]
    [InlineData("LocalDevelopment")]
    public void IsLiveEnvironment_should_return_false_when_given_specified_nonlive_environment(string environmentName)
    {
        var mockWebHostEnvironment = CreateMockEnvironment(environmentName);

        mockWebHostEnvironment.IsLiveEnvironment().Should().BeFalse();
    }

    [Theory]
    [InlineData("Production")]
    [InlineData("My new environment")]
    [InlineData("ProductionPlus")]
    public void IsLiveEnvironment_should_default_To_true_when_given_production_or_unknown_environment(
        string environmentName)
    {
        var mockWebHostEnvironment = CreateMockEnvironment(environmentName);

        mockWebHostEnvironment.IsLiveEnvironment().Should().BeTrue();
    }

    private static IWebHostEnvironment CreateMockEnvironment(string environmentName)
    {
        var mockWebHostEnvironment = Substitute.For<IWebHostEnvironment>();
        mockWebHostEnvironment.EnvironmentName.Returns(environmentName);
        return mockWebHostEnvironment;
    }
}

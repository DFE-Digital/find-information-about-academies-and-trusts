using DfE.FindInformationAcademiesTrusts.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Net.Http.Headers;
using Moq;
using System.Collections.Generic;

namespace ConcernsCaseWork.Tests.Authorization
{
	public class AutomationHandlerTests
	{

   

    [Theory]
    [InlineData("Development",true)]
    [InlineData("Staging",true)]
    [InlineData("Production",false)]

    public static void Validate_Environment(string environment, bool expected)
    {
            IHostEnvironment hostEnvironment = new HostingEnvironment()
			{
				EnvironmentName = environment
			};

            var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers.Add(HeaderNames.Authorization, "Bearer 123");

            Mock<IHttpContextAccessor> mockHttpAccessor = new Mock<IHttpContextAccessor>();
			mockHttpAccessor.Setup(m => m.HttpContext).Returns(httpContext);

            var configurationSettings = new Dictionary<string, string?>()
			{
				{ "PlaywrightTestSecret","123" }
			};


            IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(configurationSettings)
				.Build();

            var result = HeaderRequirementHandler.ClientSecretHeaderValid(hostEnvironment, mockHttpAccessor.Object, configuration);

			result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("123", null)]
    [InlineData("123", "456")]
    [InlineData("", "")]

    public void Validate_AuthKey(string headerAuthKey, string serverAuthKey)
    {
            IHostEnvironment hostEnvironment = new HostingEnvironment()
			{
				EnvironmentName = Environments.Development
			};

			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {headerAuthKey}");

			Mock<IHttpContextAccessor> mockHttpAccessor = new Mock<IHttpContextAccessor>();
			mockHttpAccessor.Setup(m => m.HttpContext).Returns(httpContext);

			var configurationSettings = new Dictionary<string, string?>()
			{
				{ "PlaywrightTestSecret", serverAuthKey }
			};

			IConfiguration configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(configurationSettings)
				.Build();

			var result = HeaderRequirementHandler.ClientSecretHeaderValid(hostEnvironment, mockHttpAccessor.Object, configuration);

			result.Should().BeFalse();
    }


    

    }
}
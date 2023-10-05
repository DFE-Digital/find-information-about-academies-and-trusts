using DfE.FindInformationAcademiesTrusts;
using DfE.FindInformationAcademiesTrusts.Authorization;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
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
    [InlineData("LocalDevelopment",true)]
	[InlineData("CI",true)]
    [InlineData("Production",false)]

    public static void Validate_Environment(string environment, bool expected)
    {
           var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
				mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns(environment);

            var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers.Add(HeaderNames.Authorization, "Bearer 123");

            Mock<IHttpContextAccessor> mockHttpAccessor = new Mock<IHttpContextAccessor>();
			mockHttpAccessor.Setup(m => m.HttpContext).Returns(httpContext);

            var configurationSettings = new TestOverrideOptions()
			{
				PlaywrightTestSecret = "123"
			};


			var sut = new HeaderRequirementHandler(mockWebHostEnvironment.Object,mockHttpAccessor.Object,configurationSettings);	

            var result = sut.ClientSecretHeaderValid();

			result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("123", null)]
    [InlineData("123", "456")]
    [InlineData("", "")]

    public void ClientSecretHeaderValid_Should_Return_False_if_Contents_Are_Wrong(string headerAuthKey, string serverAuthKey)
    {
           var mockWebHostEnvironment = new Mock<IWebHostEnvironment>();
				mockWebHostEnvironment.SetupGet(m => m.EnvironmentName).Returns("Development");

			var httpContext = new DefaultHttpContext();
			httpContext.Request.Headers.Add(HeaderNames.Authorization, $"Bearer {headerAuthKey}");

			Mock<IHttpContextAccessor> mockHttpAccessor = new Mock<IHttpContextAccessor>();
			mockHttpAccessor.Setup(m => m.HttpContext).Returns(httpContext);

		    var configurationSettings = new TestOverrideOptions()
			{
				PlaywrightTestSecret = serverAuthKey
			};

			var sut = new HeaderRequirementHandler(mockWebHostEnvironment.Object,mockHttpAccessor.Object,configurationSettings);	

            var result = sut.ClientSecretHeaderValid();

			result.Should().BeFalse();
    }


    

    }
}
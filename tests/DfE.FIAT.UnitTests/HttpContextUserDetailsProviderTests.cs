using DfE.FIAT.UnitTests.Mocks;
using DfE.FIAT.Web;
using Microsoft.AspNetCore.Http;

namespace DfE.FIAT.UnitTests;

public class HttpContextUserDetailsProviderTests
{
    private readonly MockHttpContext _mockHttpContext = new();
    private HttpContextUserDetailsProvider _sut;

    public HttpContextUserDetailsProviderTests()
    {
        Mock<IHttpContextAccessor> mockHttpAccessor = new();
        mockHttpAccessor.Setup(m => m.HttpContext).Returns(_mockHttpContext.Object);

        _sut = new HttpContextUserDetailsProvider(mockHttpAccessor.Object);
    }

    [Fact]
    public void GetUserDetails_should_throw_if_there_is_no_httpcontext()
    {
        _sut = new HttpContextUserDetailsProvider(Mock.Of<IHttpContextAccessor>());

        var action = () => _sut.GetUserDetails();

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUserDetails_should_throw_if_preferred_username_claim_is_missing()
    {
        _mockHttpContext.AddUserClaim("name", "SURNAME, Forename");

        var action = () => _sut.GetUserDetails();

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUserDetails_should_throw_if_name_claim_is_missing()
    {
        _mockHttpContext.AddUserClaim("preferred_username", "user@email.com");

        var action = () => _sut.GetUserDetails();

        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetUserDetails_should_set_email_from_user_claims()
    {
        _mockHttpContext.AddUserClaim("name", "SURNAME, Forename");
        _mockHttpContext.AddUserClaim("preferred_username", "user@email.com");

        var result = _sut.GetUserDetails();

        result.Email.Should().Be("user@email.com");
    }

    [Theory]
    [InlineData("don't change me", "don't change me")]
    [InlineData("McGREGOR, Donald", "Donald McGREGOR")]
    [InlineData("SURNAME, Mary-Jane", "Mary-Jane SURNAME")]
    [InlineData("SURNAME, Forename", "Forename SURNAME")]
    public void GetUserDetails_should_set_name_from_user_claims(string claimValue, string expectedName)
    {
        _mockHttpContext.AddUserClaim("name", claimValue);
        _mockHttpContext.AddUserClaim("preferred_username", "user@email.com");

        var result = _sut.GetUserDetails();

        result.Name.Should().Be(expectedName);
    }
}

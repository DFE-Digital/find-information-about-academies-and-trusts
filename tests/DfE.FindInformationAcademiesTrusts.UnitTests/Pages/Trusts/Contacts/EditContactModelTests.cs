using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public class EditContactModelTests
{
    private readonly EditContactModel _sut;
    private readonly Mock<ITrustService> _mockTrustService = new();

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    public EditContactModelTests()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync("1234")).ReturnsAsync(
            new TrustContactsServiceModel(null, null, null, null, null));
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_fakeTrust.Uid))
            .ReturnsAsync(_fakeTrust);

        _sut = new EditSfsoLeadModel(MockDataSourceService.CreateSubstitute(),
                MockLogger.CreateLogger<EditSfsoLeadModel>(), _mockTrustService.Object)
            { Uid = "1234" };
    }

    [Fact]
    public void GetErrorClass_returns_the_class_string_when_validation_is_incorrect()
    {
        _sut.ModelState.AddModelError("Test", "Test");
        var result = _sut.GetErrorClass("Test");
        result.Should().Be("govuk-form-group--error");
    }

    [Theory]
    [InlineData("")]
    [InlineData("Name")]
    [InlineData("Email")]
    public void GetErrorClass_returns_empty_string_when_validation_is_correct(string key)
    {
        var result = _sut.GetErrorClass(key);
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void GenerateErrorAriaDescribedBy_returns_the_correct_string_when_validation_is_incorrect()
    {
        _sut.ModelState.AddModelError("Test", "Test");
        _sut.ModelState.AddModelError("Test", "Test1");
        var result = _sut.GenerateErrorAriaDescribedBy("Test");
        result.Should().Be("error-Test-0 error-Test-1");
    }

    [Fact]
    public void GenerateErrorAriaDescribedBy_returns_empty_when_validation_is_correct()
    {
        var result = _sut.GenerateErrorAriaDescribedBy("Test");
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void GetErrorList_returns_the_correct_list_when_validation_is_incorrect()
    {
        _sut.ModelState.AddModelError("Test", "Test");
        var result = _sut.GetErrorList("Test");
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo([(0, "Test")]);
    }

    [Fact]
    public void GetErrorList_returns_an_empty_list_when_validation_is_correct()
    {
        var result = _sut.GetErrorList("Test");
        result.Should().HaveCount(0);
        result.Should().BeEquivalentTo(Array.Empty<(int, string)>());
    }
}

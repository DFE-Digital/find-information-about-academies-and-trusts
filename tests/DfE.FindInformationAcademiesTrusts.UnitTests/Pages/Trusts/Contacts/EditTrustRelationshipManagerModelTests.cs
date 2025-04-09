using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public class EditTrustRelationshipManagerModelTests
{
    private readonly EditTrustRelationshipManagerModel _sut;
    private const string TrustRelationShipManagerDisplayName = "Trust relationship manager";
    
    private readonly ITrustService _mockTrustService = Substitute.For<ITrustService>();

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    private readonly InternalContact _trustRelationshipManager =
        new("Trust Relationship Manager", "trm@test.com", DateTime.Today, "test@email.com");

    public EditTrustRelationshipManagerModelTests()
    {
        _mockTrustService.GetTrustContactsAsync("1234").Returns(
            Task.FromResult(new TrustContactsServiceModel(_trustRelationshipManager, null, null, null, null)));
        _mockTrustService.GetTrustSummaryAsync(_fakeTrust.Uid)!.Returns(Task.FromResult(_fakeTrust));

        _sut = new EditTrustRelationshipManagerModel(MockDataSourceService.CreateSubstitute(),
                MockLogger.CreateLogger<EditTrustRelationshipManagerModel>(), _mockTrustService)
            { Uid = "1234" };
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.GetTrustSummaryAsync("1234").Returns(Task.FromResult<TrustSummaryServiceModel?>(null));
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_loads_the_correct_name_and_email()
    {
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<PageResult>();
        _sut.Name.Should().Be(_trustRelationshipManager.FullName);
        _sut.Email.Should().Be(_trustRelationshipManager.Email);
    }

    [Theory]
    [InlineData(true, true,
        $"Changes made to the {TrustRelationShipManagerDisplayName} name and email were updated.")]
    [InlineData(true, false,
        $"Changes made to the {TrustRelationShipManagerDisplayName} name were updated.")]
    [InlineData(false, true,
        $"Changes made to the {TrustRelationShipManagerDisplayName} email were updated.")]
    [InlineData(false, false, "")]
    public async Task OnPostAsync_sets_ContactUpdated_to_true_when_validation_is_correct(bool nameUpdated,
        bool emailUpdated, string expectedMessage)
    {
        _sut.TrustSummary = _fakeTrust;
        _mockTrustService
            .UpdateContactAsync(1234, Arg.Any<string>(), Arg.Any<string>(),
                ContactRole.TrustRelationshipManager)
            .Returns(Task.FromResult(new TrustContactUpdatedServiceModel(emailUpdated, nameUpdated)));

        var result = await _sut.OnPostAsync();

        _sut.ContactUpdatedMessage.Should().Be(expectedMessage);

        result.Should().BeOfType<RedirectToPageResult>()
            .Which.PageName.Should().Be("/Trusts/Contacts/InDfe");
    }

    [Fact]
    public async Task OnPostAsync_sets_ContactUpdated_to_false_when_validation_is_incorrect()
    {
        _sut.ModelState.AddModelError("Test", "Test");

        var result = await _sut.OnPostAsync();

        result.Should().BeOfType<PageResult>();
        _sut.ContactUpdatedMessage.Should().Be(string.Empty);
    }

    [Fact]
    public async Task OnGetAsync_should_configure_TrustPageMetadata()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Edit Trust relationship manager details");
        _sut.TrustPageMetadata.PageName.Should().Be(ViewConstants.ContactsPageName);
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
    }

    [Fact]
    public async Task OnPostAsync_should_configure_TrustPageMetadata_when_model_is_valid()
    {
        _sut.TrustSummary = _fakeTrust;
        _mockTrustService
            .UpdateContactAsync(1234, Arg.Any<string>(), Arg.Any<string>(),
                ContactRole.TrustRelationshipManager)
            .Returns(Task.FromResult(new TrustContactUpdatedServiceModel(true, true)));
        _ = await _sut.OnPostAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Edit Trust relationship manager details");
        _sut.TrustPageMetadata.PageName.Should().Be("Contacts");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
        _sut.TrustPageMetadata.ModelStateIsValid.Should().BeTrue();
    }

    [Fact]
    public async Task OnPostAsync_should_configure_TrustPageMetadata_when_model_is_not_valid()
    {
        _sut.ModelState.AddModelError("Test", "Test");
        _ = await _sut.OnPostAsync();

        _sut.TrustPageMetadata.SubPageName.Should().Be("Edit Trust relationship manager details");
        _sut.TrustPageMetadata.PageName.Should().Be("Contacts");
        _sut.TrustPageMetadata.TrustName.Should().Be("My Trust");
        _sut.TrustPageMetadata.ModelStateIsValid.Should().BeFalse();
    }
}

using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Contacts;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Contacts;

public class EditSfsoLeadModelTests
{
    private readonly EditSfsoLeadModel _sut;
    private readonly Mock<ITrustProvider> _mockTrustProvider = new();

    private readonly MockDataSourceService _mockDataSourceService = new();
    private readonly Mock<ITrustService> _mockTrustService = new();

    private readonly TrustSummaryServiceModel _fakeTrust = new("1234", "My Trust", "Multi-academy trust", 3);

    private readonly Person _sfsoLead = new("Sfso Lead", "sfso.lead@test.com");

    public EditSfsoLeadModelTests()
    {
        _mockTrustService.Setup(tp => tp.GetTrustContactsAsync("1234")).ReturnsAsync(
            new TrustContactsServiceModel(null, _sfsoLead, null, null, null));
        _mockTrustService.Setup(t => t.GetTrustSummaryAsync(_fakeTrust.Uid))
            .ReturnsAsync(_fakeTrust);

        _sut = new EditSfsoLeadModel(_mockTrustProvider.Object, _mockDataSourceService.Object,
                new MockLogger<EditSfsoLeadModel>().Object, _mockTrustService.Object)
            { Uid = "1234" };
    }

    [Fact]
    public void PageName_should_be_correct()
    {
        _sut.PageName.Should().Be("Edit SFSO (Schools financial support and oversight) lead");
    }

    [Fact]
    public async Task OnGetAsync_returns_NotFoundResult_if_Trust_is_not_found()
    {
        _mockTrustService.Setup(r => r.GetTrustSummaryAsync("1234")).ReturnsAsync((TrustSummaryServiceModel?)null);
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task OnGetAsync_loads_the_correct_name_and_email()
    {
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<PageResult>();
        _sut.Name.Should().Be(_sfsoLead.FullName);
        _sut.Email.Should().Be(_sfsoLead.Email);
    }
}
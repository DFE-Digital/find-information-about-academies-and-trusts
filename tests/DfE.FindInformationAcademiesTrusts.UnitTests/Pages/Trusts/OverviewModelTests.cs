﻿using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts;

public class OverviewModelTests
{
    private readonly Mock<ITrustProvider> _mockTrustProvider;
    private readonly OverviewModel _sut;
    private readonly DummyTrustFactory _dummyTrustFactory;


    public OverviewModelTests()
    {
        _dummyTrustFactory = new DummyTrustFactory();
        _mockTrustProvider = new Mock<ITrustProvider>();
        _sut = new OverviewModel(_mockTrustProvider.Object);
    }

    [Fact]
    public async void OnGetAsync_should_fetch_a_trust_by_ukprn()
    {
        var dummyTrust = _dummyTrustFactory.GetDummyTrust("1234");
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync(dummyTrust.Uid))
            .ReturnsAsync(dummyTrust);
        _sut.Uid = dummyTrust.Uid;

        await _sut.OnGetAsync();
        _sut.Trust.Should().Be(dummyTrust);
    }

    [Fact]
    public async void Ukprn_should_be_empty_string_by_default()
    {
        await _sut.OnGetAsync();
        _sut.Uid.Should().BeEquivalentTo(string.Empty);
    }

    [Fact]
    public void PageName_should_be_Overview()
    {
        _sut.PageName.Should().Be("Overview");
    }

    [Fact]
    public void PageSection_should_be_AboutTheTrust()
    {
        _sut.Section.Should().Be("About the trust");
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_trust_is_not_found()
    {
        _mockTrustProvider.Setup(s => s.GetTrustByUidAsync("1111"))
            .ReturnsAsync((Trust?)null);

        _sut.Uid = "1111";
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void OnGetAsync_should_return_not_found_result_if_Ukprn_is_not_provided()
    {
        var result = await _sut.OnGetAsync();
        result.Should().BeOfType<NotFoundResult>();
    }
}

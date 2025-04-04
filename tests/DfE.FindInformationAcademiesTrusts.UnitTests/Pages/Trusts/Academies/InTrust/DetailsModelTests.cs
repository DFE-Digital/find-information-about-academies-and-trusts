using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.InTrust;

public class AcademiesDetailsModelTests : AcademiesInTrustAreaModelTests<AcademiesInTrustDetailsModel>
{
    private readonly IOtherServicesLinkBuilder _mockLinkBuilder = Substitute.For<IOtherServicesLinkBuilder>();

    public AcademiesDetailsModelTests()
    {
        Sut = new AcademiesInTrustDetailsModel(MockDataSourceService,
                _mockLinkBuilder,
                MockLogger.CreateLogger<AcademiesInTrustDetailsModel>(),
                MockTrustService,
                MockAcademyService,
                MockExportService,
                MockDateTimeProvider)
            { Uid = TrustUid };
    }

    [Fact]
    public void OtherServicesLinkBuilder_should_be_injected()
    {
        Sut.LinkBuilder.Should().Be(_mockLinkBuilder);
    }

    [Fact]
    public override async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyDetailsServiceModel("1", "", "", "", ""),
            new AcademyDetailsServiceModel("2", "", "", "", ""),
            new AcademyDetailsServiceModel("3", "", "", "", "")
        };
        MockAcademyService.GetAcademiesInTrustDetailsAsync(Sut.Uid).Returns(Task.FromResult(academies));

        _ = await Sut.OnGetAsync();

        Sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesInTrustDetailsPageName);
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_TabList_to_current_tab()
    {
        _ = await Sut.OnGetAsync();

        Sut.TabList.Should().ContainSingle(l => l.LinkIsActive)
            .Which.TabPageLink.Should().Be("./Details");
    }
}

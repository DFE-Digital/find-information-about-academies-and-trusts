using DfE.FindInformationAcademiesTrusts.Pages;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class AcademiesDetailsModelTests : BaseAcademiesPageModelTests<AcademiesDetailsModel>
{
    private readonly Mock<IOtherServicesLinkBuilder> _mockLinkBuilder = new();
    private readonly Mock<IAcademyService> _mockAcademyService = new();

    public AcademiesDetailsModelTests()
    {
        Sut = new AcademiesDetailsModel(MockDataSourceService.Object, _mockLinkBuilder.Object,
                new MockLogger<AcademiesDetailsModel>().Object, MockTrustService.Object, _mockAcademyService.Object,
                MockExportService.Object, MockDateTimeProvider.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void OtherServicesLinkBuilder_should_be_injected()
    {
        Sut.LinkBuilder.Should().Be(_mockLinkBuilder.Object);
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyDetailsServiceModel("1", "", "", "", ""),
            new AcademyDetailsServiceModel("2", "", "", "", ""),
            new AcademyDetailsServiceModel("3", "", "", "", "")
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustDetailsAsync(Sut.Uid))
            .ReturnsAsync(academies);

        _ = await Sut.OnGetAsync();

        Sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesDetailsPageName);
    }
}

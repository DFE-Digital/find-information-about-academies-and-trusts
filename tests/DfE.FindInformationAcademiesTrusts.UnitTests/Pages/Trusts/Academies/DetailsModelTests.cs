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
        _sut = new AcademiesDetailsModel(_mockDataSourceService.Object, _mockLinkBuilder.Object,
                new MockLogger<AcademiesDetailsModel>().Object, _mockTrustService.Object, _mockAcademyService.Object,
                _mockExportService.Object, _mockDateTimeProvider.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public void OtherServicesLinkBuilder_should_be_injected()
    {
        _sut.LinkBuilder.Should().Be(_mockLinkBuilder.Object);
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
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustDetailsAsync(_sut.Uid))
            .ReturnsAsync(academies);

        _ = await _sut.OnGetAsync();

        _sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName()
    {
        _ = await _sut.OnGetAsync();

        _sut.TrustPageMetadata.TabName.Should().Be(ViewConstants.AcademiesDetailsPageName);
    }
}

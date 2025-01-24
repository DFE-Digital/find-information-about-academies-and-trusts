using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies;

public class FreeSchoolMealsModelTests : BaseAcademiesPageModelTests<FreeSchoolMealsModel>
{
    private readonly Mock<IAcademyService> _mockAcademyService = new();

    public FreeSchoolMealsModelTests()
    {
        Sut = new FreeSchoolMealsModel(
                MockDataSourceService.Object, new MockLogger<FreeSchoolMealsModel>().Object,
                MockTrustService.Object, _mockAcademyService.Object, MockExportService.Object,
                MockDateTimeProvider.Object)
            { Uid = TrustUid };
    }

    [Fact]
    public async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academies = new[]
        {
            new AcademyFreeSchoolMealsServiceModel("1", "Academy 1", 12.5, 13.5, 14.5),
            new AcademyFreeSchoolMealsServiceModel("2", "Academy 2", null, 70.1, 64.1),
            new AcademyFreeSchoolMealsServiceModel("3", "Academy 3", 8.2, 4, 10)
        };
        _mockAcademyService.Setup(a => a.GetAcademiesInTrustFreeSchoolMealsAsync(Sut.Uid))
            .ReturnsAsync(academies);

        _ = await Sut.OnGetAsync();

        Sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.TabName.Should().Be("Free school meals");
    }
}

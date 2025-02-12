using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Trusts.Academies.InTrust;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Pages.Trusts.Academies.InTrust;

public class PupilNumbersModelTests : AcademiesInTrustAreaModelTests<PupilNumbersModel>
{
    public PupilNumbersModelTests()
    {
        Sut = new PupilNumbersModel(MockDataSourceService.Object,
                new MockLogger<PupilNumbersModel>().Object,
                MockTrustService.Object,
                MockAcademyService.Object,
                MockExportService.Object,
                MockDateTimeProvider.Object,
                MockFeatureManager.Object)
            { Uid = TrustUid };
    }

    [Theory]
    [InlineData("Primary", 5, 11, "Primary0511")]
    [InlineData("Primary", 5, 9, "Primary0509")]
    [InlineData("Primary", 0, 7, "Primary0007")]
    [InlineData("16 plus", 16, 19, "16 plus1619")]
    [InlineData("Secondary", 10, 18, "Secondary1018")]
    public void PhaseAndAgeRangeSortValue_should_be_amalgamation_of_Phase_and_age_range_properties(string phase,
        int minAge, int maxAge, string expected)
    {
        var ageRange = new AgeRange(minAge, maxAge);
        var testAcademyUrn = "1234";
        var testAcademyName = "Test Academy";
        var testAcademyNumberOfPupils = 100;
        var testAcademySchoolCapacity = 100;

        var result = PupilNumbersModel.PhaseAndAgeRangeSortValue(new AcademyPupilNumbersServiceModel(testAcademyUrn,
            testAcademyName, phase, ageRange,
            testAcademyNumberOfPupils, testAcademySchoolCapacity));
        result.Should().Be(expected);
    }

    [Fact]
    public override async Task OnGetAsync_sets_academies_from_academyService()
    {
        var academy = new AcademyPupilNumbersServiceModel("", null, null, new AgeRange(5, 11), null, null);
        var academies = new[]
        {
            academy with { Urn = "1" },
            academy with { Urn = "2" },
            academy with { Urn = "3" }
        };
        MockAcademyService.Setup(a => a.GetAcademiesInTrustPupilNumbersAsync(TrustUid))
            .ReturnsAsync(academies);

        _ = await Sut.OnGetAsync();

        Sut.Academies.Should().BeEquivalentTo(academies);
    }

    [Fact]
    public override async Task OnGetAsync_should_configure_TrustPageMetadata_TabPageName()
    {
        _ = await Sut.OnGetAsync();

        Sut.TrustPageMetadata.TabName.Should().Be("Pupil numbers");
    }

    [Fact]
    public override async Task OnGetAsync_should_set_active_TabList_to_current_tab()
    {
        _ = await Sut.OnGetAsync();

        Sut.TabList.Should().ContainSingle(l => l.LinkIsActive)
            .Which.SubPageLink.Should().Be("./PupilNumbers");
    }
}

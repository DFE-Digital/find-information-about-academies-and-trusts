using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using DfE.FindInformationAcademiesTrusts.Services.School;
using NSubstitute.ReturnsExtensions;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Services
{
    public class SchoolOverviewDetailsServiceTests
    {
        private readonly int _laMaintainedSchoolUrn = 123;
        private readonly int _academySchoolUrn = 678;

        private readonly SchoolOverviewDetailsService _sut;
        private readonly ISchoolRepository _mockSchoolRepository = Substitute.For<ISchoolRepository>();

        private readonly SchoolDetails laMaintainedSchoolDetails = new("Cool school",
            "some address", "yorkshire", "leeds", "secondary", new AgeRange(2, 6), "none");

        private static readonly SchoolDetails academySchoolDetails = new("Cool academy",
            "some address", "yorkshire", "leeds", "secondary", new AgeRange(2, 6), "none");


        public SchoolOverviewDetailsServiceTests()
        {
            _sut = new SchoolOverviewDetailsService(_mockSchoolRepository);
        }

        [Fact]
        public async Task GetSchoolOverviewDetailsAsync_should_return_null_if_details_not_found()
        {
            _mockSchoolRepository.GetSchoolDetailsAsync(_academySchoolUrn).ReturnsNull();

            var result = await _sut.GetSchoolOverviewDetailsAsync(_academySchoolUrn, SchoolCategory.Academy);

            result.Should().BeNull();
        }

        [Fact]
        public async Task If_school_is_la_maintained_should_not_get_date_joined_trust()
        {
            _mockSchoolRepository.GetSchoolDetailsAsync(_laMaintainedSchoolUrn).Returns(laMaintainedSchoolDetails);

            var result = await _sut.GetSchoolOverviewDetailsAsync(_laMaintainedSchoolUrn, SchoolCategory.LaMaintainedSchool);

            result.Should().NotBeNull();
            result!.DateJoinedTrust.Should().BeNull();
            await _mockSchoolRepository.Received(0).GetDateJoinedTrustAsync(_laMaintainedSchoolUrn);
        }

        [Fact]
        public async Task If_school_is_academy_should_return_with_date_joined_trust()
        {
            _mockSchoolRepository.GetSchoolDetailsAsync(_academySchoolUrn).Returns(academySchoolDetails);
            _mockSchoolRepository.GetDateJoinedTrustAsync(_academySchoolUrn).Returns(new DateOnly(2024, 01, 25));

            var result = await _sut.GetSchoolOverviewDetailsAsync(_academySchoolUrn, SchoolCategory.Academy);

            result.Should().NotBeNull();
            result!.DateJoinedTrust.Should().NotBeNull();
        }

        public static TheoryData<SchoolDetails, NurseryProvision> expectedResults => new()
        {
            { academySchoolDetails with {NurseryProvision = ""}, NurseryProvision.NotRecorded },
            { academySchoolDetails with {NurseryProvision = "has nursery classes"}, NurseryProvision.HasClasses },
            { academySchoolDetails with {NurseryProvision = "haS Nursery classes"}, NurseryProvision.HasClasses },
            { academySchoolDetails with {NurseryProvision = "no nursery classes"}, NurseryProvision.NoClasses },
            { academySchoolDetails with {NurseryProvision = "No Nursery Classes"}, NurseryProvision.NoClasses },
            { academySchoolDetails with {NurseryProvision = "not recorded"}, NurseryProvision.NotRecorded }
        };

        [Theory, MemberData(nameof(expectedResults))]
        public async Task Should_return_correct_nursery_provision(SchoolDetails schoolDetails, NurseryProvision expectedNurseryProvision)
        {
            _mockSchoolRepository.GetSchoolDetailsAsync(_academySchoolUrn).Returns(schoolDetails);

            var result = await _sut.GetSchoolOverviewDetailsAsync(_academySchoolUrn, SchoolCategory.Academy);

            result?.NurseryProvision.Should().Be(expectedNurseryProvision);
        }

    }
}

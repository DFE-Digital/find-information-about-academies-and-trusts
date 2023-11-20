using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class GovernorFactoryTests
{
    private readonly GovernorFactory _sut = new();

    [Fact]
    public void CreateFrom_should_transform_a_governance_into_a_governor()
    {
        var governance = new Governance
        {
            Gid = "1011111",
            Uid = "1234",
            AppointingBody = "Appointed by GB/board",
            DateOfAppointment = "01/09/2023",
            DateTermOfOfficeEndsEnded = "01/09/2024",
            Forename1 = "Oliver",
            Forename2 = "Jane",
            Surname = "Wood",
            Role = "Trustee",
            Title = "Mr"
        };

        var result = _sut.CreateFrom(governance);

        result.Should().BeEquivalentTo(new Governor(
                "1011111",
                "1234",
                "Oliver Jane Wood",
                "Trustee",
                "Appointed by GB/board",
                new DateTime(2023, 09, 01),
                new DateTime(2024, 09, 01),
                null
            )
        );
    }

    [Theory]
    [InlineData("Oliver", "", "Wood", "Oliver Wood")]
    [InlineData("Oliver", "", "", "Oliver")]
    [InlineData("Oliver", "James", "Wood", "Oliver James Wood")]
    public void GetFullName_should_return_full_name(string forename1, string forename2, string surname,
        string expectedFullName)
    {
        var governance = new Governance
        {
            Gid = "1011111",
            Uid = "1234",
            AppointingBody = "Appointed by GB/board",
            DateOfAppointment = "01/09/2023",
            DateTermOfOfficeEndsEnded = "01/09/2024",
            Forename1 = forename1,
            Forename2 = forename2,
            Surname = surname,
            Role = "Trustee",
            Title = "not used"
        };

        var result = _sut.CreateFrom(governance);

        result.FullName.Should().Be(expectedFullName);
    }
}

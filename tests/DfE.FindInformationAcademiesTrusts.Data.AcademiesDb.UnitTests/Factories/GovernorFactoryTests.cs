using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class GovernorFactoryTests
{
    private readonly GovernorFactory _sut = new();

    private readonly MstrTrustGovernance _mstrTrustGovernance = new()
    {
        Gid = "1011111",
        Email = "ms.governor@thetrust.com"
    };

    [Fact]
    public void CreateFrom_should_transform_a_giasGovernance_into_a_governor()
    {
        var giasGovernance = new GiasGovernance
        {
            Gid = "1011111",
            Uid = "1234",
            AppointingBody = "Appointed by GB/board",
            DateOfAppointment = "01/09/2023",
            DateTermOfOfficeEndsEnded = "01/09/2024",
            Role = "Trustee"
        };

        var result = _sut.CreateFrom(giasGovernance, _mstrTrustGovernance);

        result.GID.Should().Be("1011111");
        result.UID.Should().Be("1234");
        result.Role.Should().Be(GovernanceRole.Trustee);
        result.AppointingBody.Should().Be("Appointed by GB/board");
        result.DateOfAppointment.Should().Be(new DateTime(2023, 09, 01));
        result.DateOfTermEnd.Should().Be(new DateTime(2024, 09, 01));
    }

    [Theory]
    [InlineData("ms.governor@thetrust.com")]
    [InlineData("")]
    public void CreateFrom_should_use_email_from_mstrTrustGovernance(string email)
    {
        var giasGovernance = new GiasGovernance
        {
            Gid = "1011111",
            Uid = "1234"
        };
        var mstrTrustGovernance = new MstrTrustGovernance
        {
            Gid = "1011111",
            Email = email
        };

        var result = _sut.CreateFrom(giasGovernance, mstrTrustGovernance);

        result.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("Oliver", "", "Wood", "Oliver Wood")]
    [InlineData("Oliver", "", "", "Oliver")]
    [InlineData("Oliver", "James", "Wood", "Oliver James Wood")]
    public void GetFullName_should_return_full_name(string forename1, string forename2, string surname,
        string expectedFullName)
    {
        var giasGovernance = new GiasGovernance
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

        var result = _sut.CreateFrom(giasGovernance, _mstrTrustGovernance);

        result.FullName.Should().Be(expectedFullName);
    }
}

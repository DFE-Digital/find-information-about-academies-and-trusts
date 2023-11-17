using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Factories;

public class TrustFactoryTests
{
    private readonly TrustFactory _sut = new();

    private readonly Group _testGroup = new()
    {
        GroupName = "trust 1", GroupUid = "1234", GroupType = "Multi-academy trust", Ukprn = "my ukprn",
        GroupId = "my groupId",
        GroupContactStreet = "12 Abbey Road",
        GroupContactLocality = "Dorthy Inlet",
        GroupContactTown = "East Park",
        GroupContactPostcode = "JY36 9VC",
        IncorporatedOnOpenDate = "20/12/1990",
        CompaniesHouseNumber = "00123444"
    };

    [Fact]
    public void CreateTrustFrom_should_transform_a_group_and_mstrTrust_into_a_trust()
    {
        var mstrTrust = new MstrTrust
        {
            GroupUid = "1234", GORregion = "North East"
        };

        var result = _sut.CreateTrustFrom(_testGroup, mstrTrust, Array.Empty<Academy>(), Array.Empty<Governor>());

        result.Should().BeEquivalentTo(new Trust(
                "1234",
                "trust 1",
                "my groupId",
                "my ukprn",
                "Multi-academy trust",
                "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC",
                new DateTime(1990, 12, 20),
                "00123444",
                "North East",
                Array.Empty<Academy>(),
                Array.Empty<Governor>(),
                null,
                null
            )
        );
    }

    [Fact]
    public void CreateTrustFrom_should_transform_a_group_without_mstrTrust_into_a_trust()
    {
        var result = _sut.CreateTrustFrom(_testGroup, null, Array.Empty<Academy>(), Array.Empty<Governor>());

        result.Should().BeEquivalentTo(new Trust(
                "1234",
                "trust 1",
                "my groupId",
                "my ukprn",
                "Multi-academy trust",
                "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC",
                new DateTime(1990, 12, 20),
                "00123444",
                "",
                Array.Empty<Academy>(),
                Array.Empty<Governor>(),
                null,
                null
            )
        );
    }

    [Fact]
    public void CreateTrustFrom_should_set_academies_from_parameters()
    {
        var academies = new[] { DummyAcademyFactory.GetDummyAcademy(1234546) };

        var result = _sut.CreateTrustFrom(_testGroup, null, academies, Array.Empty<Governor>());

        result.Academies.Should().Equal(academies);
    }

    [Fact]
    public void CreateTrustFrom_should_set_governors_from_parameters()
    {
        var governors = new[] { DummyGovernorFactory.GetDummyGovernor("1234546", "1234") };

        var result = _sut.CreateTrustFrom(_testGroup, null, Array.Empty<Academy>(), governors);

        result.Governors.Should().Equal(governors);
    }

    [Theory]
    [MemberData(nameof(EmptyData))]
    public void CreateTrustFromGroup_Should_Include_Empty_string_values_if_properties_have_no_value(
        Group group, MstrTrust mstrTrust, Trust expected)
    {
        var result = _sut.CreateTrustFrom(group, mstrTrust, Array.Empty<Academy>(), Array.Empty<Governor>());
        result.Should().BeEquivalentTo(expected);
    }

    public static IEnumerable<object[]> EmptyData =>
        new List<object[]>
        {
            new object[]
            {
                new Group
                {
                    GroupUid = "1", GroupName = "", GroupId = "", GroupType = "", IncorporatedOnOpenDate = "",
                    CompaniesHouseNumber = ""
                },
                new MstrTrust
                {
                    GroupUid = "1", GORregion = ""
                },
                new Trust("1", "", "", null, "", "", null, "", "", Array.Empty<Academy>(),
                    Array.Empty<Governor>(),
                    null,
                    null)
            },
            new object[]
            {
                new Group { GroupUid = "2" },
                new MstrTrust { GroupUid = "2" },
                new Trust("2", "", "", null, "", "", null, "", "", Array.Empty<Academy>(),
                    Array.Empty<Governor>(),
                    null,
                    null)
            }
        };
}

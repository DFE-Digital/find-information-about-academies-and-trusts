using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests;

public class TrustHelperTests
{
    private readonly TrustHelper _sut = new();

    [Fact]
    public void CreateTrustFromGroup_should_transform_a_group_into_a_trust()
    {
        var group = new Group
        {
            GroupName = "trust 1", GroupUid = "1234", GroupType = "Multi-academy trust", Ukprn = "my ukprn",
            GroupId = "my groupId",
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC",
            IncorporatedOnOpenDate = "20 Dec 1990",
            CompaniesHouseNumber = "00123444"
        };

        var result = _sut.CreateTrustFromGroup(group);

        result.Should().BeEquivalentTo(new Trust(
                "1234",
                "trust 1",
                "my groupId",
                "my ukprn",
                "Multi-academy trust",
                "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC",
                "20 Dec 1990",
                "00123444"
            )
        );
    }

    [Fact]
    public void BuildAddressString_should_return_full_address_as_string()
    {
        var group = new Group
        {
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        };
        var expected = "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC";
        var result = _sut.BuildAddressString(group);

        result.Should().Be(expected);
    }

    [Fact]
    public void BuildAddressString_should_return_empty_string_if_group_has_no_address_values()
    {
        var group = new Group { GroupName = "trust 1" };
        var result = _sut.BuildAddressString(group);
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void BuildAddressString_should_return_empty_string_if_group_has_empty_address_values()
    {
        var group = new Group
        {
            GroupContactStreet = string.Empty,
            GroupContactLocality = string.Empty,
            GroupContactTown = string.Empty,
            GroupContactPostcode = string.Empty
        };
        var result = _sut.BuildAddressString(group);
        result.Should().Be(string.Empty);
    }

    [Theory]
    [MemberData(nameof(AddressData))]
    public void BuildAddressString_should_be_correctly_formatted_if_group_does_not_contain_all_address_values(
        Group group, string expected)
    {
        var result = _sut.BuildAddressString(group);
        result.Should().Be(expected);
    }

    public static IEnumerable<object[]> AddressData =>
        new List<object[]>
        {
            new object[] { new Group { GroupContactStreet = "DorthyInlet" }, "DorthyInlet" },
            new object[] { new Group { GroupContactLocality = "DorthyInlet" }, "DorthyInlet" },
            new object[]
            {
                new Group { GroupContactStreet = "DorthyInlet", GroupContactTown = "East Park" },
                "DorthyInlet, East Park"
            },
            new object[]
            {
                new Group { GroupContactStreet = "DorthyInlet", GroupContactPostcode = "JY36 9VC" },
                "DorthyInlet, JY36 9VC"
            }
        };
}

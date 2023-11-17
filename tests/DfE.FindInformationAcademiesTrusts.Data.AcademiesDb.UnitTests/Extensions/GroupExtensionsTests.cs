using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Extensions;

public class GroupExtensionsTests
{
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
        var result = group.BuildAddressString();

        result.Should().Be(expected);
    }

    [Fact]
    public void BuildAddressString_should_return_empty_string_if_group_has_no_address_values()
    {
        var group = new Group { GroupName = "trust 1" };
        var result = group.BuildAddressString();
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
        var result = group.BuildAddressString();
        result.Should().Be(string.Empty);
    }

    [Theory]
    [MemberData(nameof(AddressData))]
    public void BuildAddressString_should_be_correctly_formatted_if_group_does_not_contain_all_address_values(
        Group group, string expected)
    {
        var result = group.BuildAddressString();
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

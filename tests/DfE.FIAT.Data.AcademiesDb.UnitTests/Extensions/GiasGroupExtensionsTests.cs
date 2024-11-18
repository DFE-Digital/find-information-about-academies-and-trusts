using DfE.FIAT.Data.AcademiesDb.Extensions;
using DfE.FIAT.Data.AcademiesDb.Models.Gias;

namespace DfE.FIAT.Data.AcademiesDb.UnitTests.Extensions;

public class GiasGroupExtensionsTests
{
    [Fact]
    public void BuildAddressString_should_return_full_address_as_string()
    {
        var giasGroup = new GiasGroup
        {
            GroupContactStreet = "12 Abbey Road",
            GroupContactLocality = "Dorthy Inlet",
            GroupContactTown = "East Park",
            GroupContactPostcode = "JY36 9VC"
        };
        var expected = "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC";
        var result = giasGroup.BuildAddressString();

        result.Should().Be(expected);
    }

    [Fact]
    public void BuildAddressString_should_return_empty_string_if_group_has_no_address_values()
    {
        var giasGroup = new GiasGroup { GroupName = "trust 1" };
        var result = giasGroup.BuildAddressString();
        result.Should().Be(string.Empty);
    }

    [Fact]
    public void BuildAddressString_should_return_empty_string_if_group_has_empty_address_values()
    {
        var giasGroup = new GiasGroup
        {
            GroupContactStreet = string.Empty,
            GroupContactLocality = string.Empty,
            GroupContactTown = string.Empty,
            GroupContactPostcode = string.Empty
        };
        var result = giasGroup.BuildAddressString();
        result.Should().Be(string.Empty);
    }

    [Theory]
    [MemberData(nameof(AddressData))]
    public void BuildAddressString_should_be_correctly_formatted_if_group_does_not_contain_all_address_values(
        GiasGroup giasGroup, string expected)
    {
        var result = giasGroup.BuildAddressString();
        result.Should().Be(expected);
    }

    public static IEnumerable<object[]> AddressData =>
        new List<object[]>
        {
            new object[] { new GiasGroup { GroupContactStreet = "DorthyInlet" }, "DorthyInlet" },
            new object[] { new GiasGroup { GroupContactLocality = "DorthyInlet" }, "DorthyInlet" },
            new object[]
            {
                new GiasGroup { GroupContactStreet = "DorthyInlet", GroupContactTown = "East Park" },
                "DorthyInlet, East Park"
            },
            new object[]
            {
                new GiasGroup { GroupContactStreet = "DorthyInlet", GroupContactPostcode = "JY36 9VC" },
                "DorthyInlet, JY36 9VC"
            }
        };
}

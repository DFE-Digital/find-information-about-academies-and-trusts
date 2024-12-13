namespace DfE.FIAT.Data.AcademiesDb.UnitTests;

public class StringFormattingUtilitiesTests
{
    [Theory]
    [InlineData("12 Abbey Road", "Dorthy Inlet", "East Park", "JY36 9VC",
        "12 Abbey Road, Dorthy Inlet, East Park, JY36 9VC")]
    [InlineData(null, "Dorthy Inlet", "East Park", "JY36 9VC", "Dorthy Inlet, East Park, JY36 9VC")]
    [InlineData("12 Abbey Road", null, "East Park", "JY36 9VC", "12 Abbey Road, East Park, JY36 9VC")]
    [InlineData("12 Abbey Road", "Dorthy Inlet", null, "JY36 9VC", "12 Abbey Road, Dorthy Inlet, JY36 9VC")]
    [InlineData("12 Abbey Road", "Dorthy Inlet", "East Park", null, "12 Abbey Road, Dorthy Inlet, East Park")]
    [InlineData(null, null, null, null, "")]
    [InlineData("", "     ", "", null, "")]
    [InlineData("12 Abbey Road", "  ", " ", "JY36 9VC", "12 Abbey Road, JY36 9VC")]
    public void BuildAddressString_should_build_address_correctly_from_different_combinations_of_values(
        string? street, string? locality, string? town, string? postcode, string expected)
    {
        var sut = new StringFormattingUtilities();

        var result = sut.BuildAddressString(street, locality, town, postcode);

        result.Should().Be(expected);
    }
}

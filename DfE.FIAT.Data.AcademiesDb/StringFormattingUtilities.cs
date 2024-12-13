namespace DfE.FIAT.Data.AcademiesDb;

public interface IStringFormattingUtilities
{
    string BuildAddressString(string? street, string? locality, string? town, string? postcode);
}

public class StringFormattingUtilities : IStringFormattingUtilities
{
    public string BuildAddressString(string? street, string? locality, string? town, string? postcode)
    {
        return string.Join(", ", new[]
        {
            street,
            locality,
            town,
            postcode
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}

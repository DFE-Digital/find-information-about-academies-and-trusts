namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IUtilities
{
    string BuildAddressString(string? street, string? locality, string? town, string? postcode);
}

public class Utilities : IUtilities
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

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IStringFormattingUtilities
{
    string BuildAddressString(string? street, string? locality, string? town, string? postcode);
    string? GetFullName(string? firstName, string? lastName);
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

    public string? GetFullName(string? firstName, string? lastName)
    {
        var hasFirstName = !string.IsNullOrWhiteSpace(firstName);
        var hasLastName = !string.IsNullOrWhiteSpace(lastName);

        var fullName = (hasFirstName, hasLastName) switch
        {
            (hasFirstName: true, hasLastName: true) => $"{firstName} {lastName}",
            (hasFirstName: true, hasLastName: false) => $"{firstName}",
            (hasFirstName: false, hasLastName: true) => $"{lastName}",
            (hasFirstName: false, hasLastName: false) => null
        };

        return fullName;
    }
}

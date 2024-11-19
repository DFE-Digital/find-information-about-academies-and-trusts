using DfE.FIAT.Data.AcademiesDb.Models.Gias;

namespace DfE.FIAT.Data.AcademiesDb.Extensions;

public static class GiasGroupExtensions
{
    public static string BuildAddressString(this GiasGroup giasGroup)
    {
        return string.Join(", ", new[]
        {
            giasGroup.GroupContactStreet,
            giasGroup.GroupContactLocality,
            giasGroup.GroupContactTown,
            giasGroup.GroupContactPostcode
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}

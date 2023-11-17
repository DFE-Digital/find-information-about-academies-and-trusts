using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;

public static class GroupExtensions
{
    public static string BuildAddressString(this Group group)
    {
        return string.Join(", ", new[]
        {
            group.GroupContactStreet,
            group.GroupContactLocality,
            group.GroupContactTown,
            group.GroupContactPostcode
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
    }
}

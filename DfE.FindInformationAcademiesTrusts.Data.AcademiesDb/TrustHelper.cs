using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface ITrustHelper
{
    string BuildAddressString(Group group);
    Trust CreateTrustFromGroup(Group group);
}

public class TrustHelper : ITrustHelper
{
    public Trust CreateTrustFromGroup(Group group)
    {
        return new Trust(
            group.GroupUid!,
            group.GroupName ?? string.Empty,
            group.GroupId ?? string.Empty,
            group.Ukprn,
            group.GroupType ?? string.Empty,
            BuildAddressString(group)
        );
    }

    public string BuildAddressString(Group group)
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

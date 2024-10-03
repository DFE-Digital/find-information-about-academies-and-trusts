using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class ContactRoleExtensions
{
    public static string MapRoleToViewString(this ContactRole role)
    {
        return role switch
        {
            ContactRole.TrustRelationshipManager => "Trust relationship manager",
            ContactRole.SfsoLead => "SFSO (Schools financial support and oversight) lead",
            _ => throw new ArgumentOutOfRangeException(nameof(role))
        };
    }
}

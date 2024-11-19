using DfE.FIAT.Data.Enums;

namespace DfE.FIAT.Web.Extensions;

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

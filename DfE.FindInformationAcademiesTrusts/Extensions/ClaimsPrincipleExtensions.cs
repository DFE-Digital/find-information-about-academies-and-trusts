using System.Security.Claims;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static bool HasAccessToFiat(this ClaimsPrincipal user)
    {
        return user.IsInRole("User.Role.Authorised");
    }
}

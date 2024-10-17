using System.Security.Claims;
using DfE.FindInformationAcademiesTrusts.Configuration;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class ClaimsPrincipleExtensions
{
    public static bool HasAccessToFiat(this ClaimsPrincipal user)
    {
        return user.IsInRole(UserRoles.AuthorisedFiatUser);
    }
}

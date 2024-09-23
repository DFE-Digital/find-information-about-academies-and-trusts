using System.Text.RegularExpressions;
using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts;

public interface IHttpContextUserDetailsProvider
{
    (string Name, string Email) GetUserDetails();
}

public class HttpContextUserDetailsProvider(IHttpContextAccessor httpContextAccessor)
    : IUserDetailsProvider, IHttpContextUserDetailsProvider
{
    public (string Name, string Email) GetUserDetails()
    {
        var context = httpContextAccessor.HttpContext ?? throw new InvalidOperationException();

        var name = context.User.Claims.Single(c => c.Type == "name").Value;
        var email = context.User.Claims.Single(c => c.Type == "preferred_username").Value;

        var nameRegex = new Regex("^([^,]+), ([^,]+)$", RegexOptions.NonBacktracking);

        if (nameRegex.IsMatch(name))
        {
            name = nameRegex.Replace(name, "$2 $1");
        }

        return (name, email);
    }
}

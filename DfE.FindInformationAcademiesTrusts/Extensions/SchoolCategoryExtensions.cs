using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class SchoolCategoryExtensions
{
    public static string ToDisplayString(this SchoolCategory type)
    {
        return type switch
        {
            SchoolCategory.LaMaintainedSchool => "School",
            SchoolCategory.Academy => "Academy",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}

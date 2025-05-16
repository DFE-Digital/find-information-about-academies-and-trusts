using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Extensions;

public static class AgeRangeExtensions
{
    public static string ToFullDisplayString(this AgeRange ageRange)
    {
        return $"{ageRange.Minimum} to {ageRange.Maximum}";
    }
}

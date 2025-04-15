using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Extensions
{
    public static class BeforeOrAfterJoiningExtensions
    {
        public static string ToDisplayString(this BeforeOrAfterJoining beforeOrAfterJoining)
        {
            return beforeOrAfterJoining switch
            {
                BeforeOrAfterJoining.Before => "Before joining",
                BeforeOrAfterJoining.After => "After joining",
                _ => "Unknown"
            };
        }
    }
}

using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public static class MockLogger
{
    public static ILogger<T> CreateLogger<T>()
    {
        return Substitute.For<ILogger<T>>();
    }

    public static void VerifyLogError<T>(this ILogger<T> logger, string regexPattern)
    {
        VerifyLog(logger, LogLevel.Error, regexPattern);
    }

    public static void VerifyLogErrors<T>(this ILogger<T> logger, params string[] regexPatterns)
    {
        foreach (var regexPattern in regexPatterns)
        {
            logger.VerifyLogError(regexPattern);
        }
    }

    private static void VerifyLog<T>(ILogger<T> logger, LogLevel expectedLogLevel, string regexPattern)
    {
        logger.Received(1).Log(
            Arg.Is<LogLevel>(logLevel => logLevel == expectedLogLevel),
            Arg.Any<EventId>(),
            Arg.Is<object>((v) => Regex.IsMatch(v.ToString()!, regexPattern, RegexOptions.NonBacktracking)),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception?, string>>()
        );
    }
}

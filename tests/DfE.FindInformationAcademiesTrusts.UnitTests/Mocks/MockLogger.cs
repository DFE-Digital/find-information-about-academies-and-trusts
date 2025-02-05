using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockLogger<T> : Mock<ILogger<T>>
{
    public MockLogger<T> VerifyLogInformation(string expectedMessage)
    {
        return VerifyLog(LogLevel.Information, expectedMessage);
    }

    public MockLogger<T> VerifyLogWarning(string expectedMessage)
    {
        return VerifyLog(LogLevel.Warning, expectedMessage);
    }

    public MockLogger<T> VerifyLogError(string regexPattern)
    {
        VerifyLog(LogLevel.Error, regexPattern);
        return this;
    }

    public MockLogger<T> VerifyLogErrors(params string[] regexPatterns)
    {
        foreach (var regexPattern in regexPatterns)
        {
            VerifyLogError(regexPattern);
        }

        return this;
    }

    private MockLogger<T> VerifyLog(LogLevel expectedLogLevel, string regexPattern)
    {
        Verify(
            mock => mock.Log(
                It.Is<LogLevel>(logLevel => logLevel == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => Regex.IsMatch(v.ToString()!, regexPattern, RegexOptions.NonBacktracking)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once,
            $"Couldn't find {expectedLogLevel} log matching \"{regexPattern}\""
        );

        return this;
    }
}

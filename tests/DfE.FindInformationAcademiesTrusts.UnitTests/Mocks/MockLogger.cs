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

    public MockLogger<T> VerifyLogError(string expectedMessage)
    {
        return VerifyLog(LogLevel.Error, expectedMessage);
    }

    private MockLogger<T> VerifyLog(LogLevel expectedLogLevel, string expectedMessage)
    {
        Verify(
            mock => mock.Log(
                It.Is<LogLevel>(logLevel => logLevel == expectedLogLevel),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(expectedMessage)),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ), Times.Once, $"Couldn't find {expectedLogLevel} log containing \"{expectedMessage}\""
        );

        return this;
    }
}

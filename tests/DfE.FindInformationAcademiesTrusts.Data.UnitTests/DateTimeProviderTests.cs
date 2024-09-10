namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class DateTimeProviderTests
{
    [Fact]
    public void Now_ShouldReturnCurrentDateTime()
    {
        // Arrange
        IDateTimeProvider dateTimeProvider = new DateTimeProvider();
        DateTime beforeNow = DateTime.Now;

        // Act
        DateTime result = dateTimeProvider.Now;
        DateTime afterNow = DateTime.Now;

        // Assert
        result.Should().BeOnOrAfter(beforeNow);
        result.Should().BeOnOrBefore(afterNow);
    }
}
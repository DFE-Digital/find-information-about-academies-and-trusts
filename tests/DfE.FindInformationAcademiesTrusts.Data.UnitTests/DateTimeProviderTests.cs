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

    [Fact]
    public void Today_ShouldReturnCurrentDateWithoutTime()
    {
        // Arrange
        IDateTimeProvider dateTimeProvider = new DateTimeProvider();
        DateTime expectedDate = DateTime.Today;

        // Act
        DateTime result = dateTimeProvider.Today;

        // Assert
        result.Should().Be(expectedDate);
        result.TimeOfDay.Should().Be(TimeSpan.Zero); // Ensure time component is zero, as this is for 'today' and should have no time element
    }
}
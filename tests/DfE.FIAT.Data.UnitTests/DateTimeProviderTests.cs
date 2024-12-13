using DfE.FIAT.Data;

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class DateTimeProviderTests
{
    [Fact]
    public void Now_ShouldReturnCurrentDateTime()
    {
        // Arrange
        IDateTimeProvider dateTimeProvider = new DateTimeProvider();
        var beforeNow = DateTime.Now;

        // Act
        var result = dateTimeProvider.Now;
        var afterNow = DateTime.Now;

        // Assert
        result.Should().BeOnOrAfter(beforeNow);
        result.Should().BeOnOrBefore(afterNow);
    }

    [Fact]
    public void Today_ShouldReturnCurrentDateWithoutTime()
    {
        // Arrange
        IDateTimeProvider dateTimeProvider = new DateTimeProvider();
        var expectedDate = DateTime.Today;

        // Act
        var result = dateTimeProvider.Today;

        // Assert
        result.Should().Be(expectedDate);
        result.TimeOfDay.Should()
            .Be(TimeSpan.Zero); // Ensure time component is zero, as this is for 'today' and should have no time element
    }
}

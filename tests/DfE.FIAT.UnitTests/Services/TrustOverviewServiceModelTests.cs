using DfE.FIAT.Data.Enums;
using DfE.FIAT.Web.Services.Trust;

namespace DfE.FIAT.UnitTests.Services;

public class TrustOverviewServiceModelTests
{
    private static readonly TrustOverviewServiceModel BaseTrustOverviewServiceModel =
        new("1234", "", "", "", TrustType.MultiAcademyTrust, "", "", null, null, 0, new Dictionary<string, int>(), 0,
            0);

    [Theory]
    [InlineData(100, 100, 100)]
    [InlineData(0, 100, null)]
    [InlineData(0, 0, null)]
    [InlineData(100, 0, 0)]
    [InlineData(100, 50, 50)]
    [InlineData(100, 30, 30)]
    [InlineData(4, 1, 25)]
    [InlineData(3, 1, 33)]
    public void PercentageFull_ReturnsCorrectValue(int totalCapacity, int totalPupilNumbers, int? expectedPercentage)
    {
        // Arrange
        var model = BaseTrustOverviewServiceModel with
        {
            TotalCapacity = totalCapacity,
            TotalPupilNumbers = totalPupilNumbers
        };

        // Act
        var percentageFull = model.PercentageFull;

        // Assert
        percentageFull.Should().Be(expectedPercentage);
    }
}

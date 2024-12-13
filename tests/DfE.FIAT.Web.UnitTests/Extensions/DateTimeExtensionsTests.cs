using DfE.FIAT.Web.Extensions;
using DfE.FIAT.Web.Pages;

namespace DfE.FIAT.Web.UnitTests.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void ShowDateStringOrReplaceWithText_ReturnsFormattedDate_WhenNotNull()
    {
        DateTime? testTime = DateTime.Today;
        var result = testTime.ShowDateStringOrReplaceWithText();
        result.Should().BeEquivalentTo(DateTime.Today.ToString(StringFormatConstants.ViewDate));
    }

    [Fact]
    public void ShowDateStringOrReplaceWithText_returns_NoData_WhenNull()
    {
        DateTime? testTime = null;
        var result = testTime.ShowDateStringOrReplaceWithText();
        result.Should().Be("No data");
    }
}

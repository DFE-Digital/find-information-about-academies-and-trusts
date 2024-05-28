using DfE.FindInformationAcademiesTrusts.Options;

namespace DfE.FindInformationAcademiesTrusts.UnitTests;

public class NotificationBannerOptionsTests
{
    [Fact]
    public void Configuration_section_should_be_NotificationBanner()
    {
        NotificationBannerOptions.ConfigurationSection.Should().Be("NotificationBanner");
    }

    [Fact]
    public void Message_should_default_to_empty_string()
    {
        NotificationBannerOptions sut = new();
        sut.Message.Should().BeNull();
    }
}

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests;

public class PaginatedListTest
{
    private PaginatedList<string> _list = new();

    private readonly string[] _testNames =
    {
        "Evergreen Academy Trust", "Pinnacle Educational Foundation", "Horizon Learning Group",
        "Unity Educational Services", "Starlight Schools Consortium"
    };


    private void SetupList(int page)
    {
        _list = new PaginatedList<string>(_testNames, 20, page, 5);
    }

    [Fact]
    public void PageIndex_property_is_0_by_default()
    {
        _list.PageIndex.Should().Be(0);
    }

    [Fact]
    public void PageIndex_property_is_correct_when_on_firstPage()
    {
        SetupList(1);
        _list.PageIndex.Should().Be(1);
    }

    [Fact]
    public void PageIndex_property_is_correct_when_on_another_page()
    {
        SetupList(3);
        _list.PageIndex.Should().Be(3);
    }

    [Fact]
    public void TotalPages_property_is_0_by_default()
    {
        _list.TotalPages.Should().Be(0);
    }

    [Fact]
    public void TotalPages_property_is_correct_when_on_first_page()
    {
        SetupList(1);
        _list.TotalPages.Should().Be(4);
    }

    [Fact]
    public void TotalResults_property_is_0_by_default()
    {
        _list.TotalResults.Should().Be(0);
    }

    [Fact]
    public void TotalResults_property_is_correct_when_setup()
    {
        SetupList(1);
        _list.TotalResults.Should().Be(20);
    }

    [Fact]
    public void HasPreviousPage_property_is_false_by_default()
    {
        _list.HasPreviousPage.Should().Be(false);
    }

    [Fact]
    public void HasPreviousPage_property_is_false_when_on_first_page()
    {
        SetupList(1);
        _list.HasPreviousPage.Should().Be(false);
    }

    [Fact]
    public void HasPreviousPage_property_is_true_when_on_last_page()
    {
        SetupList(4);
        _list.HasPreviousPage.Should().Be(true);
    }

    [Fact]
    public void HasNextPage_property_is_false_by_default()
    {
        _list.HasNextPage.Should().Be(false);
    }

    [Fact]
    public void HasNextPage_property_is_true_when_on_first_page()
    {
        SetupList(1);
        _list.HasNextPage.Should().Be(true);
    }

    [Fact]
    public void HasNextPage_property_is_false_when_on_first_page()
    {
        SetupList(4);
        _list.HasNextPage.Should().Be(false);
    }
}

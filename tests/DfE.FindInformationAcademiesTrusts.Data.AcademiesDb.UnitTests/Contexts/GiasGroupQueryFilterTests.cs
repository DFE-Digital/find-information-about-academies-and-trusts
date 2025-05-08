using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasGroupQueryFilterTests
{
    [Fact]
    public void GiasGroupQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validGiasGroup = new GiasGroup
            { GroupUid = "1234", GroupName = "Some GroupName", GroupType = "Some GroupType", GroupStatusCode = "OPEN" };

        var filterFunction = AcademiesDbContext.GiasGroupQueryFilter.Compile();

        GiasGroup[] data =
        [
            validGiasGroup,
            new()
            {
                GroupUid = null, GroupName = "Some GroupName", GroupType = "Some GroupType", GroupStatusCode = "OPEN"
            },
            new() { GroupUid = "1234", GroupName = null, GroupType = "Some GroupType", GroupStatusCode = "OPEN" },
            new() { GroupUid = "1234", GroupName = "Some GroupName", GroupType = null, GroupStatusCode = "OPEN" },
            new()
            {
                GroupUid = "1234", GroupName = "Some GroupName", GroupType = "Some GroupType", GroupStatusCode = null
            }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validGiasGroup);
    }

    [Theory]
    [InlineData("CLOSED", false)]
    [InlineData("OPEN", true)]
    [InlineData("PROPOSED_TO_CLOSE", true)]
    public void GiasGroupQueryFilter_should_filter_on_group_status(string groupStatusCode, bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasGroupQueryFilter.Compile();

        var giasGroup = new GiasGroup
        {
            GroupUid = "1234", GroupName = "Some GroupName", GroupType = "Some GroupType",
            GroupStatusCode = groupStatusCode
        };
        filterFunction(giasGroup).Should().Be(expectedResult);
    }
}

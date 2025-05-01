using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasGroupLinkQueryFilterTests
{
    [Fact]
    public void GiasGroupLinkQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validGiasGroupLink = new GiasGroupLink { Urn = "123456", GroupUid = "1234", GroupStatusCode = "OPEN" };

        var filterFunction = AcademiesDbContext.GiasGroupLinkQueryFilter.Compile();

        GiasGroupLink[] data =
        [
            validGiasGroupLink,
            new() { Urn = null, GroupUid = "1234", GroupStatusCode = "OPEN" },
            new() { Urn = "123456", GroupUid = null, GroupStatusCode = "OPEN" }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validGiasGroupLink);
    }

    [Theory]
    [InlineData("CLOSED", false)]
    [InlineData("OPEN", true)]
    [InlineData("PROPOSED_TO_CLOSE", true)]
    public void GiasGroupLinkQueryFilter_should_filter_on_group_status(string groupStatusCode, bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasGroupLinkQueryFilter.Compile();

        var giasGroupLink = new GiasGroupLink { Urn = "123456", GroupUid = "1234", GroupStatusCode = groupStatusCode };

        filterFunction(giasGroupLink).Should().Be(expectedResult);
    }
}

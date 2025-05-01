using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasGroupLinkQueryFilterTests
{
    [Fact]
    public void GiasGroupLinkQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validGiasGroupLink = new GiasGroupLink { Urn = "123456", GroupUid = "1234" };

        var filterFunction = AcademiesDbContext.GiasGroupLinkQueryFilter.Compile();

        GiasGroupLink[] data =
        [
            validGiasGroupLink,
            new() { Urn = null, GroupUid = "1234" },
            new() { Urn = "123456", GroupUid = null }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validGiasGroupLink);
    }
}

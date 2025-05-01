using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasEstablishmentLinkQueryFilterTests
{
    [Fact]
    public void GiasEstablishmentLinkQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validGiasEstablishmentLink = new GiasEstablishmentLink
            { Urn = "123456", LinkUrn = "789123", LinkType = "Successor" };

        var filterFunction = AcademiesDbContext.GiasEstablishmentLinkQueryFilter.Compile();

        GiasEstablishmentLink[] data =
        [
            validGiasEstablishmentLink,
            new() { Urn = null, LinkUrn = "789123", LinkType = "Successor" },
            new() { Urn = "123456", LinkUrn = null, LinkType = "Successor" },
            new() { Urn = "123456", LinkUrn = "789123", LinkType = null }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validGiasEstablishmentLink);
    }
}

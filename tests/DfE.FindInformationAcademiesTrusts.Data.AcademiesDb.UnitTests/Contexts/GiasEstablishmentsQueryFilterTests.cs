using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasEstablishmentsQueryFilterTests
{
    [Fact]
    public void GiasEstablishmentsQueryFilter_should_filter_nullable_columns_that_can_never_contain_null_data()
    {
        var validGiasEstablishment = new GiasEstablishment
        {
            EstablishmentStatusName = "Open",
            EstablishmentTypeGroupName = "Academies",
            EstablishmentName = "My Academy"
        };

        var filterFunction = AcademiesDbContext.GiasEstablishmentsQueryFilter.Compile();

        GiasEstablishment[] data =
        [
            validGiasEstablishment,
            new()
            {
                EstablishmentStatusName = null, EstablishmentTypeGroupName = "Academies",
                EstablishmentName = "My Academy"
            },
            new()
            {
                EstablishmentStatusName = "Open", EstablishmentTypeGroupName = null, EstablishmentName = "My Academy"
            },
            new()
            {
                EstablishmentStatusName = "Open", EstablishmentTypeGroupName = "Academies", EstablishmentName = null
            }
        ];

        data.Where(filterFunction).Should()
            .ContainSingle()
            .Which.Should().Be(validGiasEstablishment);
    }

    [Theory]
    [InlineData("Academies", true)]
    [InlineData("Colleges", true)]
    [InlineData("Free Schools", true)]
    [InlineData("Local authority maintained schools", true)]
    [InlineData("Special schools", true)]
    [InlineData("Independent schools", false)]
    [InlineData("Online provider", false)]
    [InlineData("Other types", false)]
    [InlineData("Universities", false)]
    public void GiasEstablishmentsQueryFilter_should_filter_for_supported_school_types(string schoolType,
        bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasEstablishmentsQueryFilter.Compile();

        filterFunction(new GiasEstablishment
            {
                EstablishmentTypeGroupName = schoolType,
                EstablishmentStatusName = "Open",
                EstablishmentName = "My Academy"
            })
            .Should()
            .Be(expectedResult);
    }

    [Theory]
    [InlineData("Open", true)]
    [InlineData("Closed", false)]
    [InlineData("Open, but proposed to close", true)]
    [InlineData("Proposed to open", true)]
    public void GiasEstablishmentQueryFilter_should_filter_for_establishment_status(string establishmentStatusName,
        bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasEstablishmentsQueryFilter.Compile();

        filterFunction(new GiasEstablishment
            {
                EstablishmentTypeGroupName = "Academies",
                EstablishmentStatusName = establishmentStatusName,
                EstablishmentName = "My Academy"
            })
            .Should()
            .Be(expectedResult);
    }
}

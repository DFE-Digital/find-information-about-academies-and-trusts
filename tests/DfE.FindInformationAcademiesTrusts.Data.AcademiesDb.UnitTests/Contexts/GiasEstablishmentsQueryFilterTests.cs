using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasEstablishmentsQueryFilterTests
{
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

        filterFunction(new GiasEstablishment { EstablishmentTypeGroupName = schoolType }).Should().Be(expectedResult);
    }
}

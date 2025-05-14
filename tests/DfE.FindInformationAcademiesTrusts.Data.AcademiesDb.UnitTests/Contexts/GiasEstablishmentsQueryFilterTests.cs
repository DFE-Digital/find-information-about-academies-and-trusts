using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Contexts;

public class GiasEstablishmentsQueryFilterTests
{
    [Theory]
    [InlineData("Academies", "Open", true)]
    [InlineData("Colleges", "Open",true)]
    [InlineData("Free Schools", "Open",true)]
    [InlineData("Local authority maintained schools", "Open",true)]
    [InlineData("Special schools", "Open",true)]
    [InlineData("Independent schools", "Open",false)]
    [InlineData("Online provider", "Open",false)]
    [InlineData("Other types", "Open",false)]
    [InlineData("Universities", "Open",false)]
    public void GiasEstablishmentsQueryFilter_should_filter_for_supported_school_types(string schoolType,
        string establishmentStatusName,
        bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasEstablishmentsQueryFilter.Compile();

        filterFunction(new GiasEstablishment
                { EstablishmentTypeGroupName = schoolType, EstablishmentStatusName = establishmentStatusName })
            .Should()
            .Be(expectedResult);
    }

    [Theory]
    [InlineData("Academies", "Open", true)]
    [InlineData("Academies","Closed", false)]
    [InlineData("Academies","Open, but proposed to close", true)]
    [InlineData("Academies","Proposed to open", true)]
    public void GiasEstablishmentQueryFilter_should_filter_for_establishment_status(string schoolType,
        string establishmentStatusName,
        bool expectedResult)
    {
        var filterFunction = AcademiesDbContext.GiasEstablishmentsQueryFilter.Compile();

        filterFunction(new GiasEstablishment
                { EstablishmentTypeGroupName = schoolType, EstablishmentStatusName = establishmentStatusName })
            .Should()
            .Be(expectedResult);
    }
}

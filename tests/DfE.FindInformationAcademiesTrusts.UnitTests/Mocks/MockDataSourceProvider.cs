using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockDataSourceProvider : Mock<IDataSourceProvider>
{
    public MockDataSourceProvider()
    {
        var staticTime = new DateTime(2023, 11, 9);
        Setup(f => f.GetGiasUpdated())
            .ReturnsAsync(new DataSource(Source.Gias, staticTime, UpdateFrequency.Daily));
        Setup(f => f.GetMstrUpdated())
            .ReturnsAsync(new DataSource(Source.Mstr, staticTime, UpdateFrequency.Daily));
        Setup(f => f.GetCdmUpdated())
            .ReturnsAsync(new DataSource(Source.Cdm, staticTime, UpdateFrequency.Daily));
        Setup(f => f.GetMisEstablishmentsUpdated())
            .ReturnsAsync(new DataSource(Source.Mis, staticTime, UpdateFrequency.Monthly));
    }
}

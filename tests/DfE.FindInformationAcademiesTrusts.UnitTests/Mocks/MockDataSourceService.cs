using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using DfE.FindInformationAcademiesTrusts.Services;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockDataSourceService : Mock<IDataSourceService>
{
    public MockDataSourceService()
    {
        var staticTime = new DateTime(2023, 11, 9);
        Setup(f => f.GetAsync(It.IsAny<Source>()))
            .ReturnsAsync((Source source) => new DataSourceServiceModel(source, staticTime, source switch
            {
                Source.Gias => UpdateFrequency.Daily,
                Source.Mstr => UpdateFrequency.Daily,
                Source.Cdm => UpdateFrequency.Daily,
                Source.Mis => UpdateFrequency.Monthly,
                Source.ExploreEducationStatistics => UpdateFrequency.Annually,
                _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
            }));
    }
}

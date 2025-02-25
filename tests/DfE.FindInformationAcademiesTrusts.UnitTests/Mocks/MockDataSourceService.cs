using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockDataSourceService : Mock<IDataSourceService>
{
    private static readonly DateTime StaticTime = new(2023, 11, 9);

    public MockDataSourceService()
    {
        Setup(f => f.GetAsync(It.IsAny<Source>())).ReturnsAsync((Source source) => GetDummyDataSource(source));
    }

    public static DataSourceServiceModel GetDummyDataSource(Source source)
    {
        return new DataSourceServiceModel(source, StaticTime, source switch
        {
            Source.Cdm => UpdateFrequency.Daily,
            Source.Complete => UpdateFrequency.Daily,
            Source.Gias => UpdateFrequency.Daily,
            Source.ManageFreeSchoolProjects => UpdateFrequency.Daily,
            Source.Mstr => UpdateFrequency.Daily,
            Source.Prepare => UpdateFrequency.Daily,
            Source.Mis => UpdateFrequency.Monthly,
            Source.ExploreEducationStatistics => UpdateFrequency.Annually,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        });
    }

    public DataSourceServiceModel Prepare { get; } = GetDummyDataSource(Source.Prepare);
    public DataSourceServiceModel Complete { get; } = GetDummyDataSource(Source.Complete);
    public DataSourceServiceModel ManageFreeSchool { get; } = GetDummyDataSource(Source.ManageFreeSchoolProjects);
}

using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public static class MockDataSourceService
{
    private static readonly DateTime StaticTime = new(2023, 11, 9);
    public static IDataSourceService CreateSubstitute()
    {
        var mockDataSourceService = Substitute.For<IDataSourceService>();

        mockDataSourceService
            .GetAsync(Arg.Any<Source>())
            .Returns(args =>
            {
                var source = (Source)args[0];
                return Task.FromResult(GetDummyDataSource(source));
            });
        return mockDataSourceService;
    }

    private static DataSourceServiceModel GetDummyDataSource(Source source)
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
    
    public static DataSourceServiceModel Prepare { get; } = GetDummyDataSource(Source.Prepare);
    public static DataSourceServiceModel Complete { get; } = GetDummyDataSource(Source.Complete);
    public static DataSourceServiceModel ManageFreeSchool { get; } = GetDummyDataSource(Source.ManageFreeSchoolProjects);
}

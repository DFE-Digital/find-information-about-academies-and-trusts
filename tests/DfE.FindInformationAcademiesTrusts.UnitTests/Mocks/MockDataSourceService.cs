using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;
using Microsoft.EntityFrameworkCore.Storage;
using NSubstitute;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class MockDataSourceService: IDataSourceService
{
    private static readonly DateTime StaticTime = new(2023, 11, 9);
    
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
    
    public Task<DataSourceServiceModel> GetAsync(Source source)
    {

         var mockDataSourceService = Substitute.For<IDataSourceService>();
         mockDataSourceService.GetAsync(source)
            .Returns(Task.FromResult(GetDummyDataSource(source)));
         return mockDataSourceService.GetAsync(source);
    }
    
    public DataSourceServiceModel Prepare { get; } = GetDummyDataSource(Source.Prepare);
    public DataSourceServiceModel Complete { get; } = GetDummyDataSource(Source.Complete);
    public DataSourceServiceModel ManageFreeSchool { get; } = GetDummyDataSource(Source.ManageFreeSchoolProjects);
}

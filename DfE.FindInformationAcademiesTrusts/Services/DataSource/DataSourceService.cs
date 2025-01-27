using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;
using Microsoft.Extensions.Caching.Memory;
using System.Runtime.CompilerServices;

namespace DfE.FindInformationAcademiesTrusts.Services.DataSource;

public interface IDataSourceService
{
    Task<DataSourceServiceModel> GetAsync(Source source);
}

public class DataSourceService(
    IDataSourceRepository dataSourceRepository,
    IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider,
    IMemoryCache memoryCache) : IDataSourceService
{
    public async Task<DataSourceServiceModel> GetAsync(Source source)
    {
        if (memoryCache.TryGetValue(source, out DataSourceServiceModel? cachedDataSource))
        {
            return cachedDataSource!;
        }

        var dataSource = source switch
        {
            Source.Gias or Source.Mstr or Source.Cdm or Source.Mis => await dataSourceRepository.GetAsync(source),
            Source.ExploreEducationStatistics => freeSchoolMealsAverageProvider.GetFreeSchoolMealsUpdated(),
            Source.Prepare or Source.Complete or Source.ManageFreeSchoolProjects =>
               new Data.Repositories.DataSource.DataSource(source, new DateTime(2025, 1, 1), UpdateFrequency.Daily),
            //TODO: Last updated put it to not fixed date
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };

        var dataSourceServiceModel =
            new DataSourceServiceModel(dataSource.Source, dataSource.LastUpdated, dataSource.NextUpdated);

        if (dataSourceServiceModel.LastUpdated is not null)
        {
            var cacheExpiration = dataSourceServiceModel.NextUpdated switch
            {
                UpdateFrequency.Daily => TimeSpan.FromHours(1),
                UpdateFrequency.Monthly or
                    UpdateFrequency.Annually => TimeSpan.FromDays(1),
                _ => throw new SwitchExpressionException(dataSourceServiceModel.NextUpdated)
            };

            memoryCache.Set(source, dataSourceServiceModel, cacheExpiration);
        }

        return dataSourceServiceModel;
    }
}

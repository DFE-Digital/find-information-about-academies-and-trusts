using System.Runtime.CompilerServices;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.ServiceModels;
using Microsoft.Extensions.Caching.Memory;

namespace DfE.FindInformationAcademiesTrusts.Services;

public interface IDataSourceService
{
    Task<DataSourceServiceModel> GetAsync(Source source);
}

public class DataSourceService(
    IAcademiesDbDataSourceRepository academiesDbDataSourceRepository,
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
            Source.Gias => await academiesDbDataSourceRepository.GetGiasUpdatedAsync(),
            Source.Mstr => await academiesDbDataSourceRepository.GetMstrUpdatedAsync(),
            Source.Cdm => await academiesDbDataSourceRepository.GetCdmUpdatedAsync(),
            Source.Mis => await academiesDbDataSourceRepository.GetMisEstablishmentsUpdatedAsync(),
            Source.ExploreEducationStatistics => freeSchoolMealsAverageProvider.GetFreeSchoolMealsUpdated(),
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

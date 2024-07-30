using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories;
using DfE.FindInformationAcademiesTrusts.ServiceModels;

namespace DfE.FindInformationAcademiesTrusts.Services;

public interface IDataSourceService
{
    Task<DataSourceServiceModel> GetAsync(Source source);
}

public class DataSourceService(
    IAcademiesDbDataSourceRepository academiesDbDataSourceRepository,
    IFreeSchoolMealsAverageProvider freeSchoolMealsAverageProvider) : IDataSourceService
{
    public async Task<DataSourceServiceModel> GetAsync(Source source)
    {
        var dataSource = source switch
        {
            Source.Gias => await academiesDbDataSourceRepository.GetGiasUpdatedAsync(),
            Source.Mstr => await academiesDbDataSourceRepository.GetMstrUpdatedAsync(),
            Source.Cdm => await academiesDbDataSourceRepository.GetCdmUpdatedAsync(),
            Source.Mis => await academiesDbDataSourceRepository.GetMisEstablishmentsUpdatedAsync(),
            Source.ExploreEducationStatistics => freeSchoolMealsAverageProvider.GetFreeSchoolMealsUpdated(),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };

        return new DataSourceServiceModel(dataSource.Source, dataSource.LastUpdated, dataSource.NextUpdated);
    }
}

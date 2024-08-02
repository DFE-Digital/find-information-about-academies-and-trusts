using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface IDataSourceRepository
{
    Task<DataSource> GetGiasUpdatedAsync();
    Task<DataSource> GetMstrUpdatedAsync();
    Task<DataSource> GetCdmUpdatedAsync();
    Task<DataSource> GetMisEstablishmentsUpdatedAsync();
}

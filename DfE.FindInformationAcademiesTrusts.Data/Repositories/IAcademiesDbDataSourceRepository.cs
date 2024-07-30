using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface IAcademiesDbDataSourceRepository
{
    Task<DataSource> GetGiasUpdatedAsync();
    Task<DataSource> GetMstrUpdatedAsync();
    Task<DataSource> GetCdmUpdatedAsync();
    Task<DataSource> GetMisEstablishmentsUpdatedAsync();
}

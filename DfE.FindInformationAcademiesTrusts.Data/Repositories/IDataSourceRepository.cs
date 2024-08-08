using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories;

public interface IDataSourceRepository
{
    Task<DataSource> GetAsync(Source source);
}

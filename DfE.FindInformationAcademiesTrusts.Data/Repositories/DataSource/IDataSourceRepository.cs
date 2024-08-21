using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.DataSource;

public interface IDataSourceRepository
{
    Task<DataSource> GetAsync(Source source);
}

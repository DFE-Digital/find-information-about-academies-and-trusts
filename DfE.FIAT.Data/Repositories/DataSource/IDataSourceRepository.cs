using DfE.FIAT.Data.Enums;

namespace DfE.FIAT.Data.Repositories.DataSource;

public interface IDataSourceRepository
{
    Task<DataSource> GetAsync(Source source);
}

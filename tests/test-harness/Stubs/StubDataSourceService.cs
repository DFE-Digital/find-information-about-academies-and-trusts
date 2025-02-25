using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Services.DataSource;

namespace test_harness;

public class StubDataSourceService : IDataSourceService
{
    public Task<DataSourceServiceModel> GetAsync(Source source)
    {
        return Task.FromResult(new DataSourceServiceModel(source, DateTime.Now, UpdateFrequency.Daily));
    }
}

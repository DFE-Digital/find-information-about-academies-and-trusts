using Microsoft.Data.SqlClient;
using Testcontainers.MsSql;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests.TestContainer;

public class FiatDbContainerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _fiatDbContainer = new MsSqlBuilder()
        .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
        .Build();

    public string? ConnectionString { get; private set; }

    public virtual async Task InitializeAsync()
    {
        await _fiatDbContainer.StartAsync();

        ConnectionString = new SqlConnectionStringBuilder(_fiatDbContainer.GetConnectionString())
        {
            InitialCatalog = "fiat"
        }.ToString();
    }

    public async Task DisposeAsync()
    {
        await _fiatDbContainer.DisposeAsync();
    }
}

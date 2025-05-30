using Dapper;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Dapper;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Repositories;

public class GenericRepository(IDbConnectionFactory dbConnectionFactory)
{
    public async Task<int> InsertAsync<T>(string query, List<T> data)
    {
        using var connection = dbConnectionFactory.CreateConnection();

       return await connection.ExecuteAsync(query, data);
    }
}
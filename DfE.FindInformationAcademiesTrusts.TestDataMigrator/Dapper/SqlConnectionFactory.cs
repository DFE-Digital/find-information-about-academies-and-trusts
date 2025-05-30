namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Dapper
{
    using Microsoft.Data.SqlClient;
    using System.Data;

    internal class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}

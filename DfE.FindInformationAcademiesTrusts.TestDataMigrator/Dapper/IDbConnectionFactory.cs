namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Dapper
{
    using System.Data;

    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

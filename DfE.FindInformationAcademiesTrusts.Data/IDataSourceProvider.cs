namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IDataSourceProvider
{
    Task<DataSource> GetGiasUpdated();
    Task<DataSource> GetMstrUpdated();
    Task<DataSource> GetCdmUpdated();
    Task<DataSource> GetMisEstablishmentsUpdated();
}

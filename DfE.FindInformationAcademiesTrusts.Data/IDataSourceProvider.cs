namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IDataSourceProvider
{
    Task<DataSource?> GetGIASUpdated();
    Task<DataSource?> GetMSTRUpdated();
    Task<DataSource?> GetCDMUpdated();
    Task<DataSource?> GetMISEstablishmentsUpdated();
    Task<DataSource?> GetMISFurtherEducationEstablishmentsUpdated();
}

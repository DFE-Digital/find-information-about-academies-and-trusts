namespace DfE.FindInformationAcademiesTrusts.Data;

public record DataSource(string Name, DateTime LastUpdated, DateTime? NextUpdated, string NextUpdatedWorded);

namespace DfE.FindInformationAcademiesTrusts.Data;

public record DataSource(Source Source, DateTime? LastUpdated, UpdateFrequency NextUpdated);

public enum Source
{
    Gias,
    Mstr,
    Cdm,
    Mis
}

public enum UpdateFrequency
{
    Daily,
    Monthly
}

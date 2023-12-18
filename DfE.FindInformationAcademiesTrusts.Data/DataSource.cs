namespace DfE.FindInformationAcademiesTrusts.Data;

public record DataSource(Source Source, DateTime? LastUpdated, UpdateFrequency NextUpdated);

public enum Source
{
    Gias,
    Mstr,
    Cdm,
    Mis,
    ExploreEducationStatistics
}

public enum UpdateFrequency
{
    Daily,
    Monthly,
    Annually
}

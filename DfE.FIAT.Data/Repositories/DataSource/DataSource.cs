using DfE.FIAT.Data.Enums;

namespace DfE.FIAT.Data.Repositories.DataSource;

public record DataSource(Source Source, DateTime? LastUpdated, UpdateFrequency NextUpdated);

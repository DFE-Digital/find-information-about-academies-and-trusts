using DfE.FIAT.Data.Enums;

namespace DfE.FIAT.Web.Services.DataSource;

public record DataSourceServiceModel(
    Source Source,
    DateTime? LastUpdated,
    UpdateFrequency? NextUpdated,
    string? UpdatedBy = null);

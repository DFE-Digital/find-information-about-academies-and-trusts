using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.DataSource;

public record DataSourceServiceModel(
    Source Source,
    DateTime? LastUpdated,
    UpdateFrequency? NextUpdated,
    string? UpdatedBy = null);

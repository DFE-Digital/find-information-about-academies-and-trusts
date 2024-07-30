using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.ServiceModels;

public record DataSourceServiceModel(Source Source, DateTime? LastUpdated, UpdateFrequency NextUpdated);

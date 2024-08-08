using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Models;

public record DataSource(Source Source, DateTime? LastUpdated, UpdateFrequency NextUpdated);

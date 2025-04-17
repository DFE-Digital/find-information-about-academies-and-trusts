using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public record SchoolSummaryServiceModel(string Urn, string Name, string Type, SchoolCategory Category);

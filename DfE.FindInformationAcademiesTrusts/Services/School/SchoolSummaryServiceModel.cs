using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public record SchoolSummaryServiceModel(int Urn, string Name, string Type, SchoolCategory Category);

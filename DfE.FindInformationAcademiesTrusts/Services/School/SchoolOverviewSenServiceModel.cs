namespace DfE.FindInformationAcademiesTrusts.Services.School;

public record SchoolOverviewSenServiceModel(
     string? ResourcedProvisionOnRoll,
     string? ResourcedProvisionCapacity,
     string? SenOnRoll,
     string? SenCapacity,
     string? ResourcedProvisionTypes,
     List<string> SenProvisionTypes);


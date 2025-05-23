namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

public record SenProvision(
    string? ResourcedProvisionOnRoll, 
    string? ResourcedProvisionCapacity, 
    string? SenOnRoll,
    string? SenCapacity,
    string? ResourcedProvisionTypes,
    List<string> SenProvisionTypes);
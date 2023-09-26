namespace DfE.FindInformationAcademiesTrusts;

public record TrustSearchEntry(string Name, string Address, string? Ukprn, int AcademyCount);

public record Trust(string Name, string? Ukprn, string Type);

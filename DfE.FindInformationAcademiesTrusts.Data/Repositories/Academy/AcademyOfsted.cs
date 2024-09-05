namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy;

public record AcademyOfsted(
    string Urn,
    string? EstablishmentName,
    DateTime DateAcademyJoinedTrust,
    OfstedRating PreviousOfstedRating,
    OfstedRating CurrentOfstedRating
);

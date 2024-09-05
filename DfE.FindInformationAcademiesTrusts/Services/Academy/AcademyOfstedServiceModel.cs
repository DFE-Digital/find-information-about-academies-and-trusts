using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademyOfstedServiceModel(
    string Urn,
    string? EstablishmentName,
    DateTime DateAcademyJoinedTrust,
    OfstedRating PreviousOfstedRating,
    OfstedRating CurrentOfstedRating
);

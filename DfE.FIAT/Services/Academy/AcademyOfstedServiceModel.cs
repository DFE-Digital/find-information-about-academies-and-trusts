using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Services.Academy;

public record AcademyOfstedServiceModel(
    string Urn,
    string? EstablishmentName,
    DateTime DateAcademyJoinedTrust,
    OfstedRating PreviousOfstedRating,
    OfstedRating CurrentOfstedRating
);

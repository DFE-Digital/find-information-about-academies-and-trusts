namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.Academy
{
    public record AcademyOverview(
     string Urn,
     string EstablishmentName,
     string LocalAuthority,
     int? NumberOfPupils,
     int? SchoolCapacity,
     OfstedRatingScore CurrentOfstedRating);


}

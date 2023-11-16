using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IAcademyHelper
{
    Academy CreateAcademyFrom(GroupLink gl, Establishment establishment);
}

public class AcademyHelper : IAcademyHelper
{
    public Academy CreateAcademyFrom(GroupLink gl, Establishment establishment)
    {
        return new Academy(
            establishment.Urn,
            establishment.EstablishmentName,
            establishment.TypeOfEstablishmentName,
            establishment.LaName,
            establishment.UrbanRuralName,
            establishment.PhaseOfEducationName,
            establishment.NumberOfPupils,
            establishment.SchoolCapacity,
            establishment.PercentageFsm,
            new AgeRange(establishment.StatutoryLowAge!, establishment.StatutoryHighAge!),
            establishment.OfstedRatingName != null
                ? new OfstedRating(establishment.OfstedRatingName, ParseAsDate(establishment.OfstedLastInsp))
                : null,
            null
        );
    }

    private static DateTime? ParseAsDate(string? date)
    {
        if (string.IsNullOrEmpty(date)) return null;
        var newDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

        return newDate;
    }
}

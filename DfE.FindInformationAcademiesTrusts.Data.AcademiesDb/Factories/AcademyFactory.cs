using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IAcademyFactory
{
    Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishment, MisFurtherEducationEstablishment? misFurtherEducationEstablishment);
}

public class AcademyFactory : IAcademyFactory
{
    public Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishment, MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        return new Academy(
            giasEstablishment.Urn,
            DateTime.ParseExact(gl.JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture),
            giasEstablishment.EstablishmentName,
            giasEstablishment.TypeOfEstablishmentName,
            giasEstablishment.LaName,
            giasEstablishment.UrbanRuralName,
            giasEstablishment.PhaseOfEducationName,
            giasEstablishment.NumberOfPupils.ParseAsNullableInt(),
            giasEstablishment.SchoolCapacity.ParseAsNullableInt(),
            giasEstablishment.PercentageFsm,
            new AgeRange(giasEstablishment.StatutoryLowAge!, giasEstablishment.StatutoryHighAge!),
            GetCurrentOfstedRating(misEstablishment, misFurtherEducationEstablishment),
            null
        );
    }

    private static OfstedRating GetCurrentOfstedRating(MisEstablishment? misEstablishment,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        if (misEstablishment is not null)
        {
            return new OfstedRating(
                (OfstedRatingScore)misEstablishment.OverallEffectiveness!.Value,
                DateTime.ParseExact(misEstablishment.InspectionEndDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }

        if (misFurtherEducationEstablishment is not null)
        {
            return misFurtherEducationEstablishment.OverallEffectiveness is null
                ? OfstedRating.None
                : new OfstedRating(
                    (OfstedRatingScore)misFurtherEducationEstablishment.OverallEffectiveness.Value,
                    DateTime.ParseExact(misFurtherEducationEstablishment.LastDayOfInspection!, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture)
                );
        }

        return OfstedRating.None;
    }
}

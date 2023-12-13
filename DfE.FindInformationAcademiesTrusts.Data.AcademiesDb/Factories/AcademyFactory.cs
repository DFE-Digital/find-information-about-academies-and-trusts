using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IAcademyFactory
{
    Academy CreateFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishmentCurrentOfsted, MisEstablishment? misEstablishmentPreviousOfsted,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment);
}

public class AcademyFactory : IAcademyFactory
{
    public Academy CreateFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishmentCurrentOfsted = null, MisEstablishment? misEstablishmentPreviousOfsted = null,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment = null)
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
            GetCurrentOfstedRating(misEstablishmentCurrentOfsted, misFurtherEducationEstablishment),
            GetPreviousOfstedRating(misEstablishmentPreviousOfsted, misFurtherEducationEstablishment)
        );
    }

    private static OfstedRating GetCurrentOfstedRating(MisEstablishment? misEstablishmentCurrentOfsted,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        if (misEstablishmentCurrentOfsted is not null)
        {
            return new OfstedRating(
                (OfstedRatingScore)misEstablishmentCurrentOfsted.OverallEffectiveness!.Value,
                DateTime.ParseExact(misEstablishmentCurrentOfsted.InspectionEndDate!, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture)
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

    private static OfstedRating GetPreviousOfstedRating(MisEstablishment? misEstablishmentPreviousOfsted,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        if (misEstablishmentPreviousOfsted is not null)
        {
            return new OfstedRating(
                (OfstedRatingScore)int.Parse(misEstablishmentPreviousOfsted
                    .PreviousFullInspectionOverallEffectiveness!),
                DateTime.ParseExact(misEstablishmentPreviousOfsted.PreviousInspectionEndDate!, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture)
            );
        }

        if (misFurtherEducationEstablishment is not null)
        {
            return misFurtherEducationEstablishment.PreviousOverallEffectiveness is null
                ? OfstedRating.None
                : new OfstedRating(
                    (OfstedRatingScore)misFurtherEducationEstablishment.PreviousOverallEffectiveness.Value,
                    DateTime.ParseExact(misFurtherEducationEstablishment.PreviousLastDayOfInspection!, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture)
                );
        }

        return OfstedRating.None;
    }
}

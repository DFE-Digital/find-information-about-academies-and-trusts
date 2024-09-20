using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using System.Globalization;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IAcademyFactory
{
    Academy CreateFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishmentCurrentOfsted = null, MisEstablishment? misEstablishmentPreviousOfsted = null,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment = null);
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
            giasEstablishment.PercentageFsm.ParseAsNullableDouble(),
            new AgeRange(giasEstablishment.StatutoryLowAge!, giasEstablishment.StatutoryHighAge!),
            GetCurrentOfstedRating(misEstablishmentCurrentOfsted, misFurtherEducationEstablishment),
            GetPreviousOfstedRating(misEstablishmentPreviousOfsted, misFurtherEducationEstablishment),
            int.Parse(giasEstablishment.LaCode!)
        );
    }

    private static OfstedRating GetCurrentOfstedRating(MisEstablishment? misEstablishmentCurrentOfsted,
        MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        if (misEstablishmentCurrentOfsted is not null
            && misEstablishmentCurrentOfsted.OverallEffectiveness is not null
            && !string.IsNullOrEmpty(misEstablishmentCurrentOfsted.InspectionEndDate))
        {
            return new OfstedRating(
                (OfstedRatingScore)misEstablishmentCurrentOfsted.OverallEffectiveness.Value,
                DateTime.ParseExact(misEstablishmentCurrentOfsted.InspectionEndDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }

        if (misFurtherEducationEstablishment is not null
            && misFurtherEducationEstablishment.OverallEffectiveness is not null
            && !string.IsNullOrEmpty(misFurtherEducationEstablishment.LastDayOfInspection))
        {
            return new OfstedRating(
                (OfstedRatingScore)misFurtherEducationEstablishment.OverallEffectiveness.Value,
                DateTime.ParseExact(misFurtherEducationEstablishment.LastDayOfInspection!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }

        return OfstedRating.None;
    }

    private static OfstedRating GetPreviousOfstedRating(MisEstablishment? misEstablishmentPreviousOfsted,
    MisFurtherEducationEstablishment? misFurtherEducationEstablishment)
    {
        if (misEstablishmentPreviousOfsted is not null
            && !string.IsNullOrEmpty(misEstablishmentPreviousOfsted.PreviousFullInspectionOverallEffectiveness)
            && !string.IsNullOrEmpty(misEstablishmentPreviousOfsted.PreviousInspectionEndDate))
        {
            return new OfstedRating(
                (OfstedRatingScore)int.Parse(misEstablishmentPreviousOfsted.PreviousFullInspectionOverallEffectiveness!),
                DateTime.ParseExact(misEstablishmentPreviousOfsted.PreviousInspectionEndDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }

        if (misFurtherEducationEstablishment is not null
            && misFurtherEducationEstablishment.PreviousOverallEffectiveness is not null
            && !string.IsNullOrEmpty(misFurtherEducationEstablishment.PreviousLastDayOfInspection))
        {
            return new OfstedRating(
                (OfstedRatingScore)misFurtherEducationEstablishment.PreviousOverallEffectiveness.Value,
                DateTime.ParseExact(misFurtherEducationEstablishment.PreviousLastDayOfInspection!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }

        return OfstedRating.None;
    }

}

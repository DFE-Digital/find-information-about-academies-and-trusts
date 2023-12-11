using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IAcademyFactory
{
    Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishment);
}

public class AcademyFactory : IAcademyFactory
{
    public Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment,
        MisEstablishment? misEstablishment = null)
    {
        OfstedRating currentOfstedRating;
        if (misEstablishment is not null)
        {
            currentOfstedRating = new OfstedRating(
                (OfstedRatingScore)misEstablishment.OverallEffectiveness!.Value,
                DateTime.ParseExact(misEstablishment.InspectionEndDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            );
        }
        else
        {
            currentOfstedRating = OfstedRating.None;
        }

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
            currentOfstedRating,
            null
        );
    }
}

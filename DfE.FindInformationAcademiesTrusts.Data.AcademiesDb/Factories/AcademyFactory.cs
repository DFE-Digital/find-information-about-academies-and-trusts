using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Extensions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;

public interface IAcademyFactory
{
    Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment);
}

public class AcademyFactory : IAcademyFactory
{
    public Academy CreateAcademyFrom(GiasGroupLink gl, GiasEstablishment giasEstablishment)
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
            giasEstablishment.OfstedRatingName != null
                ? new OfstedRating(giasEstablishment.OfstedRatingName,
                    giasEstablishment.OfstedLastInsp.ParseAsNullableDate())
                : null,
            null
        );
    }
}

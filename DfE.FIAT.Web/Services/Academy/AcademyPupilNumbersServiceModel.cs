using DfE.FIAT.Data;

namespace DfE.FIAT.Web.Services.Academy;

public record AcademyPupilNumbersServiceModel(
    string Urn,
    string? EstablishmentName,
    string? PhaseOfEducation,
    AgeRange AgeRange,
    int? NumberOfPupils,
    int? SchoolCapacity
)
{
    public float? PercentageFull
    {
        get
        {
            if (this is { NumberOfPupils: not null, SchoolCapacity: not null } &&
                SchoolCapacity != 0)
            {
                return (float)Math.Round((int)NumberOfPupils / (float)SchoolCapacity * 100);
            }

            return null;
        }
    }
}

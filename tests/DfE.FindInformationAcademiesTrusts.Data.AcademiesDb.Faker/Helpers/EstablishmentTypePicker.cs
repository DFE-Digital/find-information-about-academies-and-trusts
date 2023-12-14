using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Helpers;

public interface IEstablishmentTypePicker
{
    public string[] GetEstablishmentTypes(GiasEstablishment establishment);
}

public class EstablishmentTypePicker : IEstablishmentTypePicker
{
    private readonly string[] _secondaryAllThroughPhaseNames =
    {
        "Secondary", "Middle Deemed Secondary", "16 Plus", "Not applicable", "All-through"
    };

    public string[] GetEstablishmentTypes(GiasEstablishment establishment)
    {
        if (establishment.PhaseOfEducationName == "Primary" ||
            establishment.PhaseOfEducationName == "Middle Deemed Primary")
        {
            return new[]
            {
                "Community School", "Voluntary Aided School", "Foundation School",
                "Voluntary Controlled School", "Academy Sponsor Led", "Academy Converter", "City Technology College",
                "Free Schools", "University technical college", "Studio schools"
            };
        }

        if (_secondaryAllThroughPhaseNames.Contains(establishment.PhaseOfEducationName))
        {
            return new[]
            {
                "Community School", "Voluntary Aided School", "Foundation School",
                "Voluntary Controlled School", "Academy 16-19 Converter", "Academy Sponsor Led", "Academy Converter",
                "City Technology College",
                "Free Schools", "Free schools 16 to 19", "University technical college", "Studio schools",
                "Academy 16 to 19 sponsor led"
            };
        }

        return new[]
        {
            "Foundation Special School", "Community Special School", "Academy Special Converter",
            "Academy Special Sponsor Led", "Free Schools Special",
            "Pupil Referral Unit", "Academy Alternative Provision Sponsor Led",
            "Free schools alternative provision", "Academy Alternative Provision Converter"
        };
    }
}

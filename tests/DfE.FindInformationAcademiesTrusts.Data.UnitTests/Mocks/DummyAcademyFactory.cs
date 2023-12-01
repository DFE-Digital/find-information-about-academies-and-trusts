namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

public class DummyAcademyFactory
{
    private int _numberAcademiesGenerated;

    public Academy GetDummyAcademy()
    {
        _numberAcademiesGenerated++;
        return GetDummyAcademy(_numberAcademiesGenerated);
    }

    public Academy[] GetDummyAcademies(int number)
    {
        var academies = new List<Academy>();

        for (var i = 0; i <= number; i++)
        {
            academies.Add(GetDummyAcademy());
        }

        return academies.ToArray();
    }

    public static Academy GetDummyAcademy(int urn,
        string typeOfEstablishment = "test",
        string localAuthority = "test",
        string urbanRural = "test",
        string phaseOfEducation = "test",
        string? numberOfPupils = "test",
        string? schoolCapacity = "test",
        string? percentageFreeSchoolMeals = "test",
        AgeRange? ageRange = null,
        OfstedRating? currentOfstedRating = null,
        OfstedRating? previousOfstedRating = null)
    {
        return new Academy(urn,
            new DateTime(2023, 11, 16),
            $"Academy {urn}",
            typeOfEstablishment,
            localAuthority,
            urbanRural,
            phaseOfEducation,
            numberOfPupils,
            schoolCapacity,
            percentageFreeSchoolMeals,
            ageRange ?? new AgeRange(1, 11),
            currentOfstedRating,
            previousOfstedRating);
    }
}

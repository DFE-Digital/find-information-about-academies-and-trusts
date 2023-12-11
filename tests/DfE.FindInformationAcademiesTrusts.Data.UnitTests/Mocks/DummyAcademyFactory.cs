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
        int? numberOfPupils = 300,
        int? schoolCapacity = 400,
        double? percentageFreeSchoolMeals = default,
        AgeRange? ageRange = null,
        int laCode = 334)
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
            OfstedRating.None,
            OfstedRating.None,
            laCode);
    }
}

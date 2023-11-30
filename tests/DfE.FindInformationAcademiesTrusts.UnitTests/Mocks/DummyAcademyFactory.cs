using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

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

    public static Academy GetDummyAcademy(int urn)
    {
        return new Academy(urn,
            new DateTime(2023, 11, 16),
            $"Academy {urn}",
            "test",
            "test",
            "test",
            "test",
            "test",
            "test",
            "test",
            null,
            null,
            null);
    }
}

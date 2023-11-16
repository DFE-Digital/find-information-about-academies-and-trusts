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

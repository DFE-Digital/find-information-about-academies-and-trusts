using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.Mocks;

public class DummyGovernorFactory
{
    private int _numberGenerated;

    public Governor GetDummyGovernor()
    {
        _numberGenerated++;
        return GetDummyGovernor(_numberGenerated.ToString());
    }

    public static Governor GetDummyGovernor(string gid)
    {
        return new Governor(gid,
            "uid",
            $"Governor {gid}",
            "test",
            "test",
            null,
            "test");
    }
}

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

public class DummyGovernorFactory
{
    private int _numberGenerated;

    public Governor GetDummyGovernor(string uid)
    {
        _numberGenerated++;
        return GetDummyGovernor(_numberGenerated.ToString(), uid);
    }

    public static Governor GetDummyGovernor(string gid, string uid)
    {
        return new Governor(gid,
            uid,
            $"Governor {gid}",
            "test",
            "test",
            null,
            null,
            "test");
    }
}

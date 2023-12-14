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

    public static Governor GetDummyGovernor(string name, string role, DateTime? dateOfTermEnd)
    {
        return new Governor("1",
            "2",
            name,
            role,
            "test",
            null,
            dateOfTermEnd,
            "email@email.com");
    }
}

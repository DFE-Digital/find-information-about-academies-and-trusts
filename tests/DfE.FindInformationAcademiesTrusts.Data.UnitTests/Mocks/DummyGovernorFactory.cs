using DfE.FindInformationAcademiesTrusts.Data.Enums;

namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

public class DummyGovernorFactory
{
    // private int _numberGenerated;
    //
    // public Governor GetDummyGovernor(string uid)
    // {
    //     _numberGenerated++;
    //     return GetDummyGovernor(_numberGenerated.ToString(), gid: uid);
    // }

    // public static Governor GetDummyGovernor(, string uid, GovernanceRole role = GovernanceRole.Member)
    // {
    //     return new Governor(gid,
    //         uid,
    //         $"Governor {gid}",
    //         role,
    //         "test",
    //         null,
    //         null,
    //         "test");
    // }

    public static Governor GetDummyGovernor(string name, DateTime? dateOfTermEnd = null, string gid = "1",
        string uid = "2", GovernanceRole role = GovernanceRole.Member)
    {
        return new Governor("1",
            uid,
            name,
            role,
            "test",
            null,
            dateOfTermEnd,
            "email@email.com");
    }
}

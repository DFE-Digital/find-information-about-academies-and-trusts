using System.Collections;

namespace DfE.FindInformationAcademiesTrusts.Data;

public record TrustSearchEntry(string Name, string Address, string Uid, string GroupId) : IEnumerable
{
    public IEnumerator GetEnumerator()
    {
        throw new NotImplementedException();
    }
}

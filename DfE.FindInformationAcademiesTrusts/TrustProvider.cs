namespace DfE.FindInformationAcademiesTrusts;

public interface ITrustProvider
{
    public Task<IEnumerable<string>> GetTrustsAsync();
}

public class TrustProvider : ITrustProvider
{
    public Task<IEnumerable<string>> GetTrustsAsync()
    {
        throw new NotImplementedException();
    }
}

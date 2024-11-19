namespace DfE.FIAT.Data;

public interface IPaginatedList<T> : IList<T>
{
    public PageStatus PageStatus { get; }
}

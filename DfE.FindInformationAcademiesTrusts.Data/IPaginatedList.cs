namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IPaginatedList<T> : IList<T>
{
    public PageStatus PageStatus { get; }
}

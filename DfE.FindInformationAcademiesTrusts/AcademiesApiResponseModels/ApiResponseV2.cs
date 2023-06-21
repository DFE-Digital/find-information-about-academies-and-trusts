namespace DfE.FindInformationAcademiesTrusts.AcademiesApiResponseModels;

public class ApiResponseV2<TResponse> where TResponse : class
{
    public IEnumerable<TResponse>? Data { get; set; }
    public PagingResponse? Paging { get; set; }
}

namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy
{
    public interface IPipelineEstablishmentRepository
    {
        Task<PipelineEstablishment[]?> GetPipelineFreeSchoolProjectsAsync(string trustReferenceNumber);
        Task<PipelineEstablishment[]?> GetAdvisoryConversionEstablishmentsAsync(string trustReferenceNumber, AdvisoryType advisoryType);
        Task<PipelineEstablishment[]?> GetAdvisoryTransferEstablishmentsAsync(string trustReferenceNumber, AdvisoryType isPostAdvisory);
        Task<PipelineSummary> GetAcademiesPipelineSummaryAsync(string trustReferenceNumber);
    }
}
namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineAcademy
{
    public interface IPipelineEstablishmentRepository
    {
        Task<PipelineEstablishment[]?> GetPipelineFreeSchoolProjects(string uid);
    }
}

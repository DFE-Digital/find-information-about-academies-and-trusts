namespace DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments
{
    public interface IPipelineEstablishmentRepository
    {
        Task<FreeSchoolProject[]?> GetPipelineFreeSchoolProjects(string uid);
    }
}

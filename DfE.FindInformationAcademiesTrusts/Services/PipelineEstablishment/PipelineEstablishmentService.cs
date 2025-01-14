using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public interface IPipelineEstablishmentService
{
    Task<FreeSchoolProject[]?> GetPipelineFreeSchools(string uid);
}

public class PipelineEstablishmentService(
    IPipelineEstablishmentRepository pipelineEstablishmentRepository)
    : IPipelineEstablishmentService
{
    public async Task<FreeSchoolProject[]?> GetPipelineFreeSchools(string uid)
    {
        FreeSchoolProject[]? freeSchools = await pipelineEstablishmentRepository.GetPipelineFreeSchoolProjects(uid);

        return
            freeSchools;
    }
}

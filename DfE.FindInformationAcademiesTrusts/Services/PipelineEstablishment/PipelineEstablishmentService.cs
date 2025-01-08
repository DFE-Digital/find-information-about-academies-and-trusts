using DfE.FindInformationAcademiesTrusts.Data.Repositories.PipelineEstablishments;

namespace DfE.FindInformationAcademiesTrusts.Services.Trust;

public interface IPipelineEstablishmentService
{
    Task<dynamic> GetTrustGovernanceAsync(string uid);
}

public class PipelineEstablishmentService(
    IPipelineEstablishmentRepository pipelineEstablishmentRepository)
    : IPipelineEstablishmentService
{
    public async Task<dynamic> GetTrustGovernanceAsync(string uid)
    {
        dynamic test = await pipelineEstablishmentRepository.GetPipelineFreeSchoolProjects(uid);

        return
            test;
    }
}

using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolOverviewSenService
{
    Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn);
}

public class SchoolOverviewSenService(ISchoolRepository schoolRepository) : ISchoolOverviewSenService
{
    public async Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn)
    {
        var senProvision = await schoolRepository.GetSchoolSenProvisionAsync(urn);
        var senProvisionTypes = new List<string>();

        foreach (var senType in senProvision.SenProvisionTypes)
        {
            senProvisionTypes.Add(senType);
        }

        var senModel = new SchoolOverviewSenServiceModel(
            senProvision.ResourcedProvisionOnRoll,
            senProvision.ResourcedProvisionCapacity,
            senProvision.SenOnRoll,
            senProvision.SenCapacity,
            senProvision.ResourcedProvisionTypes,
            senProvisionTypes);
        
        return senModel;
    }
}


using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolOverviewSenService
{
    Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn);
}

public class SchoolOverviewSenService(ISchoolRepository schoolRepository) : ISchoolOverviewSenService
{
    private List<string> SenProvisionTypes { get; } = new();

    public async Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn)
    {
        var senProvision = await schoolRepository.GetSchoolSenProvisionAsync(urn);

        foreach (var senType in senProvision.SenProvisionTypes)
        {
            if (senType != null)
            {
                SenProvisionTypes.Add(senType);
            }
        }

        var senModel = new SchoolOverviewSenServiceModel(
            senProvision.ResourcedProvisionOnRoll,
            senProvision.ResourcedProvisionCapacity,
            senProvision.SenOnRoll,
            senProvision.SenCapacity,
            senProvision.ResourcedProvisionTypes,
            SenProvisionTypes);
        
        return senModel;
    }
}


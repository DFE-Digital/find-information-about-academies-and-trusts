using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolOverviewSenService
{
    Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn);
}

public class SchoolOverviewSenService(ISchoolRepository schoolRepository) : ISchoolOverviewSenService
{
    public List<string> SenProvisionTypes { get; } = new();

    public async Task<SchoolOverviewSenServiceModel> GetSchoolOverviewSenAsync(int urn)
    {
        var senProvision = await schoolRepository.GetSchoolSenProvisionAsync(urn);

        AddSenProvisionType(senProvision.Sen1);
        AddSenProvisionType(senProvision.Sen2);
        AddSenProvisionType(senProvision.Sen3);
        AddSenProvisionType(senProvision.Sen4);
        AddSenProvisionType(senProvision.Sen5);
        AddSenProvisionType(senProvision.Sen6);
        AddSenProvisionType(senProvision.Sen7);
        AddSenProvisionType(senProvision.Sen8);
        AddSenProvisionType(senProvision.Sen9);
        AddSenProvisionType(senProvision.Sen10);
        AddSenProvisionType(senProvision.Sen11);
        AddSenProvisionType(senProvision.Sen12);
        AddSenProvisionType(senProvision.Sen13);

        var senModel = new SchoolOverviewSenServiceModel(
            senProvision.ResourcedProvisionOnRoll,
            senProvision.ResourcedProvisionCapacity,
            senProvision.SenOnRoll,
            senProvision.SenCapacity,
            senProvision.ResourcedProvisionTypes,
            SenProvisionTypes);
        
        return senModel;
    }

    public void AddSenProvisionType(string? senProvisionType)
    {
        if (senProvisionType != null)
        {
            SenProvisionTypes.Add(senProvisionType);
        }
    }
}


using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolOverviewDetailsService
{
    Task<SchoolOverviewServiceModel?> GetSchoolOverviewDetailsAsync(int urn, SchoolCategory schoolCategory);
}

public class SchoolOverviewDetailsService(ISchoolRepository schoolRepository) : ISchoolOverviewDetailsService
{
    public async Task<SchoolOverviewServiceModel?> GetSchoolOverviewDetailsAsync(int urn, SchoolCategory schoolCategory)
    {
        var schoolDetails = await schoolRepository.GetSchoolDetailsAsync(urn);

        if (schoolDetails is null)
        {
            return null;
        }

        var nurseryProvision = GetNurseryProvision(schoolDetails.NurseryProvision);

        var overviewModel = new SchoolOverviewServiceModel(schoolDetails.Name, schoolDetails.Address,
            schoolDetails.Region, schoolDetails.LocalAuthority, schoolDetails.PhaseOfEducationName,
            schoolDetails.AgeRange, nurseryProvision);

        if (schoolCategory is SchoolCategory.LaMaintainedSchool)
        {
            return overviewModel;
        }

        var dateJoinedTrust = await schoolRepository.GetDateJoinedTrustAsync(urn);

        return overviewModel with { DateJoinedTrust = dateJoinedTrust };
    }

    private static NurseryProvision GetNurseryProvision(string? nurseryProvisionString)
    {
        return nurseryProvisionString?.ToLower() switch
        {
            "has nursery classes" => NurseryProvision.HasClasses,
            "no nursery classes" => NurseryProvision.NoClasses,
            _ => NurseryProvision.NotRecorded
        };
    }
}

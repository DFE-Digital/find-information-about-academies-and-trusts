using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;

namespace DfE.FindInformationAcademiesTrusts.Services.School;

public interface ISchoolOverviewFederationService
{
    Task<SchoolOverviewFederationServiceModel> GetSchoolOverviewFederationAsync(int urn);
}

public class SchoolOverviewFederationService(ISchoolRepository schoolRepository) : ISchoolOverviewFederationService
{
    public async Task<SchoolOverviewFederationServiceModel> GetSchoolOverviewFederationAsync(int urn)
    {
        var federationDetails = await schoolRepository.GetSchoolFederationDetailsAsync(urn);

        var federationModel = new SchoolOverviewFederationServiceModel(
            federationDetails.Name,
            federationDetails.FederationUid,
            federationDetails.OpenedOnDate,
            federationDetails.Schools);

        return federationModel;
    }
}

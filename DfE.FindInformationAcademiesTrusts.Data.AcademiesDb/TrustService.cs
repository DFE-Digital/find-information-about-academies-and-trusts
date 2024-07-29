using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;
using DfE.FindInformationAcademiesTrusts.Data.Dto;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface ITrustService
{
    Task<TrustSummaryDto?> GetTrustSummaryAsync(string uid);
    Task<TrustDetailsDto> GetTrustDetailsAsync(string uid);
}

public class TrustService(
    IAcademyRepository academyRepository,
    ITrustRepository trustRepository)
    : ITrustService
{
    public async Task<TrustSummaryDto?> GetTrustSummaryAsync(string uid)
    {
        var summary = await trustRepository.GetTrustSummaryAsync(uid);

        if (summary is null)
        {
            return null;
        }

        var count = await academyRepository.GetNumberOfAcademiesInTrustAsync(uid);

        return new TrustSummaryDto(uid, summary.Name, summary.Type, count);
    }

    public async Task<TrustDetailsDto> GetTrustDetailsAsync(string uid)
    {
        var singleAcademyUrn = await academyRepository.GetUrnForSingleAcademyTrustAsync(uid);

        var trustDetails = await trustRepository.GetTrustDetailsAsync(uid);

        var trustDetailsDto = new TrustDetailsDto(
            trustDetails.Uid,
            trustDetails.GroupId,
            trustDetails.Ukprn,
            trustDetails.CompaniesHouseNumber,
            trustDetails.Type,
            trustDetails.Address,
            trustDetails.RegionAndTerritory,
            singleAcademyUrn,
            trustDetails.OpenedDate
        );

        return trustDetailsDto;
    }
}

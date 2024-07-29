using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.RepositoryDto;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public interface ITrustRepository
{
    Task<TrustSummaryRepoDto?> GetTrustSummaryAsync(string uid);
}

public class TrustRepository(IAcademiesDbContext academiesDbContext) : ITrustRepository
{
    public async Task<TrustSummaryRepoDto?> GetTrustSummaryAsync(string uid)
    {
        var details = await academiesDbContext.Groups
            .Where(g => g.GroupUid == uid)
            .Select(g => new { Name = g.GroupName ?? string.Empty, Type = g.GroupType ?? string.Empty })
            .SingleOrDefaultAsync();

        return details is null ? null : new TrustSummaryRepoDto(details.Name, details.Type);
    }
}

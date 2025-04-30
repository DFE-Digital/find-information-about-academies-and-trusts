using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class SchoolRepository(IAcademiesDbContext academiesDbContext) : ISchoolRepository
{
    public async Task<SchoolSummary?> GetSchoolSummaryAsync(int urn)
    {
        return await academiesDbContext.GiasEstablishments
            .Where(e => e.Urn == urn)
            .Select(e => new SchoolSummary(
                e.EstablishmentName!,
                e.TypeOfEstablishmentName!,
                e.EstablishmentTypeGroupName == "Academies"
                    ? SchoolCategory.Academy
                    : SchoolCategory.LaMaintainedSchool))
            .SingleOrDefaultAsync();
    }
}

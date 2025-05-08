using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class SchoolRepository(IAcademiesDbContext academiesDbContext, IStringFormattingUtilities stringFormattingUtilities) : ISchoolRepository
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

    public async Task<SchoolDetails?> GetSchoolDetailsAsync(int urn)
    {
       return await academiesDbContext.GiasEstablishments
            .Where(e => e.Urn == urn)
            .Select(establishment => new SchoolDetails(establishment.EstablishmentName!,
                stringFormattingUtilities.BuildAddressString(
                    establishment.Street,
                    null,
                    establishment.Town,
                    establishment.Postcode
                ),
                establishment.GorName!,
                establishment.LaName!,
                establishment.PhaseOfEducationName!,
                new AgeRange(establishment.StatutoryLowAge!, establishment.StatutoryHighAge!),
                establishment.NurseryProvisionName))
            .SingleOrDefaultAsync();
    }

    public async Task<DateOnly> GetDateJoinedTrustAsync(int urn)
    {
        return await academiesDbContext.GiasGroupLinks.Where(gl => gl.Urn == urn.ToString())
            .Select(gl => DateOnly.ParseExact(gl.JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None))
            .FirstAsync();
    }
}

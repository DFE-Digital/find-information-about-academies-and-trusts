using System.Globalization;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.School;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Repositories;

public class SchoolRepository(
    IAcademiesDbContext academiesDbContext,
    IStringFormattingUtilities stringFormattingUtilities) : ISchoolRepository
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

    public async Task<SchoolDetails> GetSchoolDetailsAsync(int urn)
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
            .SingleAsync();
    }

    public async Task<DateOnly> GetDateJoinedTrustAsync(int urn)
    {
        return await academiesDbContext.GiasGroupLinks.Where(gl => gl.Urn == urn.ToString())
            .Select(gl =>
                DateOnly.ParseExact(gl.JoinedDate!, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None))
            .FirstAsync();
    }

    public async Task<SchoolContacts> GetSchoolContactsAsync(int urn)
    {
        return await academiesDbContext.TadHeadTeacherContacts
            .Where(c => c.Urn == urn)
            .Select(contact => new SchoolContacts(contact.HeadFirstName, contact.HeadLastName, contact.HeadEmail))
            .SingleAsync();
    }

    public async Task<SenProvision> GetSchoolSenProvisionAsync(int urn)
    {
        return await academiesDbContext.GiasEstablishments
            .Where(e => e.Urn == urn)
            .Select(establishment => new SenProvision(
                establishment.ResourcedProvisionOnRoll!,
                establishment.ResourcedProvisionCapacity!,
                establishment.SenUnitOnRoll!,
                establishment.SenUnitCapacity!,
                establishment.TypeOfResourcedProvisionName!,
                new List<string>
                {
                    establishment.Sen1Name!,
                    establishment.Sen2Name!,
                    establishment.Sen3Name!,
                    establishment.Sen4Name!,
                    establishment.Sen5Name!,
                    establishment.Sen6Name!,
                    establishment.Sen7Name!,
                    establishment.Sen8Name!,
                    establishment.Sen9Name!,
                    establishment.Sen10Name!,
                    establishment.Sen11Name!,
                    establishment.Sen12Name!,
                    establishment.Sen13Name!
                }
            ))
            .SingleAsync();
    }
}

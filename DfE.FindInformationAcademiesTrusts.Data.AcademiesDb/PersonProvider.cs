using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IPersonProvider
{
    Task<Person?> GetTrustRelationshipManagerLinkedTo(string uid);
    Task<Person?> GetSfsoLeadLinkedTo(string uid);
}

public class PersonProvider : IPersonProvider
{
    private readonly IAcademiesDbContext _academiesDbContext;
    private readonly IPersonFactory _personFactory;

    [ExcludeFromCodeCoverage]
    public PersonProvider(AcademiesDbContext academiesDbContext, IPersonFactory personFactory):this((IAcademiesDbContext)academiesDbContext, personFactory)
    { }

    public PersonProvider(IAcademiesDbContext academiesDbContext, IPersonFactory personFactory)
    {
        _academiesDbContext = academiesDbContext;
        _personFactory = personFactory;
    }

    public async Task<Person?> GetTrustRelationshipManagerLinkedTo(string uid)
    {
        var personId= await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Select(cdmAccount => cdmAccount.SipTrustrelationshipmanager)
            .SingleOrDefaultAsync();

        return personId is null ? null : await GetPerson(personId.Value);
    }

    public async Task<Person?> GetSfsoLeadLinkedTo(string uid)
    {
        var personId= await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Select(cdmAccount => cdmAccount.SipAmsdlead)
            .SingleOrDefaultAsync();

        return personId is null ? null : await GetPerson(personId.Value);
    }

    private async Task<Person?> GetPerson(Guid personId)
    {
        return await _academiesDbContext.CdmSystemusers
            .Where(systemuser => systemuser.Systemuserid == personId)
            .Select(systemuser => _personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();
    }
}

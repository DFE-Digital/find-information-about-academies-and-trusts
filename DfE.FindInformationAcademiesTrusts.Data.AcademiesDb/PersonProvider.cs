using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

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
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PersonProvider> _logger;

    // [ExcludeFromCodeCoverage]
    // public PersonProvider(AcademiesDbContext academiesDbContext, IPersonFactory personFactory, IMemoryCache memoryCache,
    //     ILogger<PersonProvider> logger) : this((IAcademiesDbContext)academiesDbContext, personFactory, memoryCache,
    //     logger)
    // {
    // }

    public PersonProvider(IAcademiesDbContext academiesDbContext, IPersonFactory personFactory,
        IMemoryCache memoryCache, ILogger<PersonProvider> logger)
    {
        _academiesDbContext = academiesDbContext;
        _personFactory = personFactory;
        _memoryCache = memoryCache;
        _logger = logger;
    }

    public async Task<Person?> GetTrustRelationshipManagerLinkedTo(string uid)
    {
        var personId = await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Select(cdmAccount => cdmAccount.SipTrustrelationshipmanager)
            .SingleOrDefaultAsync();

        return personId is null ? null : await GetPerson(personId.Value);
    }

    public async Task<Person?> GetSfsoLeadLinkedTo(string uid)
    {
        var personId = await _academiesDbContext.CdmAccounts
            .Where(a => a.SipUid == uid)
            .Select(cdmAccount => cdmAccount.SipAmsdlead)
            .SingleOrDefaultAsync();

        return personId is null ? null : await GetPerson(personId.Value);
    }

    private async Task<Person?> GetPerson(Guid personId)
    {
        if (_memoryCache.TryGetValue(personId, out Person? person))
        {
            return person;
        }

        person = await _academiesDbContext.CdmSystemusers
            .Where(systemuser => systemuser.Systemuserid == personId)
            .Select(systemuser => _personFactory.CreateFrom(systemuser))
            .SingleOrDefaultAsync();

        if (person is null)
        {
            _logger.LogError(
                "Person not found with ID {personId}, is there a consistency error with the CDM tables in the database?",
                personId);
        }

        _memoryCache.Set(personId, person, TimeSpan.FromHours(1));

        return person;
    }
}

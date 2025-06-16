using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Contacts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;

public interface IContactRepository
{
    Task<TrustInternalContacts> GetTrustInternalContactsAsync(string uid);

    Task<TrustContactUpdated> UpdateTrustInternalContactsAsync(int uid, string? name, string? email,
        TrustContactRole role);
}

public class ContactRepository(FiatDbContext fiatDbContext) : IContactRepository
{
    public async Task<TrustInternalContacts> GetTrustInternalContactsAsync(string uid)
    {
        var trm = await GetTrustRelationshipManagerLinkedTo(uid);
        var sfso = await GetSfsoLeadLinkedTo(uid);

        return new TrustInternalContacts(
            trm,
            sfso);
    }

    public async Task<TrustContactUpdated> UpdateTrustInternalContactsAsync(int uid, string? name, string? email,
        TrustContactRole role)
    {
        var contact = await fiatDbContext.TrustContacts
            .SingleOrDefaultAsync(contact => contact.Uid == uid && contact.Role == role);
        if (contact is null)
        {
            return await AddNewContact(uid, name, email, role);
        }

        var nameUpdated = false;
        var emailUpdated = false;
        if (contact.Name != name)
        {
            nameUpdated = true;
            contact.Name = name ?? string.Empty;
        }

        if (contact.Email != email)
        {
            emailUpdated = true;
            contact.Email = email ?? string.Empty;
        }

        await fiatDbContext.SaveChangesAsync();
        return new TrustContactUpdated(emailUpdated, nameUpdated);
    }

    private async Task<TrustContactUpdated> AddNewContact(int uid, string? name, string? email, TrustContactRole role)
    {
        fiatDbContext.TrustContacts.Add(new TrustContact
        {
            Name = name ?? string.Empty,
            Email = email ?? string.Empty,
            Role = role,
            Uid = uid
        });
        await fiatDbContext.SaveChangesAsync();
        return new TrustContactUpdated(true, true);
    }

    private async Task<InternalContact?> GetTrustRelationshipManagerLinkedTo(string uid)
    {
        return await fiatDbContext.TrustContacts.Where(contact =>
                contact.Uid == int.Parse(uid) && contact.Role == TrustContactRole.TrustRelationshipManager)
            .Select(contact => new InternalContact(contact.Name, contact.Email,
                contact.LastModifiedAtTime, contact.LastModifiedByEmail
            )).SingleOrDefaultAsync();
    }

    private async Task<InternalContact?> GetSfsoLeadLinkedTo(string uid)
    {
        return await fiatDbContext.TrustContacts.Where(contact =>
                contact.Uid == int.Parse(uid) && contact.Role == TrustContactRole.SfsoLead)
            .Select(contact => new InternalContact(contact.Name, contact.Email,
                contact.LastModifiedAtTime, contact.LastModifiedByEmail
            )).SingleOrDefaultAsync();
    }
}

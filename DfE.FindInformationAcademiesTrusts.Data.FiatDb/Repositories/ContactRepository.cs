using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Contacts;
using DfE.FindInformationAcademiesTrusts.Data.Repositories.Trust;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;

public interface IContactRepository
{
    Task<InternalContacts> GetInternalContactsAsync(string uid);

    Task<TrustContactUpdated> UpdateInternalContactsAsync(int uid, string? name, string? email,
        ContactRole role);
    Task<Dictionary<string, InternalContacts>> GetInternalContactsAsync(IEnumerable<string> uids);
}

public class ContactRepository(FiatDbContext fiatDbContext) : IContactRepository
{
    public async Task<InternalContacts> GetInternalContactsAsync(string uid)
    {
        var trm = await GetTrustRelationshipManagerLinkedTo(uid);
        var sfso = await GetSfsoLeadLinkedTo(uid);

        return new InternalContacts(
            trm,
            sfso);
    }

    public async Task<TrustContactUpdated> UpdateInternalContactsAsync(int uid, string? name, string? email,
        ContactRole role)
    {
        var contact = await fiatDbContext.Contacts
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

    private async Task<TrustContactUpdated> AddNewContact(int uid, string? name, string? email, ContactRole role)
    {
        fiatDbContext.Contacts.Add(new Contact
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
        return await fiatDbContext.Contacts.Where(contact =>
                contact.Uid == int.Parse(uid) && contact.Role == ContactRole.TrustRelationshipManager)
            .Select(contact => new InternalContact(contact.Name, contact.Email,
                contact.LastModifiedAtTime, contact.LastModifiedByEmail
            )).SingleOrDefaultAsync();
    }

    private async Task<InternalContact?> GetSfsoLeadLinkedTo(string uid)
    {
        return await fiatDbContext.Contacts.Where(contact =>
                contact.Uid == int.Parse(uid) && contact.Role == ContactRole.SfsoLead)
            .Select(contact => new InternalContact(contact.Name, contact.Email,
                contact.LastModifiedAtTime, contact.LastModifiedByEmail
            )).SingleOrDefaultAsync();
    }

    public async Task<Dictionary<string, InternalContacts>> GetInternalContactsAsync(IEnumerable<string> uids)
    {
        var uidInts = uids.Select(uid => int.Parse(uid)).ToArray();

        // Retrieve contacts for all UIDs in one query
        var contacts = await fiatDbContext.Contacts
            .Where(contact => uidInts.Contains(contact.Uid) &&
                              (contact.Role == ContactRole.TrustRelationshipManager || contact.Role == ContactRole.SfsoLead))
            .Select(contact => new
            {
                contact.Uid,
                contact.Role,
                contact.Name,
                contact.Email,
                contact.LastModifiedAtTime,
                contact.LastModifiedByEmail
            })
            .ToListAsync();

        // Group contacts by UID
        var groupedContacts = contacts.GroupBy(c => c.Uid);

        var internalContactsDict = new Dictionary<string, InternalContacts>();

        foreach (var group in groupedContacts)
        {
            var uid = group.Key.ToString();

            var trmContact = group.FirstOrDefault(c => c.Role == ContactRole.TrustRelationshipManager);
            InternalContact? trm = null;
            if (trmContact != null)
            {
                trm = new InternalContact(
                    trmContact.Name,
                    trmContact.Email,
                    trmContact.LastModifiedAtTime,
                    trmContact.LastModifiedByEmail ?? string.Empty
                );
            }

            var sfsoContact = group.FirstOrDefault(c => c.Role == ContactRole.SfsoLead);
            InternalContact? sfso = null;
            if (sfsoContact != null)
            {
                sfso = new InternalContact(
                    sfsoContact.Name,
                    sfsoContact.Email,
                    sfsoContact.LastModifiedAtTime,
                    sfsoContact.LastModifiedByEmail ?? string.Empty
                );
            }

            internalContactsDict[uid] = new InternalContacts(trm, sfso);
        }

        return internalContactsDict;
    }

}

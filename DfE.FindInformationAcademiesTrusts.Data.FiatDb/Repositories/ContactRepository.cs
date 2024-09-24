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
}

public class ContactRepository(IFiatDbContext fiatDbContext) : IContactRepository
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
        var emailUpdated = false;
        var nameUpdated = false;
        var contact =
            await fiatDbContext.Contacts.SingleOrDefaultAsync(contact => contact.Uid == uid && contact.Role == role);
        if (contact is null)
        {
            fiatDbContext.Contacts.Add(new Contact
                { Name = name ?? string.Empty, Email = email ?? string.Empty, Role = role, Uid = uid });
        }
        else
        {
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
        }

        await fiatDbContext.SaveChangesAsync();
        return new TrustContactUpdated(emailUpdated, nameUpdated);
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
}

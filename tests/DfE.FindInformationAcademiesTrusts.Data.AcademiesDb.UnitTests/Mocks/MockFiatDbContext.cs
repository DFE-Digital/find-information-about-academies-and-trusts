using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

public class MockFiatDbContext : Mock<IFiatDbContext>
{
    private readonly List<Contact> _contacts = [];

    public MockFiatDbContext()
    {
        SetupMockDbContext(_contacts, context => context.Contacts);
        Setup(context => context.SaveChangesAsync());
    }

    private void SetupMockDbContext<T>(List<T> items, Expression<Func<IFiatDbContext, DbSet<T>>> dbContextTable)
        where T : class
    {
        Setup(dbContextTable).Returns(new MockDbSet<T>(items).Object);
    }

    public Contact CreateTrustRelationshipManager(int uid, string fullName, string email)
    {
        var contact = new Contact
            { Email = email, Name = fullName, Role = ContactRole.TrustRelationshipManager, Uid = uid };
        _contacts.Add(contact);
        return contact;
    }

    public Contact CreateSfsoLead(int uid, string fullName, string email)
    {
        var contact = new Contact { Email = email, Name = fullName, Role = ContactRole.SfsoLead, Uid = uid };
        _contacts.Add(contact);
        return contact;
    }
}

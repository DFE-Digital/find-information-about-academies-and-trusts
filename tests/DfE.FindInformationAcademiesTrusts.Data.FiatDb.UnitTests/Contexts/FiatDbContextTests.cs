using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests.Contexts;

public class FiatDbContextTests(FiatDbContainerFixture fiatDbContainerFixture) : BaseFiatDbTest(fiatDbContainerFixture)
{
    [Fact]
    public void Contacts_should_have_autogenerated_id_column()
    {
        FiatDbContext.Contacts.AddRange(
            new Contact
            {
                Name = "My TrustRelationshipManager",
                Email = "my.TrustRelationshipManager@education.gov.uk",
                Uid = 1,
                Role = ContactRole.TrustRelationshipManager
            },
            new Contact
            {
                Name = "My TrustRelationshipManager2",
                Email = "my.TrustRelationshipManager2@education.gov.uk",
                Uid = 2,
                Role = ContactRole.TrustRelationshipManager
            },
            new Contact
            {
                Name = "My TrustRelationshipManager3",
                Email = "my.TrustRelationshipManager2@education.gov.uk",
                Uid = 3,
                Role = ContactRole.TrustRelationshipManager
            });

        FiatDbContext.SaveChanges();

        FiatDbContext.Contacts.Select(c => c.Id).Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void Contacts_should_have_autogenerated_last_modified_column()
    {
        var entry = FiatDbContext.Contacts.Add(new Contact
        {
            Name = "My TrustRelationshipManager",
            Email = "my.TrustRelationshipManager@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        }).Entity;

        FiatDbContext.SaveChanges();

        entry.LastModifiedAtTime.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Contacts_should_have_unique_uid_role_combinations()
    {
        FiatDbContext.Contacts.Add(new Contact
        {
            Name = "My TrustRelationshipManager",
            Email = "my.TrustRelationshipManager@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        });
        FiatDbContext.Contacts.Add(new Contact
        {
            Name = "other TrustRelationshipManager",
            Email = "other.TrustRelationshipManager@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        });

        var action = () => FiatDbContext.SaveChanges();

        action.Should().Throw<DbUpdateException>();
    }

    [Fact]
    public async Task Contacts_should_retain_history_of_changes()
    {
        var entry = FiatDbContext.Contacts.Add(new Contact
        {
            Name = "original name",
            Email = "original.email@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        }).Entity;

        await FiatDbContext.SaveChangesAsync();

        // ensure that some time has passed before updating the entity because the temporal table will overwrite entries
        // with the same timestamp, causing this test to be flaky
        await Task.Delay(10);

        entry.Name = "new name";

        await FiatDbContext.SaveChangesAsync();
        await Task.Delay(10);

        entry.Email = "new.email@education.gov.uk";

        await FiatDbContext.SaveChangesAsync();
        await Task.Delay(10);

        var allVersions = await FiatDbContext.Contacts.TemporalAll().Where(c => c.Uid == 1234).ToArrayAsync();

        allVersions.Should().Satisfy(
            [
                contact => contact.Name == "original name" && contact.Email == "original.email@education.gov.uk",
                contact => contact.Name == "new name" && contact.Email == "original.email@education.gov.uk",
                contact => contact.Name == "new name" && contact.Email == "new.email@education.gov.uk"
            ]
            , $"there should be three versions of the history in {string.Join(", ", allVersions.Select(c => $"[{c.Name}, {c.Email}, {c.LastModifiedAtTime:O}]"))}");
    }
}

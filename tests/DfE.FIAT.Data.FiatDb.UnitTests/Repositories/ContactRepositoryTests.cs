using DfE.FIAT.Data.Enums;
using DfE.FIAT.Data.FiatDb.Models;
using DfE.FIAT.Data.FiatDb.Repositories;
using DfE.FIAT.Data.FiatDb.UnitTests.TestContainer;
using FluentAssertions.Execution;

namespace DfE.FIAT.Data.FiatDb.UnitTests.Repositories;

public class ContactRepositoryTests : BaseFiatDbTest
{
    private readonly ContactRepository _sut;

    public ContactRepositoryTests(FiatDbContainerFixture fiatDbContainerFixture) : base(fiatDbContainerFixture)
    {
        _sut = new ContactRepository(FiatDbContext);
    }

    [Fact]
    public async Task GetInternalContactsAsync_should_return_nulls_when_there_are_no_contacts_for_trust()
    {
        var result = await _sut.GetInternalContactsAsync("1234");

        using (new AssertionScope())
        {
            result.TrustRelationshipManager.Should().BeNull();
            result.SfsoLead.Should().BeNull();
        }
    }

    [Fact]
    public async Task GetInternalContactsAsync_should_return_TrustRelationshipManager_when_there_is_no_SfsoLead()
    {
        FiatDbContext.Contacts.Add(new Contact
        {
            Email = "trm@testemail.com",
            Name = "Trust Relationship Manager",
            Role = ContactRole.TrustRelationshipManager,
            Uid = 1234
        });
        await FiatDbContext.SaveChangesAsync();

        var result = await _sut.GetInternalContactsAsync("1234");

        using (new AssertionScope())
        {
            result.TrustRelationshipManager.Should().NotBeNull();
            result.TrustRelationshipManager!.FullName.Should().Be("Trust Relationship Manager");
            result.TrustRelationshipManager!.Email.Should().Be("trm@testemail.com");

            result.SfsoLead.Should().BeNull();
        }
    }

    [Fact]
    public async Task GetInternalContactsAsync_should_return_SfsoLead_when_there_is_no_TrustRelationshipManager()
    {
        FiatDbContext.Contacts.Add(new Contact
        {
            Email = "sfsolead@testemail.com",
            Name = "SFSO Lead",
            Role = ContactRole.SfsoLead,
            Uid = 1234
        });
        await FiatDbContext.SaveChangesAsync();

        var result = await _sut.GetInternalContactsAsync("1234");

        using (new AssertionScope())
        {
            result.SfsoLead.Should().NotBeNull();
            result.SfsoLead!.FullName.Should().Be("SFSO Lead");
            result.SfsoLead!.Email.Should().Be("sfsolead@testemail.com");

            result.TrustRelationshipManager.Should().BeNull();
        }
    }

    [Fact]
    public async Task
        GetInternalContactsAsync_should_return_SfsoLead_and_TrustRelationshipManager_when_both_are_present()
    {
        await FiatDbContext.Contacts.AddRangeAsync([
            new Contact
            {
                Email = "trm@testemail.com",
                Name = "Trust Relationship Manager",
                Role = ContactRole.TrustRelationshipManager,
                Uid = 1234
            },
            new Contact
            {
                Email = "sfsolead@testemail.com",
                Name = "SFSO Lead",
                Role = ContactRole.SfsoLead,
                Uid = 1234
            }
        ]);
        await FiatDbContext.SaveChangesAsync();

        var result = await _sut.GetInternalContactsAsync("1234");

        using (new AssertionScope())
        {
            result.TrustRelationshipManager.Should().NotBeNull();
            result.TrustRelationshipManager!.FullName.Should().Be("Trust Relationship Manager");
            result.TrustRelationshipManager!.Email.Should().Be("trm@testemail.com");

            result.SfsoLead.Should().NotBeNull();
            result.SfsoLead!.FullName.Should().Be("SFSO Lead");
            result.SfsoLead!.Email.Should().Be("sfsolead@testemail.com");
        }
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_should_be_able_to_add_new_contact(ContactRole role)
    {
        var result = await _sut.UpdateInternalContactsAsync(1234, "New Name", "new@email.com", role);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeTrue();
            result.EmailUpdated.Should().BeTrue();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();

            var contact = FiatDbContext.Contacts.Single(c => c.Uid == 1234);
            contact.Name.Should().Be("New Name");
            contact.Email.Should().Be("new@email.com");
            contact.Role.Should().Be(role);
        }
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_should_be_able_to_update_both_email_and_name(ContactRole role)
    {
        var contact = new Contact
        {
            Email = "oldemail@testemail.com",
            Name = "Old Name",
            Role = role,
            Uid = 1234
        };
        FiatDbContext.Contacts.Add(contact);
        await FiatDbContext.SaveChangesAsync();

        var result = await _sut.UpdateInternalContactsAsync(1234, "New Name", "new@email.com", role);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeTrue();
            result.EmailUpdated.Should().BeTrue();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();
            contact.Name.Should().Be("New Name");
            contact.Email.Should().Be("new@email.com");
        }
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_should_be_able_to_update_name_only(ContactRole role)
    {
        var contact = new Contact
        {
            Email = "oldemail@testemail.com",
            Name = "Old Name",
            Role = role,
            Uid = 1234
        };
        FiatDbContext.Contacts.Add(contact);
        await FiatDbContext.SaveChangesAsync();

        var result =
            await _sut.UpdateInternalContactsAsync(1234, "New Name", contact.Email, role);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeTrue();
            result.EmailUpdated.Should().BeFalse();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();
            contact.Name.Should().Be("New Name");
            contact.Email.Should().Be("oldemail@testemail.com");
        }
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_should_be_able_to_update_email_only(ContactRole role)
    {
        var contact = new Contact
        {
            Email = "oldemail@testemail.com",
            Name = "Old Name",
            Role = role,
            Uid = 1234
        };
        FiatDbContext.Contacts.Add(contact);
        await FiatDbContext.SaveChangesAsync();

        var result =
            await _sut.UpdateInternalContactsAsync(1234, contact.Name, "new@email.com", role);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeFalse();
            result.EmailUpdated.Should().BeTrue();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();
            contact.Name.Should().Be("Old Name");
            contact.Email.Should().Be("new@email.com");
        }
    }

    [Fact]
    public async Task UpdateInternalContactsAsync_should_default_null_fields_to_empty_string_on_update()
    {
        var contact = new Contact
        {
            Email = "oldemail@testemail.com",
            Name = "Old Name",
            Role = ContactRole.SfsoLead,
            Uid = 1234
        };
        FiatDbContext.Contacts.Add(contact);
        await FiatDbContext.SaveChangesAsync();

        var result =
            await _sut.UpdateInternalContactsAsync(1234, null, null, contact.Role);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeTrue();
            result.EmailUpdated.Should().BeTrue();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();
            contact.Name.Should().BeEmpty();
            contact.Email.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task UpdateInternalContactsAsync_should_default_null_fields_to_empty_string_on_new_contact()
    {
        var result =
            await _sut.UpdateInternalContactsAsync(1234, null, null, ContactRole.TrustRelationshipManager);

        using (new AssertionScope())
        {
            result.NameUpdated.Should().BeTrue();
            result.EmailUpdated.Should().BeTrue();

            FiatDbContext.ChangeTracker.HasChanges().Should().BeFalse();
            var contact = FiatDbContext.Contacts.Single(c => c.Uid == 1234);
            contact.Name.Should().BeEmpty();
            contact.Email.Should().BeEmpty();
        }
    }
}

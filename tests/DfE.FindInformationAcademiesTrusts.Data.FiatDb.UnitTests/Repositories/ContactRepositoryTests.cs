using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Repositories;
using FluentAssertions;
using Moq;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests.Repositories;

public class ContactRepositoryTests
{
    private readonly ContactRepository _sut;
    private readonly MockFiatDbContext _mockFiatDbContext = new();

    public ContactRepositoryTests()
    {
        _sut = new ContactRepository(_mockFiatDbContext.Object);
    }

    [Fact]
    public async Task
        GetInternalContactsAsync_Should_Return_Valid_TrustRelationshipManager_WhenOneIsPresentForTheTrust()
    {
        var trm = _mockFiatDbContext.CreateTrustRelationshipManager(1234, "Trust Relationship Manager",
            "trm@testemail.com");
        var result = await _sut.GetInternalContactsAsync("1234");
        result.TrustRelationshipManager.Should()
            .BeEquivalentTo(trm, options => options.WithAutoConversion().ExcludingMissingMembers());
    }

    [Fact]
    public async Task GetInternalContactsAsync_Should_Return_Valid_SFSOLead_WhenOneIsPresentForTheTrust()
    {
        var sfsoLead = _mockFiatDbContext.CreateSfsoLead(1234, "SFSO Lead", "sfsolead@testemail.com");
        var result = await _sut.GetInternalContactsAsync("1234");
        result.SfsoLead.Should()
            .BeEquivalentTo(sfsoLead, options => options.WithAutoConversion().ExcludingMissingMembers());
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_Should_Return_Valid_When_email_and_name_are_changed(ContactRole role)
    {
        if (role == ContactRole.SfsoLead)
        {
            _ = _mockFiatDbContext.CreateSfsoLead(1234, "SFSO Lead", "sfsolead@testemail.com");
        }

        if (role == ContactRole.TrustRelationshipManager)
        {
            _ = _mockFiatDbContext.CreateTrustRelationshipManager(1234, "Trm Lead", "trm@testemail.com");
        }

        var result = await _sut.UpdateInternalContactsAsync(1234, "New Name", "new@email.com", role);
        result.EmailUpdated.Should().Be(true);
        result.NameUpdated.Should().Be(true);
        _mockFiatDbContext.Verify(context => context.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(ContactRole.TrustRelationshipManager)]
    [InlineData(ContactRole.SfsoLead)]
    public async Task UpdateInternalContactsAsync_Should_Return_Valid_When_the_contact_does_not_exist(ContactRole role)
    {
        var result = await _sut.UpdateInternalContactsAsync(1234, "New Name", "new@email.com", role);
        result.EmailUpdated.Should().Be(true);
        result.NameUpdated.Should().Be(true);
        _mockFiatDbContext.Verify(context => context.SaveChangesAsync(), Times.Once);
    }

    public enum FieldToUpdate
    {
        email,
        name
    }

    [Theory]
    [InlineData(FieldToUpdate.name)]
    [InlineData(FieldToUpdate.email)]
    public async Task UpdateInternalContactsAsync_Should_Return_Valid_When_Only_one_field_is_changed(
        FieldToUpdate field)
    {
        if (field == FieldToUpdate.name)
        {
            _ = _mockFiatDbContext.CreateSfsoLead(1234, "SFSO Lead", "sfsolead@testemail.com");
            var result =
                await _sut.UpdateInternalContactsAsync(1234, "New Name", "sfsolead@testemail.com",
                    ContactRole.SfsoLead);
            result.EmailUpdated.Should().Be(false);
            result.NameUpdated.Should().Be(true);
        }

        else
        {
            _ = _mockFiatDbContext.CreateSfsoLead(1234, "SFSO Lead", "sfsolead@testemail.com");
            var result =
                await _sut.UpdateInternalContactsAsync(1234, "SFSO Lead", "new@email.com", ContactRole.SfsoLead);
            result.EmailUpdated.Should().Be(true);
            result.NameUpdated.Should().Be(false);
        }
    }
}

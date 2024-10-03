using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests.Contexts;

public class SetChangedByInterceptorTests(FiatDbContainerFixture fiatDbContainerFixture)
    : BaseFiatDbTest(fiatDbContainerFixture)
{
    [Theory]
    [InlineData("Test User", "user@test")]
    [InlineData("Another User", "another@test")]
    public async Task SetChangedByInterceptor_should_set_lastmodified_from_userdetailsprovider_on_SaveChangesAsync(
        string username, string email)
    {
        MockUserDetailsProvider.Setup(a => a.GetUserDetails()).Returns((username, email));

        var entry = FiatDbContext.Contacts.Add(new Contact
        {
            Name = "My TrustRelationshipManager",
            Email = "my.TrustRelationshipManager@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        }).Entity;

        await FiatDbContext.SaveChangesAsync();

        entry.LastModifiedByName.Should().Be(username);
        entry.LastModifiedByEmail.Should().Be(email);
    }

    [Theory]
    [InlineData("Test User", "user@test")]
    [InlineData("Another User", "another@test")]
    public void SetChangedByInterceptor_should_set_lastmodified_from_userdetailsprovider_on_SaveChanges(string username,
        string email)
    {
        MockUserDetailsProvider.Setup(a => a.GetUserDetails()).Returns((username, email));

        var entry = FiatDbContext.Contacts.Add(new Contact
        {
            Name = "My TrustRelationshipManager",
            Email = "my.TrustRelationshipManager@education.gov.uk",
            Uid = 1234,
            Role = ContactRole.TrustRelationshipManager
        }).Entity;

        FiatDbContext.SaveChanges();

        entry.LastModifiedByName.Should().Be(username);
        entry.LastModifiedByEmail.Should().Be(email);
    }
}

using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests;

[Collection(nameof(UseFiatDbContainer))]
public abstract class BaseFiatDbTest : IDisposable
{
    protected FiatDbContext FiatDbContext { get; }
    protected Mock<IUserDetailsProvider> MockUserDetailsProvider { get; } = new();

    protected BaseFiatDbTest(FiatDbContainerFixture fiatDbContainerFixture)
    {
        MockUserDetailsProvider.Setup(a => a.GetUserDetails()).Returns(("Default TestUser", "user@defaulttest"));

        FiatDbContext = new FiatDbContext(
            new DbContextOptionsBuilder<FiatDbContext>().UseSqlServer(fiatDbContainerFixture.ConnectionString).Options,
            new SetChangedByInterceptor(MockUserDetailsProvider.Object));

        FiatDbContext.Database.EnsureDeleted();
        FiatDbContext.Database.EnsureCreated();

        AddSeedData();
    }

    /// <summary>
    /// Add seed data to ensure that no tests passes because there was only one row in the db
    /// </summary>
    private void AddSeedData()
    {
        FiatDbContext.Contacts.AddRange([
            new Contact
            {
                Name = "Other TrustRelationshipManager",
                Email = "other.TrustRelationshipManager@education.gov.uk",
                Uid = 42,
                Role = ContactRole.TrustRelationshipManager
            },
            new Contact
            {
                Name = "Other SfsoLead",
                Email = "other.SfsoLead@education.gov.uk",
                Uid = 42,
                Role = ContactRole.SfsoLead
            }
        ]);

        FiatDbContext.SaveChanges();
    }

    public void Dispose()
    {
        FiatDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

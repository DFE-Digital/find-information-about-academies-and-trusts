using DfE.FindInformationAcademiesTrusts.Data.Enums;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests;

[Trait("MutationTest", "Ignore")]
[Collection(nameof(UseFiatDbContainer))]
public abstract class BaseFiatDbTest : IDisposable
{
    private bool _isDisposed;
    protected FiatDbContext FiatDbContext { get; }
    protected IUserDetailsProvider MockUserDetailsProvider { get; }

    protected BaseFiatDbTest(FiatDbContainerFixture fiatDbContainerFixture)
    {
        MockUserDetailsProvider = Substitute.For<IUserDetailsProvider>();
        MockUserDetailsProvider.GetUserDetails().Returns(("Default TestUser", "user@defaulttest"));

        FiatDbContext = new FiatDbContext(
            new DbContextOptionsBuilder<FiatDbContext>().UseSqlServer(fiatDbContainerFixture.ConnectionString).Options,
            new SetChangedByInterceptor(MockUserDetailsProvider));

        FiatDbContext.Database.EnsureDeleted();
        FiatDbContext.Database.EnsureCreated();

        AddSeedData();
    }

    /// <summary>
    /// Add seed data to ensure that no tests passes because there was only one row in the db
    /// </summary>
    private void AddSeedData()
    {
        FiatDbContext.SchoolContacts.AddRange(
            new SchoolContact
            {
                Name = "Other TrustRelationshipManager",
                Email = "other.TrustRelationshipManager@education.gov.uk",
                Urn = 424242,
                Role = SchoolContactRole.RegionsGroupLocalAuthorityLead
            }
        );

        FiatDbContext.TrustContacts.AddRange(
            new TrustContact
            {
                Name = "Other TrustRelationshipManager",
                Email = "other.TrustRelationshipManager@education.gov.uk",
                Uid = 42,
                Role = TrustContactRole.TrustRelationshipManager
            },
            new TrustContact
            {
                Name = "Other SfsoLead",
                Email = "other.SfsoLead@education.gov.uk",
                Uid = 42,
                Role = TrustContactRole.SfsoLead
            }
        );

        FiatDbContext.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed) return;

        if (disposing)
        {
            FiatDbContext.Dispose();
        }

        _isDisposed = true;
    }
}

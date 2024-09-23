using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.UnitTests;

[Collection(nameof(UseFiatDbContainer))]
public abstract class BaseFiatDbTest : IDisposable
{
    protected FiatDbContext FiatDbContext { get; }
    protected Mock<IUserDetailsProvider> MockUserDetailsProvider { get; } = new();

    protected BaseFiatDbTest(FiatDbContainerFixture fiatDbContainerFixture)
    {
        FiatDbContext = new FiatDbContext(
            new DbContextOptionsBuilder<FiatDbContext>().UseSqlServer(fiatDbContainerFixture.ConnectionString).Options,
            new SetChangedByInterceptor(MockUserDetailsProvider.Object));

        FiatDbContext.Database.EnsureDeleted();
        FiatDbContext.Database.EnsureCreated();

        MockUserDetailsProvider.Setup(a => a.GetUserDetails()).Returns(("Test User", "user@test"));
    }

    public void Dispose()
    {
        FiatDbContext.Dispose();
        GC.SuppressFinalize(this);
    }
}

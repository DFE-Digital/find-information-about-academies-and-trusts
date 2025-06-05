using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;

public sealed class FiatDbContext(
    DbContextOptions<FiatDbContext> options,
    SetChangedByInterceptor setChangedByInterceptor)
    : DbContext(options)
{
    public DbSet<TrustContact> TrustContacts { get; set; }

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(setChangedByInterceptor);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<Enum>()
            .HaveConversion<string>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureContactEntity<TrustContact>(modelBuilder, "Contacts",
            c => c.Uid,
            c => new { TrustUid = c.Uid, c.Role });
    }

    private static void ConfigureContactEntity<T>(ModelBuilder modelBuilder, string dbTableName,
        Expression<Func<T, object?>> organisationIndex, Expression<Func<T, object?>> organisationRoleIndex)
        where T : BaseEntity
    {
        var entity = modelBuilder.Entity<T>();

        entity.ToTable(dbTableName, table => table.IsTemporal());

        entity.HasIndex(organisationIndex);

        entity.HasIndex(organisationRoleIndex)
            .IsUnique();

        entity.Property(c => c.LastModifiedAtTime)
            .HasComputedColumnSql("[PeriodStart]");
    }
}

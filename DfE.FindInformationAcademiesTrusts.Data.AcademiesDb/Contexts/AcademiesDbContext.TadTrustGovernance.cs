using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<TadTrustGovernance> TadTrustGovernances { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingTadTrustGovernances(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TadTrustGovernance>(entity =>
        {
            entity.HasNoKey().ToTable("TrustGovernance", "tad");

            entity.Property(e => e.Email)
                .IsUnicode(false);
            entity.Property(e => e.Gid)
                .IsUnicode(false)
                .HasColumnName("GID");
        });
    }
}
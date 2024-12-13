using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;

namespace DfE.FIAT.Data.AcademiesDb.Contexts;

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
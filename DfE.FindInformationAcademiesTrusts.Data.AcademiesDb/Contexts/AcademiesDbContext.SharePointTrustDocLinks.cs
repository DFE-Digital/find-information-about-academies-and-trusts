using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Sharepoint;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<SharepointTrustDocLink> SharepointTrustDocLinks { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingSharePointTrustDocLinks(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SharepointTrustDocLink>(entity =>
        {
            entity.HasKey(e => new { e.FolderPrefix, e.FolderYear, e.DocumentFilename });

            entity.ToTable("TrustDocLinks", "sharepoint");

            entity.Property(e => e.FolderPrefix)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FolderYear)
                .HasConversion<string>()
                .HasColumnType("varchar(4)")
                .IsUnicode(false);
            entity.Property(e => e.DocumentFilename).HasMaxLength(100);
            entity.Property(e => e.CompaniesHouseNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ContentType).HasMaxLength(50);
            entity.Property(e => e.DataRefreshDateTime).HasColumnType("datetime");
            entity.Property(e => e.DocumentIdvalue)
                .HasMaxLength(100)
                .HasColumnName("DocumentIDValue");
            entity.Property(e => e.DocumentLink).HasMaxLength(300);
            entity.Property(e => e.DocumentPath).HasMaxLength(50);
            entity.Property(e => e.TrustRefNumber)
                .HasMaxLength(10)
                .IsUnicode(false);
        });
    }
}

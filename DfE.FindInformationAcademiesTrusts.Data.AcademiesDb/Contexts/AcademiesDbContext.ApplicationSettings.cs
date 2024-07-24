using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<ApplicationSetting> ApplicationSettings { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingApplicationSettings(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationSetting>(entity =>
        {
            entity.ToTable("ApplicationSettings", "ops");

            entity.HasKey(e => e.Key);
            entity.Property(e => e.Value)
                .IsUnicode(false);
            entity.Property(e => e.Created);
            entity.Property(e => e.CreatedBy)
                .IsUnicode(false)
                .HasColumnName("Created By")
                .HasMaxLength(100);
            entity.Property(e => e.Modified);
            entity.Property(e => e.ModifiedBy)
                .IsUnicode(false)
                .HasColumnName("Modified By")
                .HasMaxLength(100);
        });
    }
}

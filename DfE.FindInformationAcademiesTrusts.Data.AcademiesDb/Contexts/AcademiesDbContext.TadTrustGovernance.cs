using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
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
            entity.HasKey(e => e.Sk);

            entity.ToTable("TrustGovernance", "tad");

            entity.HasIndex(e => e.Gid, "IX_TrustGovernanceGID").IsUnique();

            entity.Property(e => e.Sk)
                .ValueGeneratedNever()
                .HasColumnName("SK");
            entity.Property(e => e.AppointingBody)
                .IsUnicode(false)
                .HasColumnName("Appointing body");
            entity.Property(e => e.DateOfAppointment)
                .IsUnicode(false)
                .HasColumnName("Date of appointment");
            entity.Property(e => e.DateTermOfOfficeEndsEnded)
                .IsUnicode(false)
                .HasColumnName("Date term of office ends/ended");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FkGovernanceRoleType).HasColumnName("FK_GovernanceRoleType");
            entity.Property(e => e.FkTrust).HasColumnName("FK_Trust");
            entity.Property(e => e.Forename1).IsUnicode(false);
            entity.Property(e => e.Forename2).IsUnicode(false);
            entity.Property(e => e.Gid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GID");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("Modified By");
            entity.Property(e => e.Surname).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
        });
    }
}
using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<TadHeadTeacherContact> TadHeadTeacherContacts { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingTadHeadTeacherContacts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TadHeadTeacherContact>(entity =>
        {
            entity.HasKey(e => e.Urn).HasName("PK__HeadTeac__DD778414D59087FB");

            entity.ToTable("HeadTeacherContacts_FIAT", "tad");

            entity.Property(e => e.Urn)
                .ValueGeneratedNever()
                .HasColumnName("urn");
            entity.Property(e => e.DateImported).HasColumnName("date_imported");
            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .HasColumnName("file_name");
            entity.Property(e => e.HeadEmail)
                .HasMaxLength(255)
                .HasColumnName("head_email");
            entity.Property(e => e.HeadFirstName)
                .HasMaxLength(255)
                .HasColumnName("head_first_name");
            entity.Property(e => e.HeadLastName)
                .HasMaxLength(255)
                .HasColumnName("head_last_name");
        });
    }
}

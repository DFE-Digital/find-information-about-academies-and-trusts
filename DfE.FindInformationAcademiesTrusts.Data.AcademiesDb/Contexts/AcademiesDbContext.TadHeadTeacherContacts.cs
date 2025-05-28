using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Tad;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<TadHeadTeacherContactsFiat> TadHeadTeacherContacts { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingTadHeadTeacherContacts(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TadHeadTeacherContactsFiat>(entity =>
        {
            entity.HasNoKey().ToTable("HeadTeacherContacts_FIAT", "tad");

            entity.Property(e => e.Urn)
                .IsRequired()
                .HasColumnName("urn");
            entity.Property(e => e.HeadFirstName)
                .HasColumnName("head_first_name");
            entity.Property(e => e.HeadLastName)
                .HasColumnName("head_last_name");
            entity.Property(e => e.HeadEmail)
                .HasColumnName("head_email");
        });
    }
}

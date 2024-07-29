using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<GiasGovernance> GiasGovernances { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingGiasGovernances(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasGovernance>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Governance", "gias");

            entity.Property(e => e.AppointingBody)
                .IsUnicode(false)
                .HasColumnName("Appointing body");
            entity.Property(e => e.CompaniesHouseNumber)
                .IsUnicode(false)
                .HasColumnName("Companies House Number");
            entity.Property(e => e.DateOfAppointment)
                .IsUnicode(false)
                .HasColumnName("Date of appointment");
            entity.Property(e => e.DateTermOfOfficeEndsEnded)
                .IsUnicode(false)
                .HasColumnName("Date term of office ends/ended");
            entity.Property(e => e.Forename1)
                .IsUnicode(false)
                .HasColumnName("Forename 1");
            entity.Property(e => e.Forename2)
                .IsUnicode(false)
                .HasColumnName("Forename 2");
            entity.Property(e => e.Gid)
                .IsUnicode(false)
                .HasColumnName("GID");
            entity.Property(e => e.Role).IsUnicode(false);
            entity.Property(e => e.Surname).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
            entity.Property(e => e.Uid)
                .IsUnicode(false)
                .HasColumnName("UID");
            entity.Property(e => e.Urn)
                .IsUnicode(false)
                .HasColumnName("URN");
        });
    }
}

using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<GiasGroup> Groups { get; set; }

    [ExcludeFromCodeCoverage]
    protected void OnModelCreatingGiasGroup(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Group", "gias");

            entity.Property(e => e.ClosedDate)
                .IsUnicode(false)
                .HasColumnName("Closed Date");
            entity.Property(e => e.CompaniesHouseNumber)
                .IsUnicode(false)
                .HasColumnName("Companies House Number");
            entity.Property(e => e.GroupContactLocality)
                .IsUnicode(false)
                .HasColumnName("Group Contact Locality");
            entity.Property(e => e.GroupContactPostcode)
                .IsUnicode(false)
                .HasColumnName("Group Contact Postcode");
            entity.Property(e => e.GroupContactStreet)
                .IsUnicode(false)
                .HasColumnName("Group Contact Street");
            entity.Property(e => e.GroupContactTown)
                .IsUnicode(false)
                .HasColumnName("Group Contact Town");
            entity.Property(e => e.GroupId)
                .IsUnicode(false)
                .HasColumnName("Group ID");
            entity.Property(e => e.GroupName)
                .IsUnicode(false)
                .HasColumnName("Group Name");
            entity.Property(e => e.GroupStatus)
                .IsUnicode(false)
                .HasColumnName("Group Status");
            entity.Property(e => e.GroupStatusCode)
                .IsUnicode(false)
                .HasColumnName("Group Status (code)");
            entity.Property(e => e.GroupType)
                .IsUnicode(false)
                .HasColumnName("Group Type");
            entity.Property(e => e.GroupTypeCode)
                .IsUnicode(false)
                .HasColumnName("Group Type (code)");
            entity.Property(e => e.GroupUid)
                .IsUnicode(false)
                .HasColumnName("Group UID");
            entity.Property(e => e.HeadOfGroupFirstName)
                .IsUnicode(false)
                .HasColumnName("Head of Group First Name");
            entity.Property(e => e.HeadOfGroupLastName)
                .IsUnicode(false)
                .HasColumnName("Head of Group Last Name");
            entity.Property(e => e.HeadOfGroupTitle)
                .IsUnicode(false)
                .HasColumnName("Head of Group Title");
            entity.Property(e => e.IncorporatedOnOpenDate)
                .IsUnicode(false)
                .HasColumnName("Incorporated on (open date)");
            entity.Property(e => e.OpenDate)
                .IsUnicode(false)
                .HasColumnName("Open date");
            entity.Property(e => e.Ukprn).HasColumnName("UKPRN");
        });
    }
}

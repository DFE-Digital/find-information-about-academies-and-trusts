using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<GiasGroupLink> GiasGroupLinks { get; set; }
    
    public static readonly Expression<Func<GiasGroupLink, bool>> GiasGroupLinkQueryFilter = gl => gl.Urn != null;
    
    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingGiasGroupLink(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasGroupLink>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("GroupLink", "gias");

            entity.Property(e => e.ClosedDate)
                .IsUnicode(false)
                .HasColumnName("Closed Date");
            entity.Property(e => e.CompaniesHouseNumber)
                .IsUnicode(false)
                .HasColumnName("Companies House Number");
            entity.Property(e => e.EstablishmentName).IsUnicode(false);
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
            entity.Property(e => e.IncorporatedOnOpenDate)
                .IsUnicode(false)
                .HasColumnName("Incorporated on (open date)");
            entity.Property(e => e.JoinedDate)
                .IsUnicode(false)
                .HasColumnName("Joined date");
            entity.Property(e => e.LaCode)
                .IsUnicode(false)
                .HasColumnName("LA (code)");
            entity.Property(e => e.LaName)
                .IsUnicode(false)
                .HasColumnName("LA (name)");
            entity.Property(e => e.OpenDate)
                .IsUnicode(false)
                .HasColumnName("Open date");
            entity.Property(e => e.PhaseOfEducationCode)
                .IsUnicode(false)
                .HasColumnName("PhaseOfEducation (code)");
            entity.Property(e => e.PhaseOfEducationName)
                .IsUnicode(false)
                .HasColumnName("PhaseOfEducation (name)");
            entity.Property(e => e.TypeOfEstablishmentCode)
                .IsUnicode(false)
                .HasColumnName("TypeOfEstablishment (code)");
            entity.Property(e => e.TypeOfEstablishmentName)
                .IsUnicode(false)
                .HasColumnName("TypeOfEstablishment (name)");
            entity.Property(e => e.Urn)
                .IsUnicode(false)
                .HasColumnName("URN");
            entity.Property(e => e.UrnGroupUid)
                .IsUnicode(false)
                .HasColumnName("URN_GroupUID");

            entity.HasQueryFilter(GiasGroupLinkQueryFilter);
        });
    }
}

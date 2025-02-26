using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<MstrFreeSchoolProject> MstrFreeSchoolProjects { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMstrFreeSchoolProjects(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstrFreeSchoolProject>(entity =>
        {
            entity
                .HasKey(e => e.SK);

            entity.ToTable("FreeSchoolProjects", "mstr");

            entity.Property(e => e.ProjectID)
                .HasColumnName("Project ID");

            entity.Property(e => e.ProjectName)
                .HasColumnName("Project Name");

            entity.Property(e => e.ProjectApplicationType)
                .HasColumnName("Project Application Type");

            entity.Property(e => e.LocalAuthority)
                .HasColumnName("Local Authority");

            entity.Property(e => e.RouteOfProject)
                .HasColumnName("Route of Project");

            entity.Property(e => e.StatutoryLowestAge)
                .HasColumnName("Statutory Lowest Age");

            entity.Property(e => e.StatutoryHighestAge)
                .HasColumnName("Statutory Highest Age");

            entity.Property(e => e.EstablishmentName)
                .HasColumnName("Establishment Name");

            entity.Property(e => e.ProvisionalOpeningDate)
                .HasColumnName("Provisional Opening Date");

            entity.Property(e => e.TrustID)
                .HasColumnName("Trust ID");

            entity.Property(e => e.NewURN)
                .HasColumnName("New URN");

            entity.Property(e => e.LastDataRefresh)
                .HasColumnName("Last Data Refresh");
        });
    }
}

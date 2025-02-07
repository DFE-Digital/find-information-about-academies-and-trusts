using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;
public partial class AcademiesDbContext
{
    public DbSet<MstrAcademyTransfers> MstrAcademyTransfers { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMstrAcademyTransfers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MstrAcademyTransfers>(entity =>
        {
            entity.HasKey(e => e.SK);

            entity.ToTable("AcademyTransfers", "mstr");

            entity.Property(e => e.AcademyName)
                .HasColumnName("Academy Name");

            entity.Property(e => e.AcademyURN)
                .HasColumnName("Academy URN");

            entity.Property(e => e.AcademyTransferStatus)
                .HasColumnName("Academy transfer Status");

            entity.Property(e => e.NewProvisionalTrustID)
                .HasColumnName("New provisional Trust ID");

            entity.Property(e => e.StatutoryLowestAge)
                .HasColumnName("Statutory Lowest Age");

            entity.Property(e => e.StatutoryHighestAge)
                .HasColumnName("Statutory Highest Age");

            entity.Property(e => e.LocalAuthority)
                .HasColumnName("Local Authority");

            entity.Property(e => e.ExpectedTransferDate)
                .HasColumnName("Expected Academy transfer date");

            entity.Property(e => e.InPrepare)
                .HasColumnName("In Prepare");

            entity.Property(e => e.InComplete)
                .HasColumnName("In Complete");

            entity.Property(e => e.LastDataRefresh)
                .HasColumnName("Last Data Refresh");

        });
    }
}
using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<MisFurtherEducationEstablishment> MisFurtherEducationEstablishments { get; set; }

    [ExcludeFromCodeCoverage]
    protected void OnModelCreatingMisFurtherEducationEstablishments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MisFurtherEducationEstablishment>(entity =>
        {
            entity.HasKey(e => e.ProviderUrn).HasName("PK_ManagementInformationFurtherEducationSchoolTableData");

            entity.ToTable("FurtherEducationEstablishments", "mis");

            entity.Property(e => e.ProviderUrn)
                .ValueGeneratedNever()
                .HasColumnName("Provider URN");
            entity.Property(e => e.BehaviourAndAttitudes).HasColumnName("Behaviour and attitudes");
            entity.Property(e => e.BehaviourAndAttitudesRaw).HasColumnName("Behaviour and attitudes RAW");
            entity.Property(e => e.DateOfLatestShortInspection).HasColumnName("Date of latest short inspection");
            entity.Property(e => e.DatePublished).HasColumnName("Date published");
            entity.Property(e => e.EffectivenessOfLeadershipAndManagement)
                .HasColumnName("Effectiveness of leadership and management");
            entity.Property(e => e.EffectivenessOfLeadershipAndManagementRaw)
                .HasColumnName("Effectiveness of leadership and management RAW");
            entity.Property(e => e.FirstDayOfInspection).HasColumnName("First day of inspection");
            entity.Property(e => e.ImprovedDeclinedStayedTheSame).HasColumnName("Improved/ declined/ stayed the same");
            entity.Property(e => e.InspectionNumber).HasColumnName("Inspection number");
            entity.Property(e => e.InspectionType).HasColumnName("Inspection type");
            entity.Property(e => e.IsSafeguardingEffective).HasColumnName("Is safeguarding effective?");
            entity.Property(e => e.LastDayOfInspection).HasColumnName("Last day of Inspection");
            entity.Property(e => e.LocalAuthority).HasColumnName("Local authority");
            entity.Property(e => e.NumberOfShortInspectionsSinceLastFullInspection)
                .HasColumnName("Number of short inspections since last full inspection");
            entity.Property(e => e.NumberOfShortInspectionsSinceLastFullInspectionRaw)
                .HasColumnName("Number of short inspections since last full inspection RAW");
            entity.Property(e => e.OfstedRegion).HasColumnName("Ofsted region");
            entity.Property(e => e.OverallEffectiveness).HasColumnName("Overall effectiveness");
            entity.Property(e => e.OverallEffectivenessRaw).HasColumnName("Overall effectiveness RAW");
            entity.Property(e => e.PersonalDevelopment).HasColumnName("Personal development");
            entity.Property(e => e.PersonalDevelopmentRaw).HasColumnName("Personal development RAW");
            entity.Property(e => e.PreviousBehaviourAndAttitudes).HasColumnName("Previous behaviour and attitudes");
            entity.Property(e => e.PreviousBehaviourAndAttitudesRaw)
                .HasColumnName("Previous behaviour and attitudes RAW");
            entity.Property(e => e.PreviousEffectivenessOfLeadershipAndManagement)
                .HasColumnName("Previous effectiveness of leadership and management");
            entity.Property(e => e.PreviousEffectivenessOfLeadershipAndManagementRaw)
                .HasColumnName("Previous effectiveness of leadership and management RAW");
            entity.Property(e => e.PreviousInspectionNumber).HasColumnName("Previous inspection number");
            entity.Property(e => e.PreviousLastDayOfInspection).HasColumnName("Previous last day of inspection");
            entity.Property(e => e.PreviousOverallEffectiveness).HasColumnName("Previous overall effectiveness");
            entity.Property(e => e.PreviousOverallEffectivenessRaw).HasColumnName("Previous overall effectiveness RAW");
            entity.Property(e => e.PreviousPersonalDevelopment).HasColumnName("Previous personal development");
            entity.Property(e => e.PreviousPersonalDevelopmentRaw).HasColumnName("Previous personal development RAW");
            entity.Property(e => e.PreviousQualityOfEducation).HasColumnName("Previous quality of education");
            entity.Property(e => e.PreviousQualityOfEducationRaw).HasColumnName("Previous quality of education RAW");
            entity.Property(e => e.PreviousSafeguarding).HasColumnName("Previous safeguarding");
            entity.Property(e => e.ProviderGroup).HasColumnName("Provider group");
            entity.Property(e => e.ProviderName).HasColumnName("Provider name");
            entity.Property(e => e.ProviderType).HasColumnName("Provider type");
            entity.Property(e => e.ProviderUkprn).HasColumnName("Provider UKPRN");
            entity.Property(e => e.QualityOfEducation).HasColumnName("Quality of education");
            entity.Property(e => e.QualityOfEducationRaw).HasColumnName("Quality of education RAW");
        });
    }
}

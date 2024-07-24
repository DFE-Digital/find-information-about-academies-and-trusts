using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<MisEstablishment> MisEstablishments { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMisEstablishment(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MisEstablishment>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Establishments", "mis");

            entity.HasIndex(e => e.Urn, "Idx_misEstablishments_URN");

            entity.Property(e => e.AdmissionsPolicy).HasColumnName("Admissions policy");
            entity.Property(e => e.BehaviourAndAttitudes).HasColumnName("Behaviour and attitudes");
            entity.Property(e => e.CategoryOfConcern).HasColumnName("Category of concern");
            entity.Property(e => e.DateOfLatestSection8Inspection).HasColumnName("Date of latest section 8 inspection");
            entity.Property(e => e.DesignatedReligiousCharacter).HasColumnName("Designated religious character");
            entity.Property(e => e.DidTheLatestSection8InspectionConvertToAFullInspection)
                .HasColumnName("Did the latest section 8 inspection convert to a full inspection?");
            entity.Property(e => e.DoesTheLatestFullInspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("Does the latest full inspection relate to the URN of the current school?");
            entity.Property(e => e.DoesThePreviousFullInspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("Does the previous full inspection relate to the URN of the current school?");
            entity.Property(e => e.DoesTheSection8InspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("Does the section 8 inspection relate to the URN of the current school?");
            entity.Property(e => e.EarlyYearsProvisionWhereApplicable)
                .HasColumnName("Early years provision (where applicable)");
            entity.Property(e => e.EffectivenessOfLeadershipAndManagement)
                .HasColumnName("Effectiveness of leadership and management");
            entity.Property(e => e.EventTypeGrouping).HasColumnName("Event type grouping");
            entity.Property(e => e.FaithGrouping).HasColumnName("Faith grouping");
            entity.Property(e => e.InspectionEndDate).HasColumnName("Inspection end date");
            entity.Property(e => e.InspectionNumberOfLatestFullInspection)
                .HasColumnName("Inspection number of latest full inspection");
            entity.Property(e => e.InspectionStartDate).HasColumnName("Inspection start date");
            entity.Property(e => e.InspectionType).HasColumnName("Inspection type");
            entity.Property(e => e.InspectionTypeGrouping).HasColumnName("Inspection type grouping");
            entity.Property(e => e.Laestab).HasColumnName("LAESTAB");
            entity.Property(e => e.LaestabAtTimeOfLatestFullInspection)
                .HasColumnName("LAESTAB at time of latest full inspection");
            entity.Property(e => e.LaestabAtTimeOfPreviousFullInspection)
                .HasColumnName("LAESTAB at time of previous full inspection");
            entity.Property(e => e.LaestabAtTimeOfTheSection8Inspection)
                .HasColumnName("LAESTAB at time of the section 8 inspection");
            entity.Property(e => e.LatestSection8InspectionNumberSinceLastFullInspection)
                .HasColumnName("Latest section 8 inspection number since last full inspection");
            entity.Property(e => e.LocalAuthority).HasColumnName("Local authority");
            entity.Property(e => e.NumberOfOtherSection8InspectionsSinceLastFullInspection)
                .HasColumnName("Number of other section 8 inspections since last full inspection");
            entity.Property(e => e.NumberOfSection8InspectionsSinceTheLastFullInspection)
                .HasColumnName("Number of section 8 inspections since the last full inspection");
            entity.Property(e => e.OfstedPhase).HasColumnName("Ofsted phase");
            entity.Property(e => e.OfstedRegion).HasColumnName("Ofsted region");
            entity.Property(e => e.OverallEffectiveness).HasColumnName("Overall effectiveness");
            entity.Property(e => e.ParliamentaryConstituency).HasColumnName("Parliamentary constituency");
            entity.Property(e => e.PersonalDevelopment).HasColumnName("Personal development");
            entity.Property(e => e.PreviousBehaviourAndAttitudes).HasColumnName("Previous behaviour and attitudes");
            entity.Property(e => e.PreviousCategoryOfConcern).HasColumnName("Previous category of concern");
            entity.Property(e => e.PreviousEarlyYearsProvisionWhereApplicable)
                .HasColumnName("Previous early years provision (where applicable)");
            entity.Property(e => e.PreviousEffectivenessOfLeadershipAndManagement)
                .HasColumnName("Previous effectiveness of leadership and management");
            entity.Property(e => e.PreviousFullInspectionNumber).HasColumnName("Previous full inspection number");
            entity.Property(e => e.PreviousFullInspectionOverallEffectiveness)
                .HasColumnName("Previous full inspection overall effectiveness");
            entity.Property(e => e.PreviousInspectionEndDate).HasColumnName("Previous inspection end date");
            entity.Property(e => e.PreviousInspectionStartDate).HasColumnName("Previous inspection start date");
            entity.Property(e => e.PreviousPersonalDevelopment).HasColumnName("Previous personal development");
            entity.Property(e => e.PreviousPublicationDate).HasColumnName("Previous publication date");
            entity.Property(e => e.PreviousQualityOfEducation).HasColumnName("Previous quality of education");
            entity.Property(e => e.PreviousSafeguardingIsEffective)
                .HasColumnName("Previous safeguarding is effective?");
            entity.Property(e => e.PreviousSixthFormProvisionWhereApplicable)
                .HasColumnName("Previous sixth form provision (where applicable)");
            entity.Property(e => e.PublicationDate).HasColumnName("Publication date");
            entity.Property(e => e.QualityOfEducation).HasColumnName("Quality of education");
            entity.Property(e => e.ReligiousEthos).HasColumnName("Religious ethos");
            entity.Property(e => e.SafeguardingIsEffective).HasColumnName("Safeguarding is effective?");
            entity.Property(e => e.SchoolName).HasColumnName("School name");
            entity.Property(e => e.SchoolNameAtTimeOfLatestFullInspection)
                .HasColumnName("School name at time of latest full inspection");
            entity.Property(e => e.SchoolNameAtTimeOfPreviousFullInspection)
                .HasColumnName("School name at time of previous full inspection");
            entity.Property(e => e.SchoolNameAtTimeOfTheLatestSection8Inspection)
                .HasColumnName("School name at time of the latest section 8 inspection");
            entity.Property(e => e.SchoolOpenDate).HasColumnName("School open date");
            entity.Property(e => e.SchoolTypeAtTimeOfLatestFullInspection)
                .HasColumnName("School type at time of latest full inspection");
            entity.Property(e => e.SchoolTypeAtTimeOfPreviousFullInspection)
                .HasColumnName("School type at time of previous full inspection");
            entity.Property(e => e.SchoolTypeAtTimeOfTheLatestSection8Inspection)
                .HasColumnName("School type at time of the latest section 8 inspection");
            entity.Property(e => e.Section8InspectionOverallOutcome)
                .HasColumnName("Section 8 inspection overall outcome");
            entity.Property(e => e.Section8InspectionPublicationDate)
                .HasColumnName("Section 8 inspection publication date");
            entity.Property(e => e.SixthForm).HasColumnName("Sixth form");
            entity.Property(e => e.SixthFormProvisionWhereApplicable)
                .HasColumnName("Sixth form provision (where applicable)");
            entity.Property(e => e.TheIncomeDeprivationAffectingChildrenIndexIdaciQuintile)
                .HasColumnName("The income deprivation affecting children index (IDACI) quintile");
            entity.Property(e => e.TotalNumberOfPupils).HasColumnName("Total number of pupils");
            entity.Property(e => e.TypeOfEducation).HasColumnName("Type of education");
            entity.Property(e => e.Urn).HasColumnName("URN");
            entity.Property(e => e.UrnAtTimeOfLatestFullInspection)
                .HasColumnName("URN at time of latest full inspection");
            entity.Property(e => e.UrnAtTimeOfPreviousFullInspection)
                .HasColumnName("URN at time of previous full inspection");
            entity.Property(e => e.UrnAtTimeOfTheSection8Inspection)
                .HasColumnName("URN at time of the section 8 inspection");
            entity.Property(e => e.WebLink).HasColumnName("Web link");
        });
    }
}

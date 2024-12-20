using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis_Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<EstablishmentFiat> EstablishmentsFiat { get; set; }

    public DbSet<FurtherEducationEstablishmentFiat> FurtherEducationEstablishmentsFiat { get; set; }
    
    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingMis_Mstr(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstablishmentFiat>(entity =>
        {
            entity.ToTable("Establishments_FIAT", "mis_mstr");

            entity.HasKey(e => e.Urn);

            entity.Property(e => e.WebLink)
                .HasColumnName("web_link");
            entity.Property(e => e.Urn)
                .HasColumnName("urn")
                .IsRequired();
            entity.Property(e => e.Laestab)
                .HasColumnName("laestab");
            entity.Property(e => e.SchoolName)
                .HasColumnName("school_name");
            entity.Property(e => e.OfstedPhase)
                .HasColumnName("ofsted_phase");
            entity.Property(e => e.TypeOfEducation)
                .HasColumnName("type_of_education");
            entity.Property(e => e.SchoolOpenDate)
                .HasColumnName("school_open_date");
            entity.Property(e => e.AdmissionsPolicy)
                .HasColumnName("admissions_policy");
            entity.Property(e => e.SixthForm)
                .HasColumnName("sixth_form");
            entity.Property(e => e.DesignatedReligiousCharacter)
                .HasColumnName("designated_religious_character");
            entity.Property(e => e.ReligiousEthos)
                .HasColumnName("religious_ethos");
            entity.Property(e => e.FaithGrouping)
                .HasColumnName("faith_grouping");
            entity.Property(e => e.OfstedRegion)
                .HasColumnName("ofsted_region");
            entity.Property(e => e.Region)
                .HasColumnName("region");
            entity.Property(e => e.LocalAuthority)
                .HasColumnName("local_authority");
            entity.Property(e => e.ParliamentaryConstituency)
                .HasColumnName("parliamentary_constituency");
            entity.Property(e => e.Postcode)
                .HasColumnName("postcode");
            entity.Property(e => e.IncomeDeprivationAffectingChildrenIndexIdaciQuintile)
                .HasColumnName("the_income_deprivation_affecting_children_index_idaci_quintile");
            entity.Property(e => e.TotalNumberOfPupils)
                .HasColumnName("total_number_of_pupils");
            entity.Property(e => e.StatutoryLowestAge)
                .HasColumnName("statutory_lowest_age");
            entity.Property(e => e.StatutoryHighestAge)
                .HasColumnName("statutory_highest_age");
            entity.Property(e => e.LatestSection8InspectionNumberSinceLastFullInspection)
                .HasColumnName("latest_section_8_inspection_number_since_last_full_inspection");
            entity.Property(e => e.DoesTheSection8InspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("does_the_section_8_inspection_relate_to_the_urn_of_the_current_school");
            entity.Property(e => e.UrnAtTimeOfSection8Inspection)
                .HasColumnName("urn_at_time_of_the_section_8_inspection");
            entity.Property(e => e.LaestabAtTimeOfSection8Inspection)
                .HasColumnName("laestab_at_time_of_the_section_8_inspection");
            entity.Property(e => e.SchoolNameAtTimeOfLatestSection8Inspection)
                .HasColumnName("school_name_at_time_of_the_latest_section_8_inspection");
            entity.Property(e => e.SchoolTypeAtTimeOfLatestSection8Inspection)
                .HasColumnName("school_type_at_time_of_the_latest_section_8_inspection");
            entity.Property(e => e.NumberOfSection8InspectionsSinceLastFullInspection)
                .HasColumnName("number_of_section_8_inspections_since_the_last_full_inspection");
            entity.Property(e => e.DateOfLatestSection8Inspection)
                .HasColumnName("date_of_latest_section_8_inspection");
            entity.Property(e => e.Section8InspectionPublicationDate)
                .HasColumnName("section_8_inspection_publication_date");
            entity.Property(e => e.DidTheLatestSection8InspectionConvertToAFullInspection)
                .HasColumnName("did_the_latest_section_8_inspection_convert_to_a_full_inspection");
            entity.Property(e => e.Section8InspectionOverallOutcome)
                .HasColumnName("section_8_inspection_overall_outcome");
            entity.Property(e => e.NumberOfOtherSection8InspectionsSinceLastFullInspection)
                .HasColumnName("number_of_other_section_8_inspections_since_last_full_inspection");
            entity.Property(e => e.InspectionNumberOfLatestFullInspection)
                .HasColumnName("inspection_number_of_latest_full_inspection");
            entity.Property(e => e.InspectionType)
                .HasColumnName("inspection_type");
            entity.Property(e => e.InspectionTypeGrouping)
                .HasColumnName("inspection_type_grouping");
            entity.Property(e => e.EventTypeGrouping)
                .HasColumnName("event_type_grouping");
            entity.Property(e => e.InspectionStartDate)
                .HasColumnName("inspection_start_date");
            entity.Property(e => e.PublicationDate)
                .HasColumnName("publication_date");
            entity.Property(e => e.DoesTheLatestFullInspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("does_the_latest_full_inspection_relate_to_the_urn_of_the_current_school");
            entity.Property(e => e.UrnAtTimeOfLatestFullInspection)
                .HasColumnName("urn_at_time_of_latest_full_inspection");
            entity.Property(e => e.LaestabAtTimeOfLatestFullInspection)
                .HasColumnName("laestab_at_time_of_latest_full_inspection");
            entity.Property(e => e.SchoolNameAtTimeOfLatestFullInspection)
                .HasColumnName("school_name_at_time_of_latest_full_inspection");
            entity.Property(e => e.SchoolTypeAtTimeOfLatestFullInspection)
                .HasColumnName("school_type_at_time_of_latest_full_inspection");
            entity.Property(e => e.OverallEffectiveness)
                .HasColumnName("overall_effectiveness");
            entity.Property(e => e.CategoryOfConcern)
                .HasColumnName("category_of_concern");
            entity.Property(e => e.QualityOfEducation)
                .HasColumnName("quality_of_education");
            entity.Property(e => e.BehaviourAndAttitudes)
                .HasColumnName("behaviour_and_attitudes");
            entity.Property(e => e.PersonalDevelopment)
                .HasColumnName("personal_development");
            entity.Property(e => e.EffectivenessOfLeadershipAndManagement)
                .HasColumnName("effectiveness_of_leadership_and_management");
            entity.Property(e => e.SafeguardingIsEffective)
                .HasColumnName("safeguarding_is_effective");
            entity.Property(e => e.EarlyYearsProvisionWhereApplicable)
                .HasColumnName("early_years_provision_where_applicable");
            entity.Property(e => e.SixthFormProvisionWhereApplicable)
                .HasColumnName("sixth_form_provision_where_applicable");
            entity.Property(e => e.PreviousFullInspectionNumber)
                .HasColumnName("previous_full_inspection_number");
            entity.Property(e => e.PreviousInspectionStartDate)
                .HasColumnName("previous_inspection_start_date");
            entity.Property(e => e.PreviousPublicationDate)
                .HasColumnName("previous_publication_date");
            entity.Property(e => e.DoesThePreviousFullInspectionRelateToTheUrnOfTheCurrentSchool)
                .HasColumnName("does_the_previous_full_inspection_relate_to_the_urn_of_the_current_school");
            entity.Property(e => e.UrnAtTimeOfPreviousFullInspection)
                .HasColumnName("urn_at_time_of_previous_full_inspection");
            entity.Property(e => e.LaestabAtTimeOfPreviousFullInspection)
                .HasColumnName("laestab_at_time_of_previous_full_inspection");
            entity.Property(e => e.SchoolNameAtTimeOfPreviousFullInspection)
                .HasColumnName("school_name_at_time_of_previous_full_inspection");
            entity.Property(e => e.SchoolTypeAtTimeOfPreviousFullInspection)
                .HasColumnName("school_type_at_time_of_previous_full_inspection");
            entity.Property(e => e.PreviousFullInspectionOverallEffectiveness)
                .HasColumnName("previous_full_inspection_overall_effectiveness");
            entity.Property(e => e.PreviousCategoryOfConcern)
                .HasColumnName("previous_category_of_concern");
            entity.Property(e => e.PreviousQualityOfEducation)
                .HasColumnName("previous_quality_of_education");
            entity.Property(e => e.PreviousBehaviourAndAttitudes)
                .HasColumnName("previous_behaviour_and_attitudes");
            entity.Property(e => e.PreviousPersonalDevelopment)
                .HasColumnName("previous_personal_development");
            entity.Property(e => e.PreviousEffectivenessOfLeadershipAndManagement)
                .HasColumnName("previous_effectiveness_of_leadership_and_management");
            entity.Property(e => e.PreviousSafeguardingIsEffective)
                .HasColumnName("previous_safeguarding_is_effective");
            entity.Property(e => e.PreviousEarlyYearsProvisionWhereApplicable)
                .HasColumnName("previous_early_years_provision_where_applicable");
            entity.Property(e => e.PreviousSixthFormProvisionWhereApplicable)
                .HasColumnName("previous_sixth_form_provision_where_applicable");
            entity.Property(e => e.MetaIngestionDateTime)
                .HasColumnName("meta_ingestion_datetime");
            entity.Property(e => e.MetaSourceFileName)
                .HasColumnName("meta_source_filename");
        });

        modelBuilder.Entity<FurtherEducationEstablishmentFiat>(entity =>
        {
            entity.ToTable("FurtherEducationEstablishments_FIAT", "mis_mstr");

            entity.HasKey(e => e.ProviderUrn);

            entity.Property(e => e.ProviderUrn)
                .HasColumnName("provider_urn")
                .IsRequired();
            entity.Property(e => e.ProviderUkprn)
                .HasColumnName("provider_ukprn");
            entity.Property(e => e.ProviderName)
                .HasColumnName("provider_name");
            entity.Property(e => e.ProviderType)
                .HasColumnName("provider_type");
            entity.Property(e => e.ProviderGroup)
                .HasColumnName("provider_group");
            entity.Property(e => e.LocalAuthority)
                .HasColumnName("local_authority");
            entity.Property(e => e.Region)
                .HasColumnName("region");
            entity.Property(e => e.OfstedRegion)
                .HasColumnName("ofsted_region");
            entity.Property(e => e.DateOfLatestShortInspection)
                .HasColumnName("date_of_latest_short_inspection");
            entity.Property(e => e.NumberOfShortInspectionsSinceLastFullInspection)
                .HasColumnName("number_of_short_inspections_since_last_full_inspection");
            entity.Property(e => e.InspectionNumber)
                .HasColumnName("inspection_number");
            entity.Property(e => e.InspectionType)
                .HasColumnName("inspection_type");
            entity.Property(e => e.FirstDayOfInspection)
                .HasColumnName("first_day_of_inspection");
            entity.Property(e => e.LastDayOfInspection)
                .HasColumnName("last_day_of_inspection");
            entity.Property(e => e.DatePublished)
                .HasColumnName("date_published");
            entity.Property(e => e.OverallEffectiveness)
                .HasColumnName("overall_effectiveness");
            entity.Property(e => e.QualityOfEducation)
                .HasColumnName("quality_of_education");
            entity.Property(e => e.BehaviourAndAttitudes)
                .HasColumnName("behaviour_and_attitudes");
            entity.Property(e => e.PersonalDevelopment)
                .HasColumnName("personal_development");
            entity.Property(e => e.EffectivenessOfLeadershipAndManagement)
                .HasColumnName("effectiveness_of_leadership_and_management");
            entity.Property(e => e.IsSafeguardingEffective)
                .HasColumnName("is_safeguarding_effective");
            entity.Property(e => e.PreviousInspectionNumber)
                .HasColumnName("previous_inspection_number");
            entity.Property(e => e.PreviousLastDayOfInspection)
                .HasColumnName("previous_last_day_of_inspection");
            entity.Property(e => e.PreviousOverallEffectiveness)
                .HasColumnName("previous_overall_effectiveness");
            entity.Property(e => e.PreviousQualityOfEducation)
                .HasColumnName("previous_quality_of_education");
            entity.Property(e => e.PreviousBehaviourAndAttitudes)
                .HasColumnName("previous_behaviour_and_attitudes");
            entity.Property(e => e.PreviousPersonalDevelopment)
                .HasColumnName("previous_personal_development");
            entity.Property(e => e.PreviousEffectivenessOfLeadershipAndManagement)
                .HasColumnName("previous_effectiveness_of_leadership_and_management");
            entity.Property(e => e.PreviousSafeguarding)
                .HasColumnName("previous_safeguarding");
            entity.Property(e => e.ImprovedDeclinedStayedTheSame)
                .HasColumnName("improved_declined_stayed_the_same");
            entity.Property(e => e.MetaIngestionDatetime)
                .HasColumnName("meta_ingestion_datetime");
            entity.Property(e => e.MetaSourceFilename)
                .HasColumnName("meta_source_filename");
        });
    }
}

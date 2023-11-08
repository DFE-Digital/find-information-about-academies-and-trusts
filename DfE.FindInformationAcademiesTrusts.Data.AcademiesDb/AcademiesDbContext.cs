using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IAcademiesDbContext
{
    DbSet<Establishment> Establishments { get; }
    DbSet<Governance> Governances { get; }
    DbSet<GroupLink> GroupLinks { get; }
    DbSet<Group> Groups { get; }
    DbSet<MstrTrust> MstrTrusts { get; set; }
}

[ExcludeFromCodeCoverage]
public class AcademiesDbContext : DbContext, IAcademiesDbContext
{
    public AcademiesDbContext()
    {
    }

    public AcademiesDbContext(DbContextOptions<AcademiesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Establishment> Establishments { get; set; }
    public DbSet<Governance> Governances { get; set; }
    public DbSet<GroupLink> GroupLinks { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<MstrTrust> MstrTrusts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Establishment>(entity =>
        {
            entity.HasKey(e => e.Urn);

            entity.ToTable("Establishment", "gias");

            entity.Property(e => e.Urn)
                .ValueGeneratedNever()
                .HasColumnName("URN");
            entity.Property(e => e.AccreditationExpiryDate).IsUnicode(false);
            entity.Property(e => e.Address3).IsUnicode(false);
            entity.Property(e => e.AdministrativeWardCode)
                .IsUnicode(false)
                .HasColumnName("AdministrativeWard (code)");
            entity.Property(e => e.AdministrativeWardName)
                .IsUnicode(false)
                .HasColumnName("AdministrativeWard (name)");
            entity.Property(e => e.AdmissionsPolicyCode)
                .IsUnicode(false)
                .HasColumnName("AdmissionsPolicy (code)");
            entity.Property(e => e.AdmissionsPolicyName)
                .IsUnicode(false)
                .HasColumnName("AdmissionsPolicy (name)");
            entity.Property(e => e.BoardersCode)
                .IsUnicode(false)
                .HasColumnName("Boarders (code)");
            entity.Property(e => e.BoardersName)
                .IsUnicode(false)
                .HasColumnName("Boarders (name)");
            entity.Property(e => e.BoardingEstablishmentName)
                .IsUnicode(false)
                .HasColumnName("BoardingEstablishment (name)");
            entity.Property(e => e.BsoinspectorateNameName)
                .IsUnicode(false)
                .HasColumnName("BSOInspectorateName (name)");
            entity.Property(e => e.CcfName)
                .IsUnicode(false)
                .HasColumnName("CCF (name)");
            entity.Property(e => e.CensusDate).IsUnicode(false);
            entity.Property(e => e.Chnumber)
                .IsUnicode(false)
                .HasColumnName("CHNumber");
            entity.Property(e => e.CloseDate).IsUnicode(false);
            entity.Property(e => e.CountryName)
                .IsUnicode(false)
                .HasColumnName("Country (name)");
            entity.Property(e => e.CountyName)
                .IsUnicode(false)
                .HasColumnName("County (name)");
            entity.Property(e => e.DateOfLastInspectionVisit).IsUnicode(false);
            entity.Property(e => e.DioceseCode)
                .IsUnicode(false)
                .HasColumnName("Diocese (code)");
            entity.Property(e => e.DioceseName)
                .IsUnicode(false)
                .HasColumnName("Diocese (name)");
            entity.Property(e => e.DistrictAdministrativeCode)
                .IsUnicode(false)
                .HasColumnName("DistrictAdministrative (code)");
            entity.Property(e => e.DistrictAdministrativeName)
                .IsUnicode(false)
                .HasColumnName("DistrictAdministrative (name)");
            entity.Property(e => e.Easting).IsUnicode(false);
            entity.Property(e => e.EbdName)
                .IsUnicode(false)
                .HasColumnName("EBD (name)");
            entity.Property(e => e.EdByOtherName)
                .IsUnicode(false)
                .HasColumnName("EdByOther (name)");
            entity.Property(e => e.EstablishmentAccreditedCode)
                .IsUnicode(false)
                .HasColumnName("EstablishmentAccredited (code)");
            entity.Property(e => e.EstablishmentAccreditedName)
                .IsUnicode(false)
                .HasColumnName("EstablishmentAccredited (name)");
            entity.Property(e => e.EstablishmentName).IsUnicode(false);
            entity.Property(e => e.EstablishmentNumber).IsUnicode(false);
            entity.Property(e => e.EstablishmentStatusCode)
                .IsUnicode(false)
                .HasColumnName("EstablishmentStatus (code)");
            entity.Property(e => e.EstablishmentStatusName)
                .IsUnicode(false)
                .HasColumnName("EstablishmentStatus (name)");
            entity.Property(e => e.EstablishmentTypeGroupCode)
                .IsUnicode(false)
                .HasColumnName("EstablishmentTypeGroup (code)");
            entity.Property(e => e.EstablishmentTypeGroupName)
                .IsUnicode(false)
                .HasColumnName("EstablishmentTypeGroup (name)");
            entity.Property(e => e.FederationFlagName)
                .IsUnicode(false)
                .HasColumnName("FederationFlag (name)");
            entity.Property(e => e.FederationsCode)
                .IsUnicode(false)
                .HasColumnName("Federations (code)");
            entity.Property(e => e.FederationsName)
                .IsUnicode(false)
                .HasColumnName("Federations (name)");
            entity.Property(e => e.Feheidentifier)
                .IsUnicode(false)
                .HasColumnName("FEHEIdentifier");
            entity.Property(e => e.Fsm)
                .IsUnicode(false)
                .HasColumnName("FSM");
            entity.Property(e => e.FtprovName)
                .IsUnicode(false)
                .HasColumnName("FTProv (name)");
            entity.Property(e => e.FurtherEducationTypeName)
                .IsUnicode(false)
                .HasColumnName("FurtherEducationType (name)");
            entity.Property(e => e.GenderCode)
                .IsUnicode(false)
                .HasColumnName("Gender (code)");
            entity.Property(e => e.GenderName)
                .IsUnicode(false)
                .HasColumnName("Gender (name)");
            entity.Property(e => e.GorCode)
                .IsUnicode(false)
                .HasColumnName("GOR (code)");
            entity.Property(e => e.GorName)
                .IsUnicode(false)
                .HasColumnName("GOR (name)");
            entity.Property(e => e.GsslacodeName)
                .IsUnicode(false)
                .HasColumnName("GSSLACode (name)");
            entity.Property(e => e.HeadFirstName).IsUnicode(false);
            entity.Property(e => e.HeadLastName).IsUnicode(false);
            entity.Property(e => e.HeadPreferredJobTitle).IsUnicode(false);
            entity.Property(e => e.HeadTitleName)
                .IsUnicode(false)
                .HasColumnName("HeadTitle (name)");
            entity.Property(e => e.InspectorateNameName)
                .IsUnicode(false)
                .HasColumnName("InspectorateName (name)");
            entity.Property(e => e.InspectorateReport).IsUnicode(false);
            entity.Property(e => e.LaCode)
                .IsUnicode(false)
                .HasColumnName("LA (code)");
            entity.Property(e => e.LaName)
                .IsUnicode(false)
                .HasColumnName("LA (name)");
            entity.Property(e => e.LastChangedDate).IsUnicode(false);
            entity.Property(e => e.Locality).IsUnicode(false);
            entity.Property(e => e.LsoaCode)
                .IsUnicode(false)
                .HasColumnName("LSOA (code)");
            entity.Property(e => e.LsoaName)
                .IsUnicode(false)
                .HasColumnName("LSOA (name)");
            entity.Property(e => e.MsoaCode)
                .IsUnicode(false)
                .HasColumnName("MSOA (code)");
            entity.Property(e => e.MsoaName)
                .IsUnicode(false)
                .HasColumnName("MSOA (name)");
            entity.Property(e => e.NextInspectionVisit).IsUnicode(false);
            entity.Property(e => e.Northing).IsUnicode(false);
            entity.Property(e => e.NumberOfBoys).IsUnicode(false);
            entity.Property(e => e.NumberOfGirls).IsUnicode(false);
            entity.Property(e => e.NumberOfPupils).IsUnicode(false);
            entity.Property(e => e.NurseryProvisionName)
                .IsUnicode(false)
                .HasColumnName("NurseryProvision (name)");
            entity.Property(e => e.OfficialSixthFormCode)
                .IsUnicode(false)
                .HasColumnName("OfficialSixthForm (code)");
            entity.Property(e => e.OfficialSixthFormName)
                .IsUnicode(false)
                .HasColumnName("OfficialSixthForm (name)");
            entity.Property(e => e.OfstedLastInsp).IsUnicode(false);
            entity.Property(e => e.OfstedRatingName)
                .IsUnicode(false)
                .HasColumnName("OfstedRating (name)");
            entity.Property(e => e.OfstedSpecialMeasuresCode)
                .IsUnicode(false)
                .HasColumnName("OfstedSpecialMeasures (code)");
            entity.Property(e => e.OfstedSpecialMeasuresName)
                .IsUnicode(false)
                .HasColumnName("OfstedSpecialMeasures (name)");
            entity.Property(e => e.OpenDate).IsUnicode(false);
            entity.Property(e => e.ParliamentaryConstituencyCode)
                .IsUnicode(false)
                .HasColumnName("ParliamentaryConstituency (code)");
            entity.Property(e => e.ParliamentaryConstituencyName)
                .IsUnicode(false)
                .HasColumnName("ParliamentaryConstituency (name)");
            entity.Property(e => e.PercentageFsm)
                .IsUnicode(false)
                .HasColumnName("PercentageFSM");
            entity.Property(e => e.PhaseOfEducationCode)
                .IsUnicode(false)
                .HasColumnName("PhaseOfEducation (code)");
            entity.Property(e => e.PhaseOfEducationName)
                .IsUnicode(false)
                .HasColumnName("PhaseOfEducation (name)");
            entity.Property(e => e.PlacesPru)
                .IsUnicode(false)
                .HasColumnName("PlacesPRU");
            entity.Property(e => e.Postcode).IsUnicode(false);
            entity.Property(e => e.PreviousEstablishmentNumber).IsUnicode(false);
            entity.Property(e => e.PreviousLaCode)
                .IsUnicode(false)
                .HasColumnName("PreviousLA (code)");
            entity.Property(e => e.PreviousLaName)
                .IsUnicode(false)
                .HasColumnName("PreviousLA (name)");
            entity.Property(e => e.PropsName).IsUnicode(false);
            entity.Property(e => e.QabnameCode)
                .IsUnicode(false)
                .HasColumnName("QABName (code)");
            entity.Property(e => e.QabnameName)
                .IsUnicode(false)
                .HasColumnName("QABName (name)");
            entity.Property(e => e.Qabreport)
                .IsUnicode(false)
                .HasColumnName("QABReport");
            entity.Property(e => e.ReasonEstablishmentClosedCode)
                .IsUnicode(false)
                .HasColumnName("ReasonEstablishmentClosed (code)");
            entity.Property(e => e.ReasonEstablishmentClosedName)
                .IsUnicode(false)
                .HasColumnName("ReasonEstablishmentClosed (name)");
            entity.Property(e => e.ReasonEstablishmentOpenedCode)
                .IsUnicode(false)
                .HasColumnName("ReasonEstablishmentOpened (code)");
            entity.Property(e => e.ReasonEstablishmentOpenedName)
                .IsUnicode(false)
                .HasColumnName("ReasonEstablishmentOpened (name)");
            entity.Property(e => e.ReligiousCharacterCode)
                .IsUnicode(false)
                .HasColumnName("ReligiousCharacter (code)");
            entity.Property(e => e.ReligiousCharacterName)
                .IsUnicode(false)
                .HasColumnName("ReligiousCharacter (name)");
            entity.Property(e => e.ReligiousEthosName)
                .IsUnicode(false)
                .HasColumnName("ReligiousEthos (name)");
            entity.Property(e => e.ResourcedProvisionCapacity).IsUnicode(false);
            entity.Property(e => e.ResourcedProvisionOnRoll).IsUnicode(false);
            entity.Property(e => e.RscregionName)
                .IsUnicode(false)
                .HasColumnName("RSCRegion (name)");
            entity.Property(e => e.SchoolCapacity).IsUnicode(false);
            entity.Property(e => e.SchoolSponsorFlagName)
                .IsUnicode(false)
                .HasColumnName("SchoolSponsorFlag (name)");
            entity.Property(e => e.SchoolSponsorsName)
                .IsUnicode(false)
                .HasColumnName("SchoolSponsors (name)");
            entity.Property(e => e.SchoolWebsite).IsUnicode(false);
            entity.Property(e => e.Section41ApprovedName)
                .IsUnicode(false)
                .HasColumnName("Section41Approved (name)");
            entity.Property(e => e.Sen10Name)
                .IsUnicode(false)
                .HasColumnName("SEN10 (name)");
            entity.Property(e => e.Sen11Name)
                .IsUnicode(false)
                .HasColumnName("SEN11 (name)");
            entity.Property(e => e.Sen12Name)
                .IsUnicode(false)
                .HasColumnName("SEN12 (name)");
            entity.Property(e => e.Sen13Name)
                .IsUnicode(false)
                .HasColumnName("SEN13 (name)");
            entity.Property(e => e.Sen1Name)
                .IsUnicode(false)
                .HasColumnName("SEN1 (name)");
            entity.Property(e => e.Sen2Name)
                .IsUnicode(false)
                .HasColumnName("SEN2 (name)");
            entity.Property(e => e.Sen3Name)
                .IsUnicode(false)
                .HasColumnName("SEN3 (name)");
            entity.Property(e => e.Sen4Name)
                .IsUnicode(false)
                .HasColumnName("SEN4 (name)");
            entity.Property(e => e.Sen5Name)
                .IsUnicode(false)
                .HasColumnName("SEN5 (name)");
            entity.Property(e => e.Sen6Name)
                .IsUnicode(false)
                .HasColumnName("SEN6 (name)");
            entity.Property(e => e.Sen7Name)
                .IsUnicode(false)
                .HasColumnName("SEN7 (name)");
            entity.Property(e => e.Sen8Name)
                .IsUnicode(false)
                .HasColumnName("SEN8 (name)");
            entity.Property(e => e.Sen9Name)
                .IsUnicode(false)
                .HasColumnName("SEN9 (name)");
            entity.Property(e => e.SenUnitCapacity).IsUnicode(false);
            entity.Property(e => e.SenUnitOnRoll).IsUnicode(false);
            entity.Property(e => e.SennoStat)
                .IsUnicode(false)
                .HasColumnName("SENNoStat");
            entity.Property(e => e.SenpruName)
                .IsUnicode(false)
                .HasColumnName("SENPRU (name)");
            entity.Property(e => e.Senstat)
                .IsUnicode(false)
                .HasColumnName("SENStat");
            entity.Property(e => e.SiteName).IsUnicode(false);
            entity.Property(e => e.SpecialClassesCode)
                .IsUnicode(false)
                .HasColumnName("SpecialClasses (code)");
            entity.Property(e => e.SpecialClassesName)
                .IsUnicode(false)
                .HasColumnName("SpecialClasses (name)");
            entity.Property(e => e.StatutoryHighAge).IsUnicode(false);
            entity.Property(e => e.StatutoryLowAge).IsUnicode(false);
            entity.Property(e => e.Street).IsUnicode(false);
            entity.Property(e => e.TeenMothName)
                .IsUnicode(false)
                .HasColumnName("TeenMoth (name)");
            entity.Property(e => e.TeenMothPlaces).IsUnicode(false);
            entity.Property(e => e.TelephoneNum).IsUnicode(false);
            entity.Property(e => e.Town).IsUnicode(false);
            entity.Property(e => e.TrustSchoolFlagCode)
                .IsUnicode(false)
                .HasColumnName("TrustSchoolFlag (code)");
            entity.Property(e => e.TrustSchoolFlagName)
                .IsUnicode(false)
                .HasColumnName("TrustSchoolFlag (name)");
            entity.Property(e => e.TrustsCode)
                .IsUnicode(false)
                .HasColumnName("Trusts (code)");
            entity.Property(e => e.TrustsName)
                .IsUnicode(false)
                .HasColumnName("Trusts (name)");
            entity.Property(e => e.TypeOfEstablishmentCode)
                .IsUnicode(false)
                .HasColumnName("TypeOfEstablishment (code)");
            entity.Property(e => e.TypeOfEstablishmentName)
                .IsUnicode(false)
                .HasColumnName("TypeOfEstablishment (name)");
            entity.Property(e => e.TypeOfResourcedProvisionName)
                .IsUnicode(false)
                .HasColumnName("TypeOfResourcedProvision (name)");
            entity.Property(e => e.Ukprn)
                .IsUnicode(false)
                .HasColumnName("UKPRN");
            entity.Property(e => e.Uprn)
                .IsUnicode(false)
                .HasColumnName("UPRN");
            entity.Property(e => e.UrbanRuralCode)
                .IsUnicode(false)
                .HasColumnName("UrbanRural (code)");
            entity.Property(e => e.UrbanRuralName)
                .IsUnicode(false)
                .HasColumnName("UrbanRural (name)");
        });

        modelBuilder.Entity<Governance>(entity =>
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

        modelBuilder.Entity<GroupLink>(entity =>
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
        });

        modelBuilder.Entity<Group>(entity =>
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

        modelBuilder.Entity<MstrTrust>(entity =>
        {
            entity
                .HasKey(e => e.GroupUid);
            entity.ToTable("Trust", "mstr");

            entity.Property(e => e.GroupUid)
                .IsUnicode(false)
                .ValueGeneratedNever()
                .HasColumnName("Group UID");

            entity.Property(e => e.GORregion)
                .IsUnicode(false)
                .HasColumnName("GORregion");
        });
    }
}

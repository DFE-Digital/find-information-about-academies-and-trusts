using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb;

public interface IAcademiesDbContext
{
    DbSet<GiasEstablishment> GiasEstablishments { get; }
    DbSet<GiasGovernance> GiasGovernances { get; }
    DbSet<GroupLink> GroupLinks { get; }
    DbSet<GiasGroup> Groups { get; }
    DbSet<MstrTrust> MstrTrusts { get; }
    DbSet<CdmAccount> CdmAccounts { get; }
    DbSet<MisEstablishment> MisEstablishments { get; }
    DbSet<CdmSystemuser> CdmSystemusers { get; }
    DbSet<MstrTrustGovernance> MstrTrustGovernances { get; }
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

    public DbSet<GiasEstablishment> GiasEstablishments { get; set; }
    public DbSet<GiasGovernance> GiasGovernances { get; set; }
    public DbSet<GroupLink> GroupLinks { get; set; }
    public DbSet<GiasGroup> Groups { get; set; }
    public DbSet<MstrTrust> MstrTrusts { get; set; }
    public DbSet<CdmAccount> CdmAccounts { get; set; }
    public DbSet<MisEstablishment> MisEstablishments { get; set; }
    public DbSet<CdmSystemuser> CdmSystemusers { get; set; }
    public DbSet<MstrTrustGovernance> MstrTrustGovernances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<GiasEstablishment>(entity =>
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

        modelBuilder.Entity<CdmAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("EPK[cdm].[account2]");

            entity.ToTable("account", "cdm");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Accountcategorycode).HasColumnName("accountcategorycode");
            entity.Property(e => e.Accountclassificationcode).HasColumnName("accountclassificationcode");
            entity.Property(e => e.Accountid).HasColumnName("accountid");
            entity.Property(e => e.Accountnumber).HasColumnName("accountnumber");
            entity.Property(e => e.Accountratingcode).HasColumnName("accountratingcode");
            entity.Property(e => e.Address1Addressid).HasColumnName("address1_addressid");
            entity.Property(e => e.Address1Addresstypecode).HasColumnName("address1_addresstypecode");
            entity.Property(e => e.Address1City).HasColumnName("address1_city");
            entity.Property(e => e.Address1Composite).HasColumnName("address1_composite");
            entity.Property(e => e.Address1Country).HasColumnName("address1_country");
            entity.Property(e => e.Address1County).HasColumnName("address1_county");
            entity.Property(e => e.Address1Fax).HasColumnName("address1_fax");
            entity.Property(e => e.Address1Freighttermscode).HasColumnName("address1_freighttermscode");
            entity.Property(e => e.Address1Latitude).HasColumnName("address1_latitude");
            entity.Property(e => e.Address1Line1).HasColumnName("address1_line1");
            entity.Property(e => e.Address1Line2).HasColumnName("address1_line2");
            entity.Property(e => e.Address1Line3).HasColumnName("address1_line3");
            entity.Property(e => e.Address1Longitude).HasColumnName("address1_longitude");
            entity.Property(e => e.Address1Name).HasColumnName("address1_name");
            entity.Property(e => e.Address1Postalcode).HasColumnName("address1_postalcode");
            entity.Property(e => e.Address1Postofficebox).HasColumnName("address1_postofficebox");
            entity.Property(e => e.Address1Primarycontactname).HasColumnName("address1_primarycontactname");
            entity.Property(e => e.Address1Shippingmethodcode).HasColumnName("address1_shippingmethodcode");
            entity.Property(e => e.Address1Stateorprovince).HasColumnName("address1_stateorprovince");
            entity.Property(e => e.Address1Telephone1).HasColumnName("address1_telephone1");
            entity.Property(e => e.Address1Telephone2).HasColumnName("address1_telephone2");
            entity.Property(e => e.Address1Telephone3).HasColumnName("address1_telephone3");
            entity.Property(e => e.Address1Upszone).HasColumnName("address1_upszone");
            entity.Property(e => e.Address1Utcoffset).HasColumnName("address1_utcoffset");
            entity.Property(e => e.Address2Addressid).HasColumnName("address2_addressid");
            entity.Property(e => e.Address2Addresstypecode).HasColumnName("address2_addresstypecode");
            entity.Property(e => e.Address2City).HasColumnName("address2_city");
            entity.Property(e => e.Address2Composite).HasColumnName("address2_composite");
            entity.Property(e => e.Address2Country).HasColumnName("address2_country");
            entity.Property(e => e.Address2County).HasColumnName("address2_county");
            entity.Property(e => e.Address2Fax).HasColumnName("address2_fax");
            entity.Property(e => e.Address2Freighttermscode).HasColumnName("address2_freighttermscode");
            entity.Property(e => e.Address2Latitude).HasColumnName("address2_latitude");
            entity.Property(e => e.Address2Line1).HasColumnName("address2_line1");
            entity.Property(e => e.Address2Line2).HasColumnName("address2_line2");
            entity.Property(e => e.Address2Line3).HasColumnName("address2_line3");
            entity.Property(e => e.Address2Longitude).HasColumnName("address2_longitude");
            entity.Property(e => e.Address2Name).HasColumnName("address2_name");
            entity.Property(e => e.Address2Postalcode).HasColumnName("address2_postalcode");
            entity.Property(e => e.Address2Postofficebox).HasColumnName("address2_postofficebox");
            entity.Property(e => e.Address2Primarycontactname).HasColumnName("address2_primarycontactname");
            entity.Property(e => e.Address2Shippingmethodcode).HasColumnName("address2_shippingmethodcode");
            entity.Property(e => e.Address2Stateorprovince).HasColumnName("address2_stateorprovince");
            entity.Property(e => e.Address2Telephone1).HasColumnName("address2_telephone1");
            entity.Property(e => e.Address2Telephone2).HasColumnName("address2_telephone2");
            entity.Property(e => e.Address2Telephone3).HasColumnName("address2_telephone3");
            entity.Property(e => e.Address2Upszone).HasColumnName("address2_upszone");
            entity.Property(e => e.Address2Utcoffset).HasColumnName("address2_utcoffset");
            entity.Property(e => e.AdxCreatedbyipaddress).HasColumnName("adx_createdbyipaddress");
            entity.Property(e => e.AdxCreatedbyusername).HasColumnName("adx_createdbyusername");
            entity.Property(e => e.AdxModifiedbyipaddress).HasColumnName("adx_modifiedbyipaddress");
            entity.Property(e => e.AdxModifiedbyusername).HasColumnName("adx_modifiedbyusername");
            entity.Property(e => e.Aging30)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging30");
            entity.Property(e => e.Aging30Base)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging30_base");
            entity.Property(e => e.Aging60)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging60");
            entity.Property(e => e.Aging60Base)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging60_base");
            entity.Property(e => e.Aging90)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging90");
            entity.Property(e => e.Aging90Base)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("aging90_base");
            entity.Property(e => e.Businesstypecode).HasColumnName("businesstypecode");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdbyexternalparty).HasColumnName("createdbyexternalparty");
            entity.Property(e => e.Createdbyexternalpartyname).HasColumnName("createdbyexternalpartyname");
            entity.Property(e => e.Createdbyexternalpartyyominame).HasColumnName("createdbyexternalpartyyominame");
            entity.Property(e => e.Createdbyname).HasColumnName("createdbyname");
            entity.Property(e => e.Createdbyyominame).HasColumnName("createdbyyominame");
            entity.Property(e => e.Createdon).HasColumnName("createdon");
            entity.Property(e => e.Createdonbehalfby).HasColumnName("createdonbehalfby");
            entity.Property(e => e.Createdonbehalfbyname).HasColumnName("createdonbehalfbyname");
            entity.Property(e => e.Createdonbehalfbyyominame).HasColumnName("createdonbehalfbyyominame");
            entity.Property(e => e.Creditlimit)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("creditlimit");
            entity.Property(e => e.CreditlimitBase)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("creditlimit_base");
            entity.Property(e => e.Creditonhold).HasColumnName("creditonhold");
            entity.Property(e => e.Customersizecode).HasColumnName("customersizecode");
            entity.Property(e => e.Customertypecode).HasColumnName("customertypecode");
            entity.Property(e => e.Defaultpricelevelid).HasColumnName("defaultpricelevelid");
            entity.Property(e => e.Defaultpricelevelidname).HasColumnName("defaultpricelevelidname");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Donotbulkemail).HasColumnName("donotbulkemail");
            entity.Property(e => e.Donotbulkpostalmail).HasColumnName("donotbulkpostalmail");
            entity.Property(e => e.Donotemail).HasColumnName("donotemail");
            entity.Property(e => e.Donotfax).HasColumnName("donotfax");
            entity.Property(e => e.Donotphone).HasColumnName("donotphone");
            entity.Property(e => e.Donotpostalmail).HasColumnName("donotpostalmail");
            entity.Property(e => e.Donotsendmm).HasColumnName("donotsendmm");
            entity.Property(e => e.Emailaddress1).HasColumnName("emailaddress1");
            entity.Property(e => e.Emailaddress2).HasColumnName("emailaddress2");
            entity.Property(e => e.Emailaddress3).HasColumnName("emailaddress3");
            entity.Property(e => e.EntityimageTimestamp).HasColumnName("entityimage_timestamp");
            entity.Property(e => e.EntityimageUrl).HasColumnName("entityimage_url");
            entity.Property(e => e.Entityimageid).HasColumnName("entityimageid");
            entity.Property(e => e.Exchangerate)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("exchangerate");
            entity.Property(e => e.Fax).HasColumnName("fax");
            entity.Property(e => e.Followemail).HasColumnName("followemail");
            entity.Property(e => e.Ftpsiteurl).HasColumnName("ftpsiteurl");
            entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");
            entity.Property(e => e.Industrycode).HasColumnName("industrycode");
            entity.Property(e => e.Isprivate).HasColumnName("isprivate");
            entity.Property(e => e.Lastonholdtime).HasColumnName("lastonholdtime");
            entity.Property(e => e.Lastusedincampaign).HasColumnName("lastusedincampaign");
            entity.Property(e => e.Marketcap)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("marketcap");
            entity.Property(e => e.MarketcapBase)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("marketcap_base");
            entity.Property(e => e.Marketingonly).HasColumnName("marketingonly");
            entity.Property(e => e.Masteraccountidname).HasColumnName("masteraccountidname");
            entity.Property(e => e.Masteraccountidyominame).HasColumnName("masteraccountidyominame");
            entity.Property(e => e.Masterid).HasColumnName("masterid");
            entity.Property(e => e.Merged).HasColumnName("merged");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiedbyexternalparty).HasColumnName("modifiedbyexternalparty");
            entity.Property(e => e.Modifiedbyexternalpartyname).HasColumnName("modifiedbyexternalpartyname");
            entity.Property(e => e.Modifiedbyexternalpartyyominame).HasColumnName("modifiedbyexternalpartyyominame");
            entity.Property(e => e.Modifiedbyname).HasColumnName("modifiedbyname");
            entity.Property(e => e.Modifiedbyyominame).HasColumnName("modifiedbyyominame");
            entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");
            entity.Property(e => e.Modifiedonbehalfby).HasColumnName("modifiedonbehalfby");
            entity.Property(e => e.Modifiedonbehalfbyname).HasColumnName("modifiedonbehalfbyname");
            entity.Property(e => e.Modifiedonbehalfbyyominame).HasColumnName("modifiedonbehalfbyyominame");
            entity.Property(e => e.MsaManagingpartnerid).HasColumnName("msa_managingpartnerid");
            entity.Property(e => e.MsaManagingpartneridname).HasColumnName("msa_managingpartneridname");
            entity.Property(e => e.MsaManagingpartneridyominame).HasColumnName("msa_managingpartneridyominame");
            entity.Property(e => e.MsdynAccountkpiid).HasColumnName("msdyn_accountkpiid");
            entity.Property(e => e.MsdynAccountkpiidname).HasColumnName("msdyn_accountkpiidname");
            entity.Property(e => e.MsdynGdproptout).HasColumnName("msdyn_gdproptout");
            entity.Property(e => e.MsdynSalesaccelerationinsightid).HasColumnName("msdyn_salesaccelerationinsightid");
            entity.Property(e => e.MsdynSalesaccelerationinsightidname)
                .HasColumnName("msdyn_salesaccelerationinsightidname");
            entity.Property(e => e.MsdynSegmentid).HasColumnName("msdyn_segmentid");
            entity.Property(e => e.MsdynSegmentidname).HasColumnName("msdyn_segmentidname");
            entity.Property(e => e.MsftDatastate).HasColumnName("msft_datastate");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Numberofemployees).HasColumnName("numberofemployees");
            entity.Property(e => e.Onholdtime).HasColumnName("onholdtime");
            entity.Property(e => e.Opendeals).HasColumnName("opendeals");
            entity.Property(e => e.OpendealsDate).HasColumnName("opendeals_date");
            entity.Property(e => e.OpendealsState).HasColumnName("opendeals_state");
            entity.Property(e => e.Openrevenue)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("openrevenue");
            entity.Property(e => e.OpenrevenueBase)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("openrevenue_base");
            entity.Property(e => e.OpenrevenueDate).HasColumnName("openrevenue_date");
            entity.Property(e => e.OpenrevenueState).HasColumnName("openrevenue_state");
            entity.Property(e => e.Originatingleadid).HasColumnName("originatingleadid");
            entity.Property(e => e.Originatingleadidname).HasColumnName("originatingleadidname");
            entity.Property(e => e.Originatingleadidyominame).HasColumnName("originatingleadidyominame");
            entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");
            entity.Property(e => e.Ownerid).HasColumnName("ownerid");
            entity.Property(e => e.Owneridname).HasColumnName("owneridname");
            entity.Property(e => e.Owneridtype).HasColumnName("owneridtype");
            entity.Property(e => e.Owneridyominame).HasColumnName("owneridyominame");
            entity.Property(e => e.Ownershipcode).HasColumnName("ownershipcode");
            entity.Property(e => e.Owningbusinessunit).HasColumnName("owningbusinessunit");
            entity.Property(e => e.Owningbusinessunitname).HasColumnName("owningbusinessunitname");
            entity.Property(e => e.Owningteam).HasColumnName("owningteam");
            entity.Property(e => e.Owninguser).HasColumnName("owninguser");
            entity.Property(e => e.Parentaccountid).HasColumnName("parentaccountid");
            entity.Property(e => e.Parentaccountidname).HasColumnName("parentaccountidname");
            entity.Property(e => e.Parentaccountidyominame).HasColumnName("parentaccountidyominame");
            entity.Property(e => e.Participatesinworkflow).HasColumnName("participatesinworkflow");
            entity.Property(e => e.Paymenttermscode).HasColumnName("paymenttermscode");
            entity.Property(e => e.Preferredappointmentdaycode).HasColumnName("preferredappointmentdaycode");
            entity.Property(e => e.Preferredappointmenttimecode).HasColumnName("preferredappointmenttimecode");
            entity.Property(e => e.Preferredcontactmethodcode).HasColumnName("preferredcontactmethodcode");
            entity.Property(e => e.Preferredequipmentid).HasColumnName("preferredequipmentid");
            entity.Property(e => e.Preferredequipmentidname).HasColumnName("preferredequipmentidname");
            entity.Property(e => e.Preferredserviceid).HasColumnName("preferredserviceid");
            entity.Property(e => e.Preferredserviceidname).HasColumnName("preferredserviceidname");
            entity.Property(e => e.Preferredsystemuserid).HasColumnName("preferredsystemuserid");
            entity.Property(e => e.Preferredsystemuseridname).HasColumnName("preferredsystemuseridname");
            entity.Property(e => e.Preferredsystemuseridyominame).HasColumnName("preferredsystemuseridyominame");
            entity.Property(e => e.Primarycontactid).HasColumnName("primarycontactid");
            entity.Property(e => e.Primarycontactidname).HasColumnName("primarycontactidname");
            entity.Property(e => e.Primarycontactidyominame).HasColumnName("primarycontactidyominame");
            entity.Property(e => e.Primarysatoriid).HasColumnName("primarysatoriid");
            entity.Property(e => e.Primarytwitterid).HasColumnName("primarytwitterid");
            entity.Property(e => e.Processid).HasColumnName("processid");
            entity.Property(e => e.Revenue)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("revenue");
            entity.Property(e => e.RevenueBase)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("revenue_base");
            entity.Property(e => e.Sharesoutstanding).HasColumnName("sharesoutstanding");
            entity.Property(e => e.Shippingmethodcode).HasColumnName("shippingmethodcode");
            entity.Property(e => e.Sic).HasColumnName("sic");
            entity.Property(e => e.Sip9gridboxbanding).HasColumnName("sip_9gridboxbanding");
            entity.Property(e => e.SipAcademyprojectlead).HasColumnName("sip_academyprojectlead");
            entity.Property(e => e.SipAcademyriskgrade).HasColumnName("sip_academyriskgrade");
            entity.Property(e => e.SipAcademywheretrustsat).HasColumnName("sip_academywheretrustsat");
            entity.Property(e => e.SipAcademywheretrustsatname).HasColumnName("sip_academywheretrustsatname");
            entity.Property(e => e.SipAcademywheretrustsatyominame).HasColumnName("sip_academywheretrustsatyominame");
            entity.Property(e => e.SipAdministrativedistrict).HasColumnName("sip_administrativedistrict");
            entity.Property(e => e.SipAgerange).HasColumnName("sip_agerange");
            entity.Property(e => e.SipAmsdlead).HasColumnName("sip_amsdlead");
            entity.Property(e => e.SipAmsdleadname).HasColumnName("sip_amsdleadname");
            entity.Property(e => e.SipAmsdleadyominame).HasColumnName("sip_amsdleadyominame");
            entity.Property(e => e.SipAmsdterritoryid).HasColumnName("sip_amsdterritoryid");
            entity.Property(e => e.SipAmsdterritoryidname).HasColumnName("sip_amsdterritoryidname");
            entity.Property(e => e.SipBehaviourandattitudes).HasColumnName("sip_behaviourandattitudes");
            entity.Property(e => e.SipCapacitybeyondopenandpipeline).HasColumnName("sip_capacitybeyondopenandpipeline");
            entity.Property(e => e.SipClosurereasons).HasColumnName("sip_closurereasons");
            entity.Property(e => e.SipCompaddress).HasColumnName("sip_compaddress");
            entity.Property(e => e.SipCompanieshousefilinghistory).HasColumnName("sip_companieshousefilinghistory");
            entity.Property(e => e.SipCompanieshousenumber).HasColumnName("sip_companieshousenumber");
            entity.Property(e => e.SipCompanieshouseofficersurl).HasColumnName("sip_companieshouseofficersurl");
            entity.Property(e => e.SipCompositeaddress).HasColumnName("sip_compositeaddress");
            entity.Property(e => e.SipConcerntotaldart).HasColumnName("sip_concerntotaldart");
            entity.Property(e => e.SipConcerntotalkim).HasColumnName("sip_concerntotalkim");
            entity.Property(e => e.SipConstituencyid).HasColumnName("sip_constituencyid");
            entity.Property(e => e.SipConstituencyidname).HasColumnName("sip_constituencyidname");
            entity.Property(e => e.SipCountofprimaryphase).HasColumnName("sip_countofprimaryphase");
            entity.Property(e => e.SipCurrentsinglelistgrouping).HasColumnName("sip_currentsinglelistgrouping");
            entity.Property(e => e.SipDateactionplannedfor).HasColumnName("sip_dateactionplannedfor");
            entity.Property(e => e.SipDateclosed).HasColumnName("sip_dateclosed");
            entity.Property(e => e.SipDateenteredontosinglelist).HasColumnName("sip_dateenteredontosinglelist");
            entity.Property(e => e.SipDateofestablishment).HasColumnName("sip_dateofestablishment");
            entity.Property(e => e.SipDateofgroupingdecision).HasColumnName("sip_dateofgroupingdecision");
            entity.Property(e => e.SipDateofjoiningtrust).HasColumnName("sip_dateofjoiningtrust");
            entity.Property(e => e.SipDateoflasttrustreview).HasColumnName("sip_dateoflasttrustreview");
            entity.Property(e => e.SipDateoflatestsection8inspection)
                .HasColumnName("sip_dateoflatestsection8inspection");
            entity.Property(e => e.SipDateoftrustreviewmeeting).HasColumnName("sip_dateoftrustreviewmeeting");
            entity.Property(e => e.SipDfenumber).HasColumnName("sip_dfenumber");
            entity.Property(e => e.SipDiocesanmat).HasColumnName("sip_diocesanmat");
            entity.Property(e => e.SipDiocesanmatpercentagego)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_diocesanmatpercentagego");
            entity.Property(e => e.SipDioceseid).HasColumnName("sip_dioceseid");
            entity.Property(e => e.SipDioceseidname).HasColumnName("sip_dioceseidname");
            entity.Property(e => e.SipDisplayepprimarysections).HasColumnName("sip_displayepprimarysections");
            entity.Property(e => e.SipDisplayepsecondarysections).HasColumnName("sip_displayepsecondarysections");
            entity.Property(e => e.SipDisplayeptrustprimary).HasColumnName("sip_displayeptrustprimary");
            entity.Property(e => e.SipDisplayeptrustsecondary).HasColumnName("sip_displayeptrustsecondary");
            entity.Property(e => e.SipDtrtoollink).HasColumnName("sip_dtrtoollink");
            entity.Property(e => e.SipEarlyyearsprovisionwhereapplicable)
                .HasColumnName("sip_earlyyearsprovisionwhereapplicable");
            entity.Property(e => e.SipEffectivenessofleadershipandmanagement)
                .HasColumnName("sip_effectivenessofleadershipandmanagement");
            entity.Property(e => e.SipEfficiencyicfpreviewcompleted).HasColumnName("sip_efficiencyicfpreviewcompleted");
            entity.Property(e => e.SipEfficiencyicfpreviewother).HasColumnName("sip_efficiencyicfpreviewother");
            entity.Property(e => e.SipEstablishmentlinktype).HasColumnName("sip_establishmentlinktype");
            entity.Property(e => e.SipEstablishmentnumber).HasColumnName("sip_establishmentnumber");
            entity.Property(e => e.SipEstablishmenttypeid).HasColumnName("sip_establishmenttypeid");
            entity.Property(e => e.SipEstablishmenttypeidname).HasColumnName("sip_establishmenttypeidname");
            entity.Property(e => e.SipEstablismenttypegroupid).HasColumnName("sip_establismenttypegroupid");
            entity.Property(e => e.SipEstablismenttypegroupidname).HasColumnName("sip_establismenttypegroupidname");
            entity.Property(e => e.SipExpectedopeningdate).HasColumnName("sip_expectedopeningdate");
            entity.Property(e => e.SipExternalgovernancereviewdate).HasColumnName("sip_externalgovernancereviewdate");
            entity.Property(e => e.SipFinanceriskgroup).HasColumnName("sip_financeriskgroup");
            entity.Property(e => e.SipFinanceriskgroupmajorrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_financeriskgroupmajorrisks");
            entity.Property(e => e.SipFinanceriskgroupriskscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_financeriskgroupriskscore");
            entity.Property(e => e.SipFinanceriskgrouptotalrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_financeriskgrouptotalrisks");
            entity.Property(e => e.SipFntistatus).HasColumnName("sip_fntistatus");
            entity.Property(e => e.SipFollowuplettersent).HasColumnName("sip_followuplettersent");
            entity.Property(e => e.SipFormswitcher).HasColumnName("sip_formswitcher");
            entity.Property(e => e.SipFreeschoolmeals)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_freeschoolmeals");
            entity.Property(e => e.SipFundingagreementpageurl).HasColumnName("sip_fundingagreementpageurl");
            entity.Property(e => e.SipFutureplans).HasColumnName("sip_futureplans");
            entity.Property(e => e.SipGorregion).HasColumnName("sip_gorregion");
            entity.Property(e => e.SipGovernanceandtrustboard).HasColumnName("sip_governanceandtrustboard");
            entity.Property(e => e.SipGovernanceriskgroup).HasColumnName("sip_governanceriskgroup");
            entity.Property(e => e.SipGovernanceriskgroupmajorrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_governanceriskgroupmajorrisks");
            entity.Property(e => e.SipGovernanceriskgroupriskscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_governanceriskgroupriskscore");
            entity.Property(e => e.SipGovernanceriskgrouptotalrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_governanceriskgrouptotalrisks");
            entity.Property(e => e.SipHasthematsponsorreceivedragfmdifotherfund)
                .HasColumnName("sip_hasthematsponsorreceivedragfmdifotherfund");
            entity.Property(e => e.SipHowmuchandwhatitwasfor).HasColumnName("sip_howmuchandwhatitwasfor");
            entity.Property(e => e.SipInadequaterequiresimprovementschoolscalc)
                .HasColumnName("sip_inadequaterequiresimprovementschoolscalc");
            entity.Property(e => e.SipInadequaterequiresimprovementschoolscalcDate)
                .HasColumnName("sip_inadequaterequiresimprovementschoolscalc_date");
            entity.Property(e => e.SipInadequaterequiresimprovementschoolscalcState)
                .HasColumnName("sip_inadequaterequiresimprovementschoolscalc_state");
            entity.Property(e => e.SipIncorporateddate).HasColumnName("sip_incorporateddate");
            entity.Property(e => e.SipInspectionforcurrenturn).HasColumnName("sip_inspectionforcurrenturn");
            entity.Property(e => e.SipIntegratedcurriculumfinancialplanningicfp)
                .HasColumnName("sip_integratedcurriculumfinancialplanningicfp");
            entity.Property(e => e.SipIpamaps).HasColumnName("sip_ipamaps");
            entity.Property(e => e.SipIrregularityriskgroup).HasColumnName("sip_irregularityriskgroup");
            entity.Property(e => e.SipIrregularityriskgroupmajorrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_irregularityriskgroupmajorrisks");
            entity.Property(e => e.SipIrregularityriskgroupriskscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_irregularityriskgroupriskscore");
            entity.Property(e => e.SipIrregularityriskgrouptotalrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_irregularityriskgrouptotalrisks");
            entity.Property(e => e.SipKeypeoplebios).HasColumnName("sip_keypeoplebios");
            entity.Property(e => e.SipKnowledgearticlefinanceglossary)
                .HasColumnName("sip_knowledgearticlefinanceglossary");
            entity.Property(e => e.SipKnowledgearticlefinanceglossaryname)
                .HasColumnName("sip_knowledgearticlefinanceglossaryname");
            entity.Property(e => e.SipLaestabnumber).HasColumnName("sip_laestabnumber");
            entity.Property(e => e.SipLaestabtext).HasColumnName("sip_laestabtext");
            entity.Property(e => e.SipLatestmatbandingsource).HasColumnName("sip_latestmatbandingsource");
            entity.Property(e => e.SipLatestmatreviewscore).HasColumnName("sip_latestmatreviewscore");
            entity.Property(e => e.SipLatitude).HasColumnName("sip_latitude");
            entity.Property(e => e.SipLbawarelandnotregistered).HasColumnName("sip_lbawarelandnotregistered");
            entity.Property(e => e.SipLbawarelandnotregisteredexplained)
                .HasColumnName("sip_lbawarelandnotregisteredexplained");
            entity.Property(e => e.SipLbcurrentbuildinglandowner).HasColumnName("sip_lbcurrentbuildinglandowner");
            entity.Property(e => e.SipLeadamsdterritoryid).HasColumnName("sip_leadamsdterritoryid");
            entity.Property(e => e.SipLeadamsdterritoryidname).HasColumnName("sip_leadamsdterritoryidname");
            entity.Property(e => e.SipLeadrscareaid).HasColumnName("sip_leadrscareaid");
            entity.Property(e => e.SipLeadrscareaidname).HasColumnName("sip_leadrscareaidname");
            entity.Property(e => e.SipLinktoofstedpage).HasColumnName("sip_linktoofstedpage");
            entity.Property(e => e.SipLinktoworkplaceforefficiencyicfpreview)
                .HasColumnName("sip_linktoworkplaceforefficiencyicfpreview");
            entity.Property(e => e.SipLocalauthorityareaid).HasColumnName("sip_localauthorityareaid");
            entity.Property(e => e.SipLocalauthorityareaidname).HasColumnName("sip_localauthorityareaidname");
            entity.Property(e => e.SipLocalauthoritynumber).HasColumnName("sip_localauthoritynumber");
            entity.Property(e => e.SipLongitude).HasColumnName("sip_longitude");
            entity.Property(e => e.SipMaincontactsemail).HasColumnName("sip_maincontactsemail");
            entity.Property(e => e.SipMaincontactsphone).HasColumnName("sip_maincontactsphone");
            entity.Property(e => e.SipMainreasonforintervention).HasColumnName("sip_mainreasonforintervention");
            entity.Property(e => e.SipMaintrustcontactid).HasColumnName("sip_maintrustcontactid");
            entity.Property(e => e.SipMaintrustcontactidname).HasColumnName("sip_maintrustcontactidname");
            entity.Property(e => e.SipManagementletterinternalaudit).HasColumnName("sip_managementletterinternalaudit");
            entity.Property(e => e.SipMathsscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_mathsscore");
            entity.Property(e => e.SipMathsscorecategory).HasColumnName("sip_mathsscorecategory");
            entity.Property(e => e.SipNationalsponsoroversight).HasColumnName("sip_nationalsponsoroversight");
            entity.Property(e => e.SipNewtrustdate).HasColumnName("sip_newtrustdate");
            entity.Property(e => e.SipNewurn).HasColumnName("sip_newurn");
            entity.Property(e => e.SipNoeducationperformacedatamessage)
                .HasColumnName("sip_noeducationperformacedatamessage");
            entity.Property(e => e.SipNoeducationperformancedatatrustmessage)
                .HasColumnName("sip_noeducationperformancedatatrustmessage");
            entity.Property(e => e.SipNoofphaseallthrough).HasColumnName("sip_noofphaseallthrough");
            entity.Property(e => e.SipNoofphaseallthroughDate).HasColumnName("sip_noofphaseallthrough_date");
            entity.Property(e => e.SipNoofphaseallthroughState).HasColumnName("sip_noofphaseallthrough_state");
            entity.Property(e => e.SipNoofprimaryphase).HasColumnName("sip_noofprimaryphase");
            entity.Property(e => e.SipNoofprimaryphaseDate).HasColumnName("sip_noofprimaryphase_date");
            entity.Property(e => e.SipNoofprimaryphaseState).HasColumnName("sip_noofprimaryphase_state");
            entity.Property(e => e.SipNoofsecondaryphase).HasColumnName("sip_noofsecondaryphase");
            entity.Property(e => e.SipNoofsecondaryphaseDate).HasColumnName("sip_noofsecondaryphase_date");
            entity.Property(e => e.SipNoofsecondaryphaseState).HasColumnName("sip_noofsecondaryphase_state");
            entity.Property(e => e.SipNumberacademiesintrust).HasColumnName("sip_numberacademiesintrust");
            entity.Property(e => e.SipNumberofacademiescalculated).HasColumnName("sip_numberofacademiescalculated");
            entity.Property(e => e.SipNumberofacademiescalculatedDate)
                .HasColumnName("sip_numberofacademiescalculated_date");
            entity.Property(e => e.SipNumberofacademiescalculatedState)
                .HasColumnName("sip_numberofacademiescalculated_state");
            entity.Property(e => e.SipNumberofacademiesintrust).HasColumnName("sip_numberofacademiesintrust");
            entity.Property(e => e.SipNumberofactivedartconcerns).HasColumnName("sip_numberofactivedartconcerns");
            entity.Property(e => e.SipNumberofactivedartconcernsDate)
                .HasColumnName("sip_numberofactivedartconcerns_date");
            entity.Property(e => e.SipNumberofactivedartconcernsState)
                .HasColumnName("sip_numberofactivedartconcerns_state");
            entity.Property(e => e.SipNumberofactivekimconcerns).HasColumnName("sip_numberofactivekimconcerns");
            entity.Property(e => e.SipNumberofactivekimconcernsDate)
                .HasColumnName("sip_numberofactivekimconcerns_date");
            entity.Property(e => e.SipNumberofactivekimconcernsState)
                .HasColumnName("sip_numberofactivekimconcerns_state");
            entity.Property(e => e.SipNumberofdartconcernsacademy).HasColumnName("sip_numberofdartconcernsacademy");
            entity.Property(e => e.SipNumberofdartconcernsacademyDate)
                .HasColumnName("sip_numberofdartconcernsacademy_date");
            entity.Property(e => e.SipNumberofdartconcernsacademyState)
                .HasColumnName("sip_numberofdartconcernsacademy_state");
            entity.Property(e => e.SipNumberofkimconcernsacademy).HasColumnName("sip_numberofkimconcernsacademy");
            entity.Property(e => e.SipNumberofkimconcernsacademyDate)
                .HasColumnName("sip_numberofkimconcernsacademy_date");
            entity.Property(e => e.SipNumberofkimconcernsacademyState)
                .HasColumnName("sip_numberofkimconcernsacademy_state");
            entity.Property(e => e.SipNumberofoutstandingschoolscalculated)
                .HasColumnName("sip_numberofoutstandingschoolscalculated");
            entity.Property(e => e.SipNumberofoutstandingschoolscalculatedDate)
                .HasColumnName("sip_numberofoutstandingschoolscalculated_date");
            entity.Property(e => e.SipNumberofoutstandingschoolscalculatedState)
                .HasColumnName("sip_numberofoutstandingschoolscalculated_state");
            entity.Property(e => e.SipNumberofpupils).HasColumnName("sip_numberofpupils");
            entity.Property(e => e.SipNumberofrequiresimprovementinadequateacad)
                .HasColumnName("sip_numberofrequiresimprovementinadequateacad");
            entity.Property(e => e.SipOfstedgoodschoolsupdateddate).HasColumnName("sip_ofstedgoodschoolsupdateddate");
            entity.Property(e => e.SipOfstedinspectiondate).HasColumnName("sip_ofstedinspectiondate");
            entity.Property(e => e.SipOfstedinspectionrating).HasColumnName("sip_ofstedinspectionrating");
            entity.Property(e => e.SipOfstedrating).HasColumnName("sip_ofstedrating");
            entity.Property(e => e.SipOfstedratingupdated).HasColumnName("sip_ofstedratingupdated");
            entity.Property(e => e.SipOfstedtotalgood).HasColumnName("sip_ofstedtotalgood");
            entity.Property(e => e.SipOfstedtotalinadequat).HasColumnName("sip_ofstedtotalinadequat");
            entity.Property(e => e.SipOfstedtotalinadequate).HasColumnName("sip_ofstedtotalinadequate");
            entity.Property(e => e.SipOfstedtotaloutstanding).HasColumnName("sip_ofstedtotaloutstanding");
            entity.Property(e => e.SipOfstedtotalrequiresimprovement)
                .HasColumnName("sip_ofstedtotalrequiresimprovement");
            entity.Property(e => e.SipOrganisationtype).HasColumnName("sip_organisationtype");
            entity.Property(e => e.SipOriginatingapplicationid).HasColumnName("sip_originatingapplicationid");
            entity.Property(e => e.SipOriginatingapplicationidname).HasColumnName("sip_originatingapplicationidname");
            entity.Property(e => e.SipOriginatingapplyingschoolid).HasColumnName("sip_originatingapplyingschoolid");
            entity.Property(e => e.SipOriginatingapplyingschoolidname)
                .HasColumnName("sip_originatingapplyingschoolidname");
            entity.Property(e => e.SipOverallriskgroup).HasColumnName("sip_overallriskgroup");
            entity.Property(e => e.SipOverallriskgroupmajorrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_overallriskgroupmajorrisks");
            entity.Property(e => e.SipOverallriskgroupriskscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_overallriskgroupriskscore");
            entity.Property(e => e.SipOverallriskgrouptotalrisks)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_overallriskgrouptotalrisks");
            entity.Property(e => e.SipOverviewoffinancialstatusofthetrust)
                .HasColumnName("sip_overviewoffinancialstatusofthetrust");
            entity.Property(e => e.SipParentcompanyreference).HasColumnName("sip_parentcompanyreference");
            entity.Property(e => e.SipPercentageoffreeschoolmeals).HasColumnName("sip_percentageoffreeschoolmeals");
            entity.Property(e => e.SipPerformanceguid).HasColumnName("sip_performanceguid");
            entity.Property(e => e.SipPersonaldevelopment).HasColumnName("sip_personaldevelopment");
            entity.Property(e => e.SipPhase).HasColumnName("sip_phase");
            entity.Property(e => e.SipPipelineacademy).HasColumnName("sip_pipelineacademy");
            entity.Property(e => e.SipPipelinefreeschool).HasColumnName("sip_pipelinefreeschool");
            entity.Property(e => e.SipPredecessorestablishment).HasColumnName("sip_predecessorestablishment");
            entity.Property(e => e.SipPredecessorestablishmentname).HasColumnName("sip_predecessorestablishmentname");
            entity.Property(e => e.SipPredecessorestablishmentyominame)
                .HasColumnName("sip_predecessorestablishmentyominame");
            entity.Property(e => e.SipPredecessorurn).HasColumnName("sip_predecessorurn");
            entity.Property(e => e.SipPredictedchanceofchangeoccuring)
                .HasColumnName("sip_predictedchanceofchangeoccuring");
            entity.Property(e => e.SipPredictedchangeinprogress8score)
                .HasColumnName("sip_predictedchangeinprogress8score");
            entity.Property(e => e.SipPreviousinspectionfor).HasColumnName("sip_previousinspectionfor");
            entity.Property(e => e.SipPreviousofstedinspectiondate).HasColumnName("sip_previousofstedinspectiondate");
            entity.Property(e => e.SipPreviousofstedinspectionrating)
                .HasColumnName("sip_previousofstedinspectionrating");
            entity.Property(e => e.SipPreviousofstedrating).HasColumnName("sip_previousofstedrating");
            entity.Property(e => e.SipPrevioustrust).HasColumnName("sip_previoustrust");
            entity.Property(e => e.SipPrevioustrustname).HasColumnName("sip_previoustrustname");
            entity.Property(e => e.SipPrevioustrustyominame).HasColumnName("sip_previoustrustyominame");
            entity.Property(e => e.SipPrioritisedforareview).HasColumnName("sip_prioritisedforareview");
            entity.Property(e => e.SipProbabilityofdeclining).HasColumnName("sip_probabilityofdeclining");
            entity.Property(e => e.SipProbabilityofimproving).HasColumnName("sip_probabilityofimproving");
            entity.Property(e => e.SipProbabilityofstayingthesame).HasColumnName("sip_probabilityofstayingthesame");
            entity.Property(e => e.SipProjectleadforpipelineacademy).HasColumnName("sip_projectleadforpipelineacademy");
            entity.Property(e => e.SipProjectroute).HasColumnName("sip_projectroute");
            entity.Property(e => e.SipProposedmemberoftrustid).HasColumnName("sip_proposedmemberoftrustid");
            entity.Property(e => e.SipProposedmemberoftrustidname).HasColumnName("sip_proposedmemberoftrustidname");
            entity.Property(e => e.SipProposedmemberoftrustidyominame)
                .HasColumnName("sip_proposedmemberoftrustidyominame");
            entity.Property(e => e.SipPupilpremiumstrategy).HasColumnName("sip_pupilpremiumstrategy");
            entity.Property(e => e.SipQualityofeducation).HasColumnName("sip_qualityofeducation");
            entity.Property(e => e.SipRatgradedescription).HasColumnName("sip_ratgradedescription");
            entity.Property(e => e.SipReadingscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_readingscore");
            entity.Property(e => e.SipReadingscorecategory).HasColumnName("sip_readingscorecategory");
            entity.Property(e => e.SipRelationshipmanagerid).HasColumnName("sip_relationshipmanagerid");
            entity.Property(e => e.SipRelationshipmanageridname).HasColumnName("sip_relationshipmanageridname");
            entity.Property(e => e.SipRelationshipmanageridyominame).HasColumnName("sip_relationshipmanageridyominame");
            entity.Property(e => e.SipReligiouscharacterid).HasColumnName("sip_religiouscharacterid");
            entity.Property(e => e.SipReligiouscharacteridname).HasColumnName("sip_religiouscharacteridname");
            entity.Property(e => e.SipReligiousethosid).HasColumnName("sip_religiousethosid");
            entity.Property(e => e.SipReligiousethosidname).HasColumnName("sip_religiousethosidname");
            entity.Property(e => e.SipRiskratingnum).HasColumnName("sip_riskratingnum");
            entity.Property(e => e.SipRisksissues).HasColumnName("sip_risksissues");
            entity.Property(e => e.SipRouteofproject).HasColumnName("sip_routeofproject");
            entity.Property(e => e.SipRschtbtrustapprovaldate).HasColumnName("sip_rschtbtrustapprovaldate");
            entity.Property(e => e.SipSatlaestabnumber).HasColumnName("sip_satlaestabnumber");
            entity.Property(e => e.SipSatpredecessorurn).HasColumnName("sip_satpredecessorurn");
            entity.Property(e => e.SipSaturn).HasColumnName("sip_saturn");
            entity.Property(e => e.SipSchoolcapacity).HasColumnName("sip_schoolcapacity");
            entity.Property(e => e.SipSchoolfinancialbenchmarklink).HasColumnName("sip_schoolfinancialbenchmarklink");
            entity.Property(e => e.SipSchoolimprovementstrategy).HasColumnName("sip_schoolimprovementstrategy");
            entity.Property(e => e.SipScpnassumptions).HasColumnName("sip_scpnassumptions");
            entity.Property(e => e.SipScpnprojectedpupilonrollyear1).HasColumnName("sip_scpnprojectedpupilonrollyear1");
            entity.Property(e => e.SipScpnprojectedpupilonrollyear2).HasColumnName("sip_scpnprojectedpupilonrollyear2");
            entity.Property(e => e.SipScpnprojectedpupilonrollyear3).HasColumnName("sip_scpnprojectedpupilonrollyear3");
            entity.Property(e => e.SipSection8inspectionfor).HasColumnName("sip_section8inspectionfor");
            entity.Property(e => e.SipSection8inspectionoveralloutcome)
                .HasColumnName("sip_section8inspectionoveralloutcome");
            entity.Property(e => e.SipSfsoterritories).HasColumnName("sip_sfsoterritories");
            entity.Property(e => e.SipSharepointfinancialglossary).HasColumnName("sip_sharepointfinancialglossary");
            entity.Property(e => e.SipSharepointsdocs).HasColumnName("sip_sharepointsdocs");
            entity.Property(e => e.SipShortofstedinspcountsinceofstedinspdate)
                .HasColumnName("sip_shortofstedinspcountsinceofstedinspdate");
            entity.Property(e => e.SipShortofstedinspectiondate).HasColumnName("sip_shortofstedinspectiondate");
            entity.Property(e => e.SipSixthformprovisionwhereapplicable)
                .HasColumnName("sip_sixthformprovisionwhereapplicable");
            entity.Property(e => e.SipSk).HasColumnName("sip_sk");
            entity.Property(e => e.SipSponsorapprovaldate).HasColumnName("sip_sponsorapprovaldate");
            entity.Property(e => e.SipSponsorcoordinator).HasColumnName("sip_sponsorcoordinator");
            entity.Property(e => e.SipSponsorname).HasColumnName("sip_sponsorname");
            entity.Property(e => e.SipSponsoroverview).HasColumnName("sip_sponsoroverview");
            entity.Property(e => e.SipSponsorreferencenumber).HasColumnName("sip_sponsorreferencenumber");
            entity.Property(e => e.SipSponsorrestrictions).HasColumnName("sip_sponsorrestrictions");
            entity.Property(e => e.SipSponsorstatus).HasColumnName("sip_sponsorstatus");
            entity.Property(e => e.SipStatutoryhighage).HasColumnName("sip_statutoryhighage");
            entity.Property(e => e.SipStatutorylowage).HasColumnName("sip_statutorylowage");
            entity.Property(e => e.SipStubtrust).HasColumnName("sip_stubtrust");
            entity.Property(e => e.SipTopsliceorchargingpolicy).HasColumnName("sip_topsliceorchargingpolicy");
            entity.Property(e => e.SipTotalnumberofrisks).HasColumnName("sip_totalnumberofrisks");
            entity.Property(e => e.SipTotalriskscore).HasColumnName("sip_totalriskscore");
            entity.Property(e => e.SipTransfer).HasColumnName("sip_transfer");
            entity.Property(e => e.SipTransfername).HasColumnName("sip_transfername");
            entity.Property(e => e.SipTransferstatus).HasColumnName("sip_transferstatus");
            entity.Property(e => e.SipTransferstatusname).HasColumnName("sip_transferstatusname");
            entity.Property(e => e.SipTransfertype).HasColumnName("sip_transfertype");
            entity.Property(e => e.SipTransfertypename).HasColumnName("sip_transfertypename");
            entity.Property(e => e.SipTriggerflow).HasColumnName("sip_triggerflow");
            entity.Property(e => e.SipTrustconcernasnumber).HasColumnName("sip_trustconcernasnumber");
            entity.Property(e => e.SipTrusthighestconcernlevel).HasColumnName("sip_trusthighestconcernlevel");
            entity.Property(e => e.SipTrustperformanceandriskdateofmeeting)
                .HasColumnName("sip_trustperformanceandriskdateofmeeting");
            entity.Property(e => e.SipTrustreferencenumber).HasColumnName("sip_trustreferencenumber");
            entity.Property(e => e.SipTrustrelationshipmanager).HasColumnName("sip_trustrelationshipmanager");
            entity.Property(e => e.SipTrustrelationshipmanagername).HasColumnName("sip_trustrelationshipmanagername");
            entity.Property(e => e.SipTrustrelationshipmanageryominame)
                .HasColumnName("sip_trustrelationshipmanageryominame");
            entity.Property(e => e.SipTrustreviewwriteup).HasColumnName("sip_trustreviewwriteup");
            entity.Property(e => e.SipTrustriskgrade).HasColumnName("sip_trustriskgrade");
            entity.Property(e => e.SipUid).HasColumnName("sip_uid");
            entity.Property(e => e.SipUkprn).HasColumnName("sip_ukprn");
            entity.Property(e => e.SipUpin).HasColumnName("sip_upin");
            entity.Property(e => e.SipUrn).HasColumnName("sip_urn");
            entity.Property(e => e.SipUrnatcurrentofstedinspection).HasColumnName("sip_urnatcurrentofstedinspection");
            entity.Property(e => e.SipUrnatpreviousofstedinspection).HasColumnName("sip_urnatpreviousofstedinspection");
            entity.Property(e => e.SipUrnsection8ofstedinspection).HasColumnName("sip_urnsection8ofstedinspection");
            entity.Property(e => e.SipUrnwholenumber).HasColumnName("sip_urnwholenumber");
            entity.Property(e => e.SipWipsummarygoestominister).HasColumnName("sip_wipsummarygoestominister");
            entity.Property(e => e.SipWritingscore)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("sip_writingscore");
            entity.Property(e => e.SipWritingscorecategory).HasColumnName("sip_writingscorecategory");
            entity.Property(e => e.Slaid).HasColumnName("slaid");
            entity.Property(e => e.Slainvokedid).HasColumnName("slainvokedid");
            entity.Property(e => e.Slainvokedidname).HasColumnName("slainvokedidname");
            entity.Property(e => e.Slaname).HasColumnName("slaname");
            entity.Property(e => e.Stageid).HasColumnName("stageid");
            entity.Property(e => e.Statecode).HasColumnName("statecode");
            entity.Property(e => e.Statuscode).HasColumnName("statuscode");
            entity.Property(e => e.Stockexchange).HasColumnName("stockexchange");
            entity.Property(e => e.Teamsfollowed).HasColumnName("teamsfollowed");
            entity.Property(e => e.Telephone1).HasColumnName("telephone1");
            entity.Property(e => e.Telephone2).HasColumnName("telephone2");
            entity.Property(e => e.Telephone3).HasColumnName("telephone3");
            entity.Property(e => e.Territorycode).HasColumnName("territorycode");
            entity.Property(e => e.Territoryid).HasColumnName("territoryid");
            entity.Property(e => e.Territoryidname).HasColumnName("territoryidname");
            entity.Property(e => e.Tickersymbol).HasColumnName("tickersymbol");
            entity.Property(e => e.Timespentbymeonemailandmeetings).HasColumnName("timespentbymeonemailandmeetings");
            entity.Property(e => e.Timezoneruleversionnumber).HasColumnName("timezoneruleversionnumber");
            entity.Property(e => e.Transactioncurrencyid).HasColumnName("transactioncurrencyid");
            entity.Property(e => e.Transactioncurrencyidname).HasColumnName("transactioncurrencyidname");
            entity.Property(e => e.Traversedpath).HasColumnName("traversedpath");
            entity.Property(e => e.Utcconversiontimezonecode).HasColumnName("utcconversiontimezonecode");
            entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            entity.Property(e => e.Websiteurl).HasColumnName("websiteurl");
            entity.Property(e => e.Yominame).HasColumnName("yominame");
        });

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

        modelBuilder.Entity<CdmSystemuser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("systemuser", "cdm");

            entity.Property(e => e.Accessmode).HasColumnName("accessmode");
            entity.Property(e => e.Activedirectoryguid).HasColumnName("activedirectoryguid");
            entity.Property(e => e.Address1Addressid).HasColumnName("address1_addressid");
            entity.Property(e => e.Address1Addresstypecode).HasColumnName("address1_addresstypecode");
            entity.Property(e => e.Address1City).HasColumnName("address1_city");
            entity.Property(e => e.Address1Composite).HasColumnName("address1_composite");
            entity.Property(e => e.Address1Country).HasColumnName("address1_country");
            entity.Property(e => e.Address1County).HasColumnName("address1_county");
            entity.Property(e => e.Address1Fax).HasColumnName("address1_fax");
            entity.Property(e => e.Address1Latitude).HasColumnName("address1_latitude");
            entity.Property(e => e.Address1Line1).HasColumnName("address1_line1");
            entity.Property(e => e.Address1Line2).HasColumnName("address1_line2");
            entity.Property(e => e.Address1Line3).HasColumnName("address1_line3");
            entity.Property(e => e.Address1Longitude).HasColumnName("address1_longitude");
            entity.Property(e => e.Address1Name).HasColumnName("address1_name");
            entity.Property(e => e.Address1Postalcode).HasColumnName("address1_postalcode");
            entity.Property(e => e.Address1Postofficebox).HasColumnName("address1_postofficebox");
            entity.Property(e => e.Address1Shippingmethodcode).HasColumnName("address1_shippingmethodcode");
            entity.Property(e => e.Address1Stateorprovince).HasColumnName("address1_stateorprovince");
            entity.Property(e => e.Address1Telephone1).HasColumnName("address1_telephone1");
            entity.Property(e => e.Address1Telephone2).HasColumnName("address1_telephone2");
            entity.Property(e => e.Address1Telephone3).HasColumnName("address1_telephone3");
            entity.Property(e => e.Address1Upszone).HasColumnName("address1_upszone");
            entity.Property(e => e.Address1Utcoffset).HasColumnName("address1_utcoffset");
            entity.Property(e => e.Address2Addressid).HasColumnName("address2_addressid");
            entity.Property(e => e.Address2Addresstypecode).HasColumnName("address2_addresstypecode");
            entity.Property(e => e.Address2City).HasColumnName("address2_city");
            entity.Property(e => e.Address2Composite).HasColumnName("address2_composite");
            entity.Property(e => e.Address2Country).HasColumnName("address2_country");
            entity.Property(e => e.Address2County).HasColumnName("address2_county");
            entity.Property(e => e.Address2Fax).HasColumnName("address2_fax");
            entity.Property(e => e.Address2Latitude).HasColumnName("address2_latitude");
            entity.Property(e => e.Address2Line1).HasColumnName("address2_line1");
            entity.Property(e => e.Address2Line2).HasColumnName("address2_line2");
            entity.Property(e => e.Address2Line3).HasColumnName("address2_line3");
            entity.Property(e => e.Address2Longitude).HasColumnName("address2_longitude");
            entity.Property(e => e.Address2Name).HasColumnName("address2_name");
            entity.Property(e => e.Address2Postalcode).HasColumnName("address2_postalcode");
            entity.Property(e => e.Address2Postofficebox).HasColumnName("address2_postofficebox");
            entity.Property(e => e.Address2Shippingmethodcode).HasColumnName("address2_shippingmethodcode");
            entity.Property(e => e.Address2Stateorprovince).HasColumnName("address2_stateorprovince");
            entity.Property(e => e.Address2Telephone1).HasColumnName("address2_telephone1");
            entity.Property(e => e.Address2Telephone2).HasColumnName("address2_telephone2");
            entity.Property(e => e.Address2Telephone3).HasColumnName("address2_telephone3");
            entity.Property(e => e.Address2Upszone).HasColumnName("address2_upszone");
            entity.Property(e => e.Address2Utcoffset).HasColumnName("address2_utcoffset");
            entity.Property(e => e.Applicationid).HasColumnName("applicationid");
            entity.Property(e => e.Applicationiduri).HasColumnName("applicationiduri");
            entity.Property(e => e.Azureactivedirectoryobjectid).HasColumnName("azureactivedirectoryobjectid");
            entity.Property(e => e.Azuredeletedon).HasColumnName("azuredeletedon");
            entity.Property(e => e.Azurestate).HasColumnName("azurestate");
            entity.Property(e => e.Businessunitid).HasColumnName("businessunitid");
            entity.Property(e => e.Businessunitidname).HasColumnName("businessunitidname");
            entity.Property(e => e.Calendarid).HasColumnName("calendarid");
            entity.Property(e => e.Caltype).HasColumnName("caltype");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdbyname).HasColumnName("createdbyname");
            entity.Property(e => e.Createdbyyominame).HasColumnName("createdbyyominame");
            entity.Property(e => e.Createdon).HasColumnName("createdon");
            entity.Property(e => e.Createdonbehalfby).HasColumnName("createdonbehalfby");
            entity.Property(e => e.Createdonbehalfbyname).HasColumnName("createdonbehalfbyname");
            entity.Property(e => e.Createdonbehalfbyyominame).HasColumnName("createdonbehalfbyyominame");
            entity.Property(e => e.Defaultfilterspopulated).HasColumnName("defaultfilterspopulated");
            entity.Property(e => e.Defaultmailbox).HasColumnName("defaultmailbox");
            entity.Property(e => e.Defaultmailboxname).HasColumnName("defaultmailboxname");
            entity.Property(e => e.Defaultodbfoldername).HasColumnName("defaultodbfoldername");
            entity.Property(e => e.Deletedstate).HasColumnName("deletedstate");
            entity.Property(e => e.Disabledreason).HasColumnName("disabledreason");
            entity.Property(e => e.Displayinserviceviews).HasColumnName("displayinserviceviews");
            entity.Property(e => e.Domainname).HasColumnName("domainname");
            entity.Property(e => e.Emailrouteraccessapproval).HasColumnName("emailrouteraccessapproval");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.EntityimageTimestamp).HasColumnName("entityimage_timestamp");
            entity.Property(e => e.EntityimageUrl).HasColumnName("entityimage_url");
            entity.Property(e => e.Entityimageid).HasColumnName("entityimageid");
            entity.Property(e => e.Exchangerate)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("exchangerate");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Fullname).HasColumnName("fullname");
            entity.Property(e => e.Governmentid).HasColumnName("governmentid");
            entity.Property(e => e.Homephone).HasColumnName("homephone");
            entity.Property(e => e.Identityid).HasColumnName("identityid");
            entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");
            entity.Property(e => e.Incomingemaildeliverymethod).HasColumnName("incomingemaildeliverymethod");
            entity.Property(e => e.Internalemailaddress).HasColumnName("internalemailaddress");
            entity.Property(e => e.Invitestatuscode).HasColumnName("invitestatuscode");
            entity.Property(e => e.Isactivedirectoryuser).HasColumnName("isactivedirectoryuser");
            entity.Property(e => e.Isdisabled).HasColumnName("isdisabled");
            entity.Property(e => e.Isemailaddressapprovedbyo365admin)
                .HasColumnName("isemailaddressapprovedbyo365admin");
            entity.Property(e => e.Isintegrationuser).HasColumnName("isintegrationuser");
            entity.Property(e => e.Islicensed).HasColumnName("islicensed");
            entity.Property(e => e.Issyncwithdirectory).HasColumnName("issyncwithdirectory");
            entity.Property(e => e.Jobtitle).HasColumnName("jobtitle");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Latestupdatetime).HasColumnName("latestupdatetime");
            entity.Property(e => e.Middlename).HasColumnName("middlename");
            entity.Property(e => e.Mobilealertemail).HasColumnName("mobilealertemail");
            entity.Property(e => e.Mobileofflineprofileid).HasColumnName("mobileofflineprofileid");
            entity.Property(e => e.Mobileofflineprofileidname).HasColumnName("mobileofflineprofileidname");
            entity.Property(e => e.Mobilephone).HasColumnName("mobilephone");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiedbyname).HasColumnName("modifiedbyname");
            entity.Property(e => e.Modifiedbyyominame).HasColumnName("modifiedbyyominame");
            entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");
            entity.Property(e => e.Modifiedonbehalfby).HasColumnName("modifiedonbehalfby");
            entity.Property(e => e.Modifiedonbehalfbyname).HasColumnName("modifiedonbehalfbyname");
            entity.Property(e => e.Modifiedonbehalfbyyominame).HasColumnName("modifiedonbehalfbyyominame");
            entity.Property(e => e.MsdynAgentType).HasColumnName("msdyn_agentType");
            entity.Property(e => e.MsdynBotapplicationid).HasColumnName("msdyn_botapplicationid");
            entity.Property(e => e.MsdynBotdescription).HasColumnName("msdyn_botdescription");
            entity.Property(e => e.MsdynBotendpoint).HasColumnName("msdyn_botendpoint");
            entity.Property(e => e.MsdynBothandle).HasColumnName("msdyn_bothandle");
            entity.Property(e => e.MsdynBotprovider).HasColumnName("msdyn_botprovider");
            entity.Property(e => e.MsdynBotsecretkeys).HasColumnName("msdyn_botsecretkeys");
            entity.Property(e => e.MsdynCapacity).HasColumnName("msdyn_capacity");
            entity.Property(e => e.MsdynDefaultpresenceiduser).HasColumnName("msdyn_defaultpresenceiduser");
            entity.Property(e => e.MsdynDefaultpresenceidusername).HasColumnName("msdyn_defaultpresenceidusername");
            entity.Property(e => e.MsdynGdproptout).HasColumnName("msdyn_gdproptout");
            entity.Property(e => e.MsdynGridwrappercontrolfield).HasColumnName("msdyn_gridwrappercontrolfield");
            entity.Property(e => e.MsdynIsexpertenabledforswarm).HasColumnName("msdyn_isexpertenabledforswarm");
            entity.Property(e => e.MsdynOwningenvironmentid).HasColumnName("msdyn_owningenvironmentid");
            entity.Property(e => e.MsdynUsertype).HasColumnName("msdyn_usertype");
            entity.Property(e => e.Nickname).HasColumnName("nickname");
            entity.Property(e => e.Organizationid).HasColumnName("organizationid");
            entity.Property(e => e.Organizationidname).HasColumnName("organizationidname");
            entity.Property(e => e.Outgoingemaildeliverymethod).HasColumnName("outgoingemaildeliverymethod");
            entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");
            entity.Property(e => e.Parentsystemuserid).HasColumnName("parentsystemuserid");
            entity.Property(e => e.Parentsystemuseridname).HasColumnName("parentsystemuseridname");
            entity.Property(e => e.Parentsystemuseridyominame).HasColumnName("parentsystemuseridyominame");
            entity.Property(e => e.Passporthi).HasColumnName("passporthi");
            entity.Property(e => e.Passportlo).HasColumnName("passportlo");
            entity.Property(e => e.Personalemailaddress).HasColumnName("personalemailaddress");
            entity.Property(e => e.Photourl).HasColumnName("photourl");
            entity.Property(e => e.Positionid).HasColumnName("positionid");
            entity.Property(e => e.Positionidname).HasColumnName("positionidname");
            entity.Property(e => e.Preferredaddresscode).HasColumnName("preferredaddresscode");
            entity.Property(e => e.Preferredemailcode).HasColumnName("preferredemailcode");
            entity.Property(e => e.Preferredphonecode).HasColumnName("preferredphonecode");
            entity.Property(e => e.Processid).HasColumnName("processid");
            entity.Property(e => e.PtmPeruserlicensingdocumentscorepack)
                .HasColumnName("ptm_peruserlicensingdocumentscorepack");
            entity.Property(e => e.PtmPeruserlicensingdocumentscorepackserver)
                .HasColumnName("ptm_peruserlicensingdocumentscorepackserver");
            entity.Property(e => e.Queueid).HasColumnName("queueid");
            entity.Property(e => e.Queueidname).HasColumnName("queueidname");
            entity.Property(e => e.Salutation).HasColumnName("salutation");
            entity.Property(e => e.Setupuser).HasColumnName("setupuser");
            entity.Property(e => e.Sharepointemailaddress).HasColumnName("sharepointemailaddress");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Siteidname).HasColumnName("siteidname");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.Stageid).HasColumnName("stageid");
            entity.Property(e => e.Systemuserid).HasColumnName("systemuserid");
            entity.Property(e => e.Territoryid).HasColumnName("territoryid");
            entity.Property(e => e.Territoryidname).HasColumnName("territoryidname");
            entity.Property(e => e.Timezoneruleversionnumber).HasColumnName("timezoneruleversionnumber");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Transactioncurrencyid).HasColumnName("transactioncurrencyid");
            entity.Property(e => e.Transactioncurrencyidname).HasColumnName("transactioncurrencyidname");
            entity.Property(e => e.Traversedpath).HasColumnName("traversedpath");
            entity.Property(e => e.Userlicensetype).HasColumnName("userlicensetype");
            entity.Property(e => e.Userpuid).HasColumnName("userpuid");
            entity.Property(e => e.Utcconversiontimezonecode).HasColumnName("utcconversiontimezonecode");
            entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            entity.Property(e => e.Windowsliveid).HasColumnName("windowsliveid");
            entity.Property(e => e.Yammeremailaddress).HasColumnName("yammeremailaddress");
            entity.Property(e => e.Yammeruserid).HasColumnName("yammeruserid");
            entity.Property(e => e.Yomifirstname).HasColumnName("yomifirstname");
            entity.Property(e => e.Yomifullname).HasColumnName("yomifullname");
            entity.Property(e => e.Yomilastname).HasColumnName("yomilastname");
            entity.Property(e => e.Yomimiddlename).HasColumnName("yomimiddlename");
        });

        modelBuilder.Entity<MstrTrustGovernance>(entity =>
        {
            entity.HasKey(e => e.Sk);

            entity.ToTable("TrustGovernance", "mstr");

            entity.HasIndex(e => e.Gid, "IX_TrustGovernanceGID").IsUnique();

            entity.Property(e => e.Sk)
                .ValueGeneratedNever()
                .HasColumnName("SK");
            entity.Property(e => e.AppointingBody)
                .IsUnicode(false)
                .HasColumnName("Appointing body");
            entity.Property(e => e.DateOfAppointment)
                .IsUnicode(false)
                .HasColumnName("Date of appointment");
            entity.Property(e => e.DateTermOfOfficeEndsEnded)
                .IsUnicode(false)
                .HasColumnName("Date term of office ends/ended");
            entity.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.FkGovernanceRoleType).HasColumnName("FK_GovernanceRoleType");
            entity.Property(e => e.FkTrust).HasColumnName("FK_Trust");
            entity.Property(e => e.Forename1).IsUnicode(false);
            entity.Property(e => e.Forename2).IsUnicode(false);
            entity.Property(e => e.Gid)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("GID");
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasColumnName("Modified By");
            entity.Property(e => e.Surname).IsUnicode(false);
            entity.Property(e => e.Title).IsUnicode(false);
        });
    }
}

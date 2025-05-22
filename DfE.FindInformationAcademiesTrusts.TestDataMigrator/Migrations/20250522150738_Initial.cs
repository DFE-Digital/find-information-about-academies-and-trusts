using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mstr");

            migrationBuilder.EnsureSchema(
                name: "ops");

            migrationBuilder.EnsureSchema(
                name: "gias");

            migrationBuilder.EnsureSchema(
                name: "mis_mstr");

            migrationBuilder.EnsureSchema(
                name: "sharepoint");

            migrationBuilder.EnsureSchema(
                name: "tad");

            migrationBuilder.CreateTable(
                name: "AcademyConversions",
                schema: "mstr",
                columns: table => new
                {
                    SK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<string>(name: "Project ID", type: "nvarchar(max)", nullable: true),
                    ProjectName = table.Column<string>(name: "Project Name", type: "nvarchar(max)", nullable: true),
                    URN = table.Column<int>(type: "int", nullable: true),
                    StatutoryLowestAge = table.Column<int>(name: "Statutory Lowest Age", type: "int", nullable: true),
                    StatutoryHighestAge = table.Column<int>(name: "Statutory Highest Age", type: "int", nullable: true),
                    LocalAuthority = table.Column<string>(name: "Local Authority", type: "nvarchar(max)", nullable: true),
                    ProjectApplicationType = table.Column<string>(name: "Project Application Type", type: "nvarchar(max)", nullable: true),
                    ProjectStatus = table.Column<string>(name: "Project Status", type: "nvarchar(max)", nullable: true),
                    RouteOfProject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dAOProgress = table.Column<string>(name: "dAO Progress", type: "nvarchar(max)", nullable: true),
                    EstablishmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpectedOpeningDate = table.Column<DateTime>(name: "Expected Opening Date", type: "datetime2", nullable: true),
                    TrustID = table.Column<string>(name: "Trust ID", type: "nvarchar(max)", nullable: true),
                    LastDataRefresh = table.Column<DateTime>(name: "Last Data Refresh", type: "datetime2", nullable: true),
                    InPrepare = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InComplete = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademyConversions", x => x.SK);
                });

            migrationBuilder.CreateTable(
                name: "AcademyTransfers",
                schema: "mstr",
                columns: table => new
                {
                    SK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcademyName = table.Column<string>(name: "Academy Name", type: "nvarchar(max)", nullable: true),
                    AcademyURN = table.Column<int>(name: "Academy URN", type: "int", nullable: true),
                    AcademytransferStatus = table.Column<string>(name: "Academy transfer Status", type: "nvarchar(max)", nullable: false),
                    NewprovisionalTrustID = table.Column<string>(name: "New provisional Trust ID", type: "nvarchar(max)", nullable: true),
                    StatutoryLowestAge = table.Column<int>(name: "Statutory Lowest Age", type: "int", nullable: true),
                    StatutoryHighestAge = table.Column<int>(name: "Statutory Highest Age", type: "int", nullable: true),
                    LocalAuthority = table.Column<string>(name: "Local Authority", type: "nvarchar(max)", nullable: true),
                    ExpectedAcademytransferdate = table.Column<DateTime>(name: "Expected Academy transfer date", type: "datetime2", nullable: true),
                    InPrepare = table.Column<string>(name: "In Prepare", type: "nvarchar(max)", nullable: true),
                    InComplete = table.Column<string>(name: "In Complete", type: "nvarchar(max)", nullable: true),
                    LastDataRefresh = table.Column<DateTime>(name: "Last Data Refresh", type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademyTransfers", x => x.SK);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationEvent",
                schema: "ops",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Source = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    UserName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    EventType = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Level = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: true),
                    Severity = table.Column<string>(type: "varchar(1)", unicode: false, maxLength: 1, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Trace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessID = table.Column<int>(type: "int", nullable: true),
                    LineNumber = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ID", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationSettings",
                schema: "ops",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(name: "Created By", type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(name: "Modified By", type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSettings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Establishment",
                schema: "gias",
                columns: table => new
                {
                    URN = table.Column<int>(type: "int", nullable: false),
                    LAcode = table.Column<string>(name: "LA (code)", type: "varchar(max)", unicode: false, nullable: true),
                    LAname = table.Column<string>(name: "LA (name)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TypeOfEstablishmentcode = table.Column<string>(name: "TypeOfEstablishment (code)", type: "varchar(max)", unicode: false, nullable: true),
                    TypeOfEstablishmentname = table.Column<string>(name: "TypeOfEstablishment (name)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentTypeGroupcode = table.Column<string>(name: "EstablishmentTypeGroup (code)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentTypeGroupname = table.Column<string>(name: "EstablishmentTypeGroup (name)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentStatuscode = table.Column<string>(name: "EstablishmentStatus (code)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentStatusname = table.Column<string>(name: "EstablishmentStatus (name)", type: "varchar(max)", unicode: false, nullable: true),
                    ReasonEstablishmentOpenedcode = table.Column<string>(name: "ReasonEstablishmentOpened (code)", type: "varchar(max)", unicode: false, nullable: true),
                    ReasonEstablishmentOpenedname = table.Column<string>(name: "ReasonEstablishmentOpened (name)", type: "varchar(max)", unicode: false, nullable: true),
                    OpenDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ReasonEstablishmentClosedcode = table.Column<string>(name: "ReasonEstablishmentClosed (code)", type: "varchar(max)", unicode: false, nullable: true),
                    ReasonEstablishmentClosedname = table.Column<string>(name: "ReasonEstablishmentClosed (name)", type: "varchar(max)", unicode: false, nullable: true),
                    CloseDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    PhaseOfEducationcode = table.Column<string>(name: "PhaseOfEducation (code)", type: "varchar(max)", unicode: false, nullable: true),
                    PhaseOfEducationname = table.Column<string>(name: "PhaseOfEducation (name)", type: "varchar(max)", unicode: false, nullable: true),
                    StatutoryLowAge = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    StatutoryHighAge = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Boarderscode = table.Column<string>(name: "Boarders (code)", type: "varchar(max)", unicode: false, nullable: true),
                    Boardersname = table.Column<string>(name: "Boarders (name)", type: "varchar(max)", unicode: false, nullable: true),
                    NurseryProvisionname = table.Column<string>(name: "NurseryProvision (name)", type: "varchar(max)", unicode: false, nullable: true),
                    OfficialSixthFormcode = table.Column<string>(name: "OfficialSixthForm (code)", type: "varchar(max)", unicode: false, nullable: true),
                    OfficialSixthFormname = table.Column<string>(name: "OfficialSixthForm (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Gendercode = table.Column<string>(name: "Gender (code)", type: "varchar(max)", unicode: false, nullable: true),
                    Gendername = table.Column<string>(name: "Gender (name)", type: "varchar(max)", unicode: false, nullable: true),
                    ReligiousCharactercode = table.Column<string>(name: "ReligiousCharacter (code)", type: "varchar(max)", unicode: false, nullable: true),
                    ReligiousCharactername = table.Column<string>(name: "ReligiousCharacter (name)", type: "varchar(max)", unicode: false, nullable: true),
                    ReligiousEthosname = table.Column<string>(name: "ReligiousEthos (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Diocesecode = table.Column<string>(name: "Diocese (code)", type: "varchar(max)", unicode: false, nullable: true),
                    Diocesename = table.Column<string>(name: "Diocese (name)", type: "varchar(max)", unicode: false, nullable: true),
                    AdmissionsPolicycode = table.Column<string>(name: "AdmissionsPolicy (code)", type: "varchar(max)", unicode: false, nullable: true),
                    AdmissionsPolicyname = table.Column<string>(name: "AdmissionsPolicy (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SchoolCapacity = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SpecialClassescode = table.Column<string>(name: "SpecialClasses (code)", type: "varchar(max)", unicode: false, nullable: true),
                    SpecialClassesname = table.Column<string>(name: "SpecialClasses (name)", type: "varchar(max)", unicode: false, nullable: true),
                    CensusDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    NumberOfPupils = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    NumberOfBoys = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    NumberOfGirls = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    PercentageFSM = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TrustSchoolFlagcode = table.Column<string>(name: "TrustSchoolFlag (code)", type: "varchar(max)", unicode: false, nullable: true),
                    TrustSchoolFlagname = table.Column<string>(name: "TrustSchoolFlag (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Trustscode = table.Column<string>(name: "Trusts (code)", type: "varchar(max)", unicode: false, nullable: true),
                    Trustsname = table.Column<string>(name: "Trusts (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SchoolSponsorFlagname = table.Column<string>(name: "SchoolSponsorFlag (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SchoolSponsorsname = table.Column<string>(name: "SchoolSponsors (name)", type: "varchar(max)", unicode: false, nullable: true),
                    FederationFlagname = table.Column<string>(name: "FederationFlag (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Federationscode = table.Column<string>(name: "Federations (code)", type: "varchar(max)", unicode: false, nullable: true),
                    Federationsname = table.Column<string>(name: "Federations (name)", type: "varchar(max)", unicode: false, nullable: true),
                    UKPRN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FEHEIdentifier = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FurtherEducationTypename = table.Column<string>(name: "FurtherEducationType (name)", type: "varchar(max)", unicode: false, nullable: true),
                    OfstedLastInsp = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    OfstedSpecialMeasurescode = table.Column<string>(name: "OfstedSpecialMeasures (code)", type: "varchar(max)", unicode: false, nullable: true),
                    OfstedSpecialMeasuresname = table.Column<string>(name: "OfstedSpecialMeasures (name)", type: "varchar(max)", unicode: false, nullable: true),
                    LastChangedDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Street = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Locality = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Address3 = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Town = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Countyname = table.Column<string>(name: "County (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Postcode = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SchoolWebsite = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TelephoneNum = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    HeadTitlename = table.Column<string>(name: "HeadTitle (name)", type: "varchar(max)", unicode: false, nullable: true),
                    HeadFirstName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    HeadLastName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    HeadPreferredJobTitle = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    InspectorateNamename = table.Column<string>(name: "InspectorateName (name)", type: "varchar(max)", unicode: false, nullable: true),
                    InspectorateReport = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    DateOfLastInspectionVisit = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    NextInspectionVisit = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TeenMothname = table.Column<string>(name: "TeenMoth (name)", type: "varchar(max)", unicode: false, nullable: true),
                    TeenMothPlaces = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CCFname = table.Column<string>(name: "CCF (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SENPRUname = table.Column<string>(name: "SENPRU (name)", type: "varchar(max)", unicode: false, nullable: true),
                    EBDname = table.Column<string>(name: "EBD (name)", type: "varchar(max)", unicode: false, nullable: true),
                    PlacesPRU = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    FTProvname = table.Column<string>(name: "FTProv (name)", type: "varchar(max)", unicode: false, nullable: true),
                    EdByOthername = table.Column<string>(name: "EdByOther (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Section41Approvedname = table.Column<string>(name: "Section41Approved (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN1name = table.Column<string>(name: "SEN1 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN2name = table.Column<string>(name: "SEN2 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN3name = table.Column<string>(name: "SEN3 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN4name = table.Column<string>(name: "SEN4 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN5name = table.Column<string>(name: "SEN5 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN6name = table.Column<string>(name: "SEN6 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN7name = table.Column<string>(name: "SEN7 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN8name = table.Column<string>(name: "SEN8 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN9name = table.Column<string>(name: "SEN9 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN10name = table.Column<string>(name: "SEN10 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN11name = table.Column<string>(name: "SEN11 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN12name = table.Column<string>(name: "SEN12 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SEN13name = table.Column<string>(name: "SEN13 (name)", type: "varchar(max)", unicode: false, nullable: true),
                    TypeOfResourcedProvisionname = table.Column<string>(name: "TypeOfResourcedProvision (name)", type: "varchar(max)", unicode: false, nullable: true),
                    ResourcedProvisionOnRoll = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ResourcedProvisionCapacity = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SenUnitOnRoll = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SenUnitCapacity = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    GORcode = table.Column<string>(name: "GOR (code)", type: "varchar(max)", unicode: false, nullable: true),
                    GORname = table.Column<string>(name: "GOR (name)", type: "varchar(max)", unicode: false, nullable: true),
                    DistrictAdministrativecode = table.Column<string>(name: "DistrictAdministrative (code)", type: "varchar(max)", unicode: false, nullable: true),
                    DistrictAdministrativename = table.Column<string>(name: "DistrictAdministrative (name)", type: "varchar(max)", unicode: false, nullable: true),
                    AdministrativeWardcode = table.Column<string>(name: "AdministrativeWard (code)", type: "varchar(max)", unicode: false, nullable: true),
                    AdministrativeWardname = table.Column<string>(name: "AdministrativeWard (name)", type: "varchar(max)", unicode: false, nullable: true),
                    ParliamentaryConstituencycode = table.Column<string>(name: "ParliamentaryConstituency (code)", type: "varchar(max)", unicode: false, nullable: true),
                    ParliamentaryConstituencyname = table.Column<string>(name: "ParliamentaryConstituency (name)", type: "varchar(max)", unicode: false, nullable: true),
                    UrbanRuralcode = table.Column<string>(name: "UrbanRural (code)", type: "varchar(max)", unicode: false, nullable: true),
                    UrbanRuralname = table.Column<string>(name: "UrbanRural (name)", type: "varchar(max)", unicode: false, nullable: true),
                    GSSLACodename = table.Column<string>(name: "GSSLACode (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Easting = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Northing = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    MSOAname = table.Column<string>(name: "MSOA (name)", type: "varchar(max)", unicode: false, nullable: true),
                    LSOAname = table.Column<string>(name: "LSOA (name)", type: "varchar(max)", unicode: false, nullable: true),
                    SENStat = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SENNoStat = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    BoardingEstablishmentname = table.Column<string>(name: "BoardingEstablishment (name)", type: "varchar(max)", unicode: false, nullable: true),
                    PropsName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    PreviousLAcode = table.Column<string>(name: "PreviousLA (code)", type: "varchar(max)", unicode: false, nullable: true),
                    PreviousLAname = table.Column<string>(name: "PreviousLA (name)", type: "varchar(max)", unicode: false, nullable: true),
                    PreviousEstablishmentNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    OfstedRatingname = table.Column<string>(name: "OfstedRating (name)", type: "varchar(max)", unicode: false, nullable: true),
                    RSCRegionname = table.Column<string>(name: "RSCRegion (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Countryname = table.Column<string>(name: "Country (name)", type: "varchar(max)", unicode: false, nullable: true),
                    UPRN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    SiteName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    MSOAcode = table.Column<string>(name: "MSOA (code)", type: "varchar(max)", unicode: false, nullable: true),
                    LSOAcode = table.Column<string>(name: "LSOA (code)", type: "varchar(max)", unicode: false, nullable: true),
                    FSM = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    BSOInspectorateNamename = table.Column<string>(name: "BSOInspectorateName (name)", type: "varchar(max)", unicode: false, nullable: true),
                    CHNumber = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentAccreditedcode = table.Column<string>(name: "EstablishmentAccredited (code)", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentAccreditedname = table.Column<string>(name: "EstablishmentAccredited (name)", type: "varchar(max)", unicode: false, nullable: true),
                    QABNamecode = table.Column<string>(name: "QABName (code)", type: "varchar(max)", unicode: false, nullable: true),
                    QABNamename = table.Column<string>(name: "QABName (name)", type: "varchar(max)", unicode: false, nullable: true),
                    QABReport = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    AccreditationExpiryDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishment", x => x.URN);
                });

            migrationBuilder.CreateTable(
                name: "EstablishmentLink",
                schema: "gias",
                columns: table => new
                {
                    URN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LinkURN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LinkName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LinkType = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LinkEstablishedDate = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Establishments_FIAT",
                schema: "mis_mstr",
                columns: table => new
                {
                    urn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    web_link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    laestab = table.Column<int>(type: "int", nullable: true),
                    school_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ofsted_phase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type_of_education = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    school_open_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    admissions_policy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sixth_form = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    designated_religious_character = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    religious_ethos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    faith_grouping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ofsted_region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    local_authority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    parliamentary_constituency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    the_income_deprivation_affecting_children_index_idaci_quintile = table.Column<int>(type: "int", nullable: true),
                    total_number_of_pupils = table.Column<int>(type: "int", nullable: true),
                    statutory_lowest_age = table.Column<int>(type: "int", nullable: true),
                    statutory_highest_age = table.Column<int>(type: "int", nullable: true),
                    latest_section_8_inspection_number_since_last_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    does_the_section_8_inspection_relate_to_the_urn_of_the_current_school = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    urn_at_time_of_the_section_8_inspection = table.Column<int>(type: "int", nullable: true),
                    laestab_at_time_of_the_section_8_inspection = table.Column<int>(type: "int", nullable: true),
                    school_name_at_time_of_the_latest_section_8_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    school_type_at_time_of_the_latest_section_8_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_section_8_inspections_since_the_last_full_inspection = table.Column<int>(type: "int", nullable: true),
                    date_of_latest_section_8_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    section_8_inspection_publication_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    did_the_latest_section_8_inspection_convert_to_a_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    section_8_inspection_overall_outcome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_other_section_8_inspections_since_last_full_inspection = table.Column<int>(type: "int", nullable: true),
                    inspection_number_of_latest_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inspection_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inspection_type_grouping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    event_type_grouping = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inspection_start_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    publication_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    does_the_latest_full_inspection_relate_to_the_urn_of_the_current_school = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    urn_at_time_of_latest_full_inspection = table.Column<int>(type: "int", nullable: true),
                    laestab_at_time_of_latest_full_inspection = table.Column<int>(type: "int", nullable: true),
                    school_name_at_time_of_latest_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    school_type_at_time_of_latest_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    overall_effectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    category_of_concern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quality_of_education = table.Column<int>(type: "int", nullable: true),
                    behaviour_and_attitudes = table.Column<int>(type: "int", nullable: true),
                    personal_development = table.Column<int>(type: "int", nullable: true),
                    effectiveness_of_leadership_and_management = table.Column<int>(type: "int", nullable: true),
                    safeguarding_is_effective = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    early_years_provision_where_applicable = table.Column<int>(type: "int", nullable: true),
                    sixth_form_provision_where_applicable = table.Column<int>(type: "int", nullable: true),
                    previous_full_inspection_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_inspection_start_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_publication_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    does_the_previous_full_inspection_relate_to_the_urn_of_the_current_school = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    urn_at_time_of_previous_full_inspection = table.Column<int>(type: "int", nullable: true),
                    laestab_at_time_of_previous_full_inspection = table.Column<int>(type: "int", nullable: true),
                    school_name_at_time_of_previous_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    school_type_at_time_of_previous_full_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_full_inspection_overall_effectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_category_of_concern = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_quality_of_education = table.Column<int>(type: "int", nullable: true),
                    previous_behaviour_and_attitudes = table.Column<int>(type: "int", nullable: true),
                    previous_personal_development = table.Column<int>(type: "int", nullable: true),
                    previous_effectiveness_of_leadership_and_management = table.Column<int>(type: "int", nullable: true),
                    previous_safeguarding_is_effective = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_early_years_provision_where_applicable = table.Column<int>(type: "int", nullable: true),
                    previous_sixth_form_provision_where_applicable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_ingestion_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    meta_source_filename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Establishments_FIAT", x => x.urn);
                });

            migrationBuilder.CreateTable(
                name: "FreeSchoolProjects",
                schema: "mstr",
                columns: table => new
                {
                    SK = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectID = table.Column<string>(name: "Project ID", type: "nvarchar(max)", nullable: true),
                    ProjectName = table.Column<string>(name: "Project Name", type: "nvarchar(max)", nullable: true),
                    ProjectApplicationType = table.Column<string>(name: "Project Application Type", type: "nvarchar(max)", nullable: true),
                    LocalAuthority = table.Column<string>(name: "Local Authority", type: "nvarchar(max)", nullable: true),
                    Stage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RouteofProject = table.Column<string>(name: "Route of Project", type: "nvarchar(max)", nullable: false),
                    StatutoryLowestAge = table.Column<int>(name: "Statutory Lowest Age", type: "int", nullable: true),
                    StatutoryHighestAge = table.Column<int>(name: "Statutory Highest Age", type: "int", nullable: true),
                    NewURN = table.Column<int>(name: "New URN", type: "int", nullable: true),
                    EstablishmentName = table.Column<string>(name: "Establishment Name", type: "nvarchar(max)", nullable: true),
                    ProvisionalOpeningDate = table.Column<DateTime>(name: "Provisional Opening Date", type: "datetime2", nullable: true),
                    TrustID = table.Column<string>(name: "Trust ID", type: "nvarchar(max)", nullable: true),
                    LastDataRefresh = table.Column<DateTime>(name: "Last Data Refresh", type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreeSchoolProjects", x => x.SK);
                });

            migrationBuilder.CreateTable(
                name: "FurtherEducationEstablishments_FIAT",
                schema: "mis_mstr",
                columns: table => new
                {
                    provider_urn = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    provider_ukprn = table.Column<int>(type: "int", nullable: true),
                    provider_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    provider_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    provider_group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    local_authority = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ofsted_region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_of_latest_short_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    number_of_short_inspections_since_last_full_inspection = table.Column<int>(type: "int", nullable: true),
                    inspection_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    inspection_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    first_day_of_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    last_day_of_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_published = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    overall_effectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    quality_of_education = table.Column<int>(type: "int", nullable: true),
                    behaviour_and_attitudes = table.Column<int>(type: "int", nullable: true),
                    personal_development = table.Column<int>(type: "int", nullable: true),
                    effectiveness_of_leadership_and_management = table.Column<int>(type: "int", nullable: true),
                    is_safeguarding_effective = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_inspection_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_last_day_of_inspection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_overall_effectiveness = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    previous_quality_of_education = table.Column<int>(type: "int", nullable: true),
                    previous_behaviour_and_attitudes = table.Column<int>(type: "int", nullable: true),
                    previous_personal_development = table.Column<int>(type: "int", nullable: true),
                    previous_effectiveness_of_leadership_and_management = table.Column<int>(type: "int", nullable: true),
                    previous_safeguarding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    improved_declined_stayed_the_same = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    meta_ingestion_datetime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    meta_source_filename = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FurtherEducationEstablishments_FIAT", x => x.provider_urn);
                });

            migrationBuilder.CreateTable(
                name: "Governance",
                schema: "gias",
                columns: table => new
                {
                    GID = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    URN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    UID = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    CompaniesHouseNumber = table.Column<string>(name: "Companies House Number", type: "varchar(max)", unicode: false, nullable: true),
                    Role = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Title = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Forename1 = table.Column<string>(name: "Forename 1", type: "varchar(max)", unicode: false, nullable: true),
                    Forename2 = table.Column<string>(name: "Forename 2", type: "varchar(max)", unicode: false, nullable: true),
                    Surname = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Dateofappointment = table.Column<string>(name: "Date of appointment", type: "varchar(max)", unicode: false, nullable: true),
                    Datetermofofficeendsended = table.Column<string>(name: "Date term of office ends/ended", type: "varchar(max)", unicode: false, nullable: true),
                    Appointingbody = table.Column<string>(name: "Appointing body", type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Group",
                schema: "gias",
                columns: table => new
                {
                    GroupUID = table.Column<string>(name: "Group UID", type: "varchar(max)", unicode: false, nullable: true),
                    GroupID = table.Column<string>(name: "Group ID", type: "varchar(max)", unicode: false, nullable: true),
                    GroupName = table.Column<string>(name: "Group Name", type: "varchar(max)", unicode: false, nullable: true),
                    CompaniesHouseNumber = table.Column<string>(name: "Companies House Number", type: "varchar(max)", unicode: false, nullable: true),
                    GroupTypecode = table.Column<string>(name: "Group Type (code)", type: "varchar(max)", unicode: false, nullable: true),
                    GroupType = table.Column<string>(name: "Group Type", type: "varchar(max)", unicode: false, nullable: true),
                    ClosedDate = table.Column<string>(name: "Closed Date", type: "varchar(max)", unicode: false, nullable: true),
                    GroupStatuscode = table.Column<string>(name: "Group Status (code)", type: "varchar(max)", unicode: false, nullable: true),
                    GroupStatus = table.Column<string>(name: "Group Status", type: "varchar(max)", unicode: false, nullable: true),
                    GroupContactStreet = table.Column<string>(name: "Group Contact Street", type: "varchar(max)", unicode: false, nullable: true),
                    GroupContactLocality = table.Column<string>(name: "Group Contact Locality", type: "varchar(max)", unicode: false, nullable: true),
                    GroupContactTown = table.Column<string>(name: "Group Contact Town", type: "varchar(max)", unicode: false, nullable: true),
                    GroupContactPostcode = table.Column<string>(name: "Group Contact Postcode", type: "varchar(max)", unicode: false, nullable: true),
                    HeadofGroupTitle = table.Column<string>(name: "Head of Group Title", type: "varchar(max)", unicode: false, nullable: true),
                    HeadofGroupFirstName = table.Column<string>(name: "Head of Group First Name", type: "varchar(max)", unicode: false, nullable: true),
                    HeadofGroupLastName = table.Column<string>(name: "Head of Group Last Name", type: "varchar(max)", unicode: false, nullable: true),
                    UKPRN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Incorporatedonopendate = table.Column<string>(name: "Incorporated on (open date)", type: "varchar(max)", unicode: false, nullable: true),
                    Opendate = table.Column<string>(name: "Open date", type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "GroupLink",
                schema: "gias",
                columns: table => new
                {
                    URN = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    GroupUID = table.Column<string>(name: "Group UID", type: "varchar(max)", unicode: false, nullable: true),
                    GroupID = table.Column<string>(name: "Group ID", type: "varchar(max)", unicode: false, nullable: true),
                    GroupName = table.Column<string>(name: "Group Name", type: "varchar(max)", unicode: false, nullable: true),
                    CompaniesHouseNumber = table.Column<string>(name: "Companies House Number", type: "varchar(max)", unicode: false, nullable: true),
                    GroupTypecode = table.Column<string>(name: "Group Type (code)", type: "varchar(max)", unicode: false, nullable: true),
                    GroupType = table.Column<string>(name: "Group Type", type: "varchar(max)", unicode: false, nullable: true),
                    ClosedDate = table.Column<string>(name: "Closed Date", type: "varchar(max)", unicode: false, nullable: true),
                    GroupStatuscode = table.Column<string>(name: "Group Status (code)", type: "varchar(max)", unicode: false, nullable: true),
                    GroupStatus = table.Column<string>(name: "Group Status", type: "varchar(max)", unicode: false, nullable: true),
                    Joineddate = table.Column<string>(name: "Joined date", type: "varchar(max)", unicode: false, nullable: true),
                    EstablishmentName = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    TypeOfEstablishmentcode = table.Column<string>(name: "TypeOfEstablishment (code)", type: "varchar(max)", unicode: false, nullable: true),
                    TypeOfEstablishmentname = table.Column<string>(name: "TypeOfEstablishment (name)", type: "varchar(max)", unicode: false, nullable: true),
                    PhaseOfEducationcode = table.Column<string>(name: "PhaseOfEducation (code)", type: "varchar(max)", unicode: false, nullable: true),
                    PhaseOfEducationname = table.Column<string>(name: "PhaseOfEducation (name)", type: "varchar(max)", unicode: false, nullable: true),
                    LAcode = table.Column<string>(name: "LA (code)", type: "varchar(max)", unicode: false, nullable: true),
                    LAname = table.Column<string>(name: "LA (name)", type: "varchar(max)", unicode: false, nullable: true),
                    Incorporatedonopendate = table.Column<string>(name: "Incorporated on (open date)", type: "varchar(max)", unicode: false, nullable: true),
                    Opendate = table.Column<string>(name: "Open date", type: "varchar(max)", unicode: false, nullable: true),
                    URN_GroupUID = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Trust",
                schema: "mstr",
                columns: table => new
                {
                    GroupUID = table.Column<string>(name: "Group UID", type: "varchar(900)", unicode: false, nullable: false),
                    GORregion = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trust", x => x.GroupUID);
                });

            migrationBuilder.CreateTable(
                name: "TrustDocLinks",
                schema: "sharepoint",
                columns: table => new
                {
                    FolderPrefix = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    FolderYear = table.Column<string>(type: "varchar(4)", unicode: false, nullable: false),
                    DocumentFilename = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentLink = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DocumentIDValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentPath = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DataRefreshDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    CompaniesHouseNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TrustRefNumber = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrustDocLinks", x => new { x.FolderPrefix, x.FolderYear, x.DocumentFilename });
                });

            migrationBuilder.CreateTable(
                name: "TrustGovernance",
                schema: "tad",
                columns: table => new
                {
                    GID = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Email = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademyConversions",
                schema: "mstr");

            migrationBuilder.DropTable(
                name: "AcademyTransfers",
                schema: "mstr");

            migrationBuilder.DropTable(
                name: "ApplicationEvent",
                schema: "ops");

            migrationBuilder.DropTable(
                name: "ApplicationSettings",
                schema: "ops");

            migrationBuilder.DropTable(
                name: "Establishment",
                schema: "gias");

            migrationBuilder.DropTable(
                name: "EstablishmentLink",
                schema: "gias");

            migrationBuilder.DropTable(
                name: "Establishments_FIAT",
                schema: "mis_mstr");

            migrationBuilder.DropTable(
                name: "FreeSchoolProjects",
                schema: "mstr");

            migrationBuilder.DropTable(
                name: "FurtherEducationEstablishments_FIAT",
                schema: "mis_mstr");

            migrationBuilder.DropTable(
                name: "Governance",
                schema: "gias");

            migrationBuilder.DropTable(
                name: "Group",
                schema: "gias");

            migrationBuilder.DropTable(
                name: "GroupLink",
                schema: "gias");

            migrationBuilder.DropTable(
                name: "Trust",
                schema: "mstr");

            migrationBuilder.DropTable(
                name: "TrustDocLinks",
                schema: "sharepoint");

            migrationBuilder.DropTable(
                name: "TrustGovernance",
                schema: "tad");
        }
    }
}

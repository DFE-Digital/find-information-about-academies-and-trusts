namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;

public static class EstablishmentsQueries
{
    public static readonly string Insert = @"
                                                DELETE FROM [gias].[Establishment]
                                                
                                                INSERT INTO [gias].[Establishment]
                                                           ([URN]
                                                           ,[LA (code)]
                                                           ,[LA (name)]
                                                           ,[EstablishmentNumber]
                                                           ,[EstablishmentName]
                                                           ,[TypeOfEstablishment (code)]
                                                           ,[TypeOfEstablishment (name)]
                                                           ,[EstablishmentTypeGroup (code)]
                                                           ,[EstablishmentTypeGroup (name)]
                                                           ,[EstablishmentStatus (code)]
                                                           ,[EstablishmentStatus (name)]
                                                           ,[ReasonEstablishmentOpened (code)]
                                                           ,[ReasonEstablishmentOpened (name)]
                                                           ,[OpenDate]
                                                           ,[ReasonEstablishmentClosed (code)]
                                                           ,[ReasonEstablishmentClosed (name)]
                                                           ,[CloseDate]
                                                           ,[PhaseOfEducation (code)]
                                                           ,[PhaseOfEducation (name)]
                                                           ,[StatutoryLowAge]
                                                           ,[StatutoryHighAge]
                                                           ,[Boarders (code)]
                                                           ,[Boarders (name)]
                                                           ,[NurseryProvision (name)]
                                                           ,[OfficialSixthForm (code)]
                                                           ,[OfficialSixthForm (name)]
                                                           ,[Gender (code)]
                                                           ,[Gender (name)]
                                                           ,[ReligiousCharacter (code)]
                                                           ,[ReligiousCharacter (name)]
                                                           ,[ReligiousEthos (name)]
                                                           ,[Diocese (code)]
                                                           ,[Diocese (name)]
                                                           ,[AdmissionsPolicy (code)]
                                                           ,[AdmissionsPolicy (name)]
                                                           ,[SchoolCapacity]
                                                           ,[SpecialClasses (code)]
                                                           ,[SpecialClasses (name)]
                                                           ,[CensusDate]
                                                           ,[NumberOfPupils]
                                                           ,[NumberOfBoys]
                                                           ,[NumberOfGirls]
                                                           ,[PercentageFSM]
                                                           ,[TrustSchoolFlag (code)]
                                                           ,[TrustSchoolFlag (name)]
                                                           ,[Trusts (code)]
                                                           ,[Trusts (name)]
                                                           ,[SchoolSponsorFlag (name)]
                                                           ,[SchoolSponsors (name)]
                                                           ,[FederationFlag (name)]
                                                           ,[Federations (code)]
                                                           ,[Federations (name)]
                                                           ,[UKPRN]
                                                           ,[FEHEIdentifier]
                                                           ,[FurtherEducationType (name)]
                                                           ,[OfstedLastInsp]
                                                           ,[OfstedSpecialMeasures (code)]
                                                           ,[OfstedSpecialMeasures (name)]
                                                           ,[LastChangedDate]
                                                           ,[Street]
                                                           ,[Locality]
                                                           ,[Address3]
                                                           ,[Town]
                                                           ,[County (name)]
                                                           ,[Postcode]
                                                           ,[SchoolWebsite]
                                                           ,[TelephoneNum]
                                                           ,[HeadTitle (name)]
                                                           ,[HeadFirstName]
                                                           ,[HeadLastName]
                                                           ,[HeadPreferredJobTitle]
                                                           ,[InspectorateName (name)]
                                                           ,[InspectorateReport]
                                                           ,[DateOfLastInspectionVisit]
                                                           ,[NextInspectionVisit]
                                                           ,[TeenMoth (name)]
                                                           ,[TeenMothPlaces]
                                                           ,[CCF (name)]
                                                           ,[SENPRU (name)]
                                                           ,[EBD (name)]
                                                           ,[PlacesPRU]
                                                           ,[FTProv (name)]
                                                           ,[EdByOther (name)]
                                                           ,[Section41Approved (name)]
                                                           ,[SEN1 (name)]
                                                           ,[SEN2 (name)]
                                                           ,[SEN3 (name)]
                                                           ,[SEN4 (name)]
                                                           ,[SEN5 (name)]
                                                           ,[SEN6 (name)]
                                                           ,[SEN7 (name)]
                                                           ,[SEN8 (name)]
                                                           ,[SEN9 (name)]
                                                           ,[SEN10 (name)]
                                                           ,[SEN11 (name)]
                                                           ,[SEN12 (name)]
                                                           ,[SEN13 (name)]
                                                           ,[TypeOfResourcedProvision (name)]
                                                           ,[ResourcedProvisionOnRoll]
                                                           ,[ResourcedProvisionCapacity]
                                                           ,[SenUnitOnRoll]
                                                           ,[SenUnitCapacity]
                                                           ,[GOR (code)]
                                                           ,[GOR (name)]
                                                           ,[DistrictAdministrative (code)]
                                                           ,[DistrictAdministrative (name)]
                                                           ,[AdministrativeWard (code)]
                                                           ,[AdministrativeWard (name)]
                                                           ,[ParliamentaryConstituency (code)]
                                                           ,[ParliamentaryConstituency (name)]
                                                           ,[UrbanRural (code)]
                                                           ,[UrbanRural (name)]
                                                           ,[GSSLACode (name)]
                                                           ,[Easting]
                                                           ,[Northing]
                                                           ,[MSOA (name)]
                                                           ,[LSOA (name)]
                                                           ,[SENStat]
                                                           ,[SENNoStat]
                                                           ,[BoardingEstablishment (name)]
                                                           ,[PropsName]
                                                           ,[PreviousLA (code)]
                                                           ,[PreviousLA (name)]
                                                           ,[PreviousEstablishmentNumber]
                                                           ,[OfstedRating (name)]
                                                           ,[RSCRegion (name)]
                                                           ,[Country (name)]
                                                           ,[UPRN]
                                                           ,[SiteName]
                                                           ,[MSOA (code)]
                                                           ,[LSOA (code)]
                                                           ,[FSM]
                                                           ,[BSOInspectorateName (name)]
                                                           ,[CHNumber]
                                                           ,[EstablishmentAccredited (code)]
                                                           ,[EstablishmentAccredited (name)]
                                                           ,[QABName (code)]
                                                           ,[QABName (name)]
                                                           ,[QABReport]
                                                           ,[AccreditationExpiryDate])
                                                     VALUES
                                                           (@Urn
                                                           ,@LaCode
                                                           ,@LaName
                                                           ,@EstablishmentNumber
                                                           ,@EstablishmentName
                                                           ,@TypeOfEstablishmentCode
                                                           ,@TypeOfEstablishmentName
                                                           ,@EstablishmentTypeGroupCode
                                                           ,@EstablishmentTypeGroupName
                                                           ,@EstablishmentStatusCode
                                                           ,@EstablishmentStatusName
                                                           ,@ReasonEstablishmentOpenedCode
                                                           ,@ReasonEstablishmentOpenedName
                                                           ,@OpenDate
                                                           ,@ReasonEstablishmentClosedCode
                                                           ,@ReasonEstablishmentClosedName
                                                           ,@CloseDate
                                                           ,@PhaseOfEducationCode
                                                           ,@PhaseOfEducationName
                                                           ,@StatutoryLowAge
                                                           ,@StatutoryHighAge
                                                           ,@BoardersCode
                                                           ,@BoardersName
                                                           ,@NurseryProvisionName
                                                           ,@OfficialSixthFormCode
                                                           ,@OfficialSixthFormName
                                                           ,@GenderCode
                                                           ,@GenderName
                                                           ,@ReligiousCharacterCode
                                                           ,@ReligiousCharacterName
                                                           ,@ReligiousEthosName
                                                           ,@DioceseCode
                                                           ,@DioceseName
                                                           ,@AdmissionsPolicyCode
                                                           ,@AdmissionsPolicyName
                                                           ,@SchoolCapacity
                                                           ,@SpecialClassesCode
                                                           ,@SpecialClassesName
                                                           ,@CensusDate
                                                           ,@NumberOfPupils
                                                           ,@NumberOfBoys
                                                           ,@NumberOfGirls
                                                           ,@PercentageFsm
                                                           ,@TrustSchoolFlagCode
                                                           ,@TrustSchoolFlagName
                                                           ,@TrustsCode
                                                           ,@TrustsName
                                                           ,@SchoolSponsorFlagName
                                                           ,@SchoolSponsorsName
                                                           ,@FederationFlagName
                                                           ,@FederationsCode
                                                           ,@FederationsName
                                                           ,@Ukprn
                                                           ,@Feheidentifier
                                                           ,@FurtherEducationTypeName
                                                           ,@OfstedLastInsp
                                                           ,@OfstedSpecialMeasuresCode
                                                           ,@OfstedSpecialMeasuresName
                                                           ,@LastChangedDate
                                                           ,@Street
                                                           ,@Locality
                                                           ,@Address3
                                                           ,@Town
                                                           ,@CountyName
                                                           ,@Postcode
                                                           ,@SchoolWebsite
                                                           ,@TelephoneNum
                                                           ,@HeadTitleName
                                                           ,@HeadFirstName
                                                           ,@HeadLastName
                                                           ,@HeadPreferredJobTitle
                                                           ,@InspectorateNameName
                                                           ,@InspectorateReport
                                                           ,@DateOfLastInspectionVisit
                                                           ,@NextInspectionVisit
                                                           ,@TeenMothName
                                                           ,@TeenMothPlaces
                                                           ,@CcfName
                                                           ,@SenpruName
                                                           ,@EbdName
                                                           ,@PlacesPru
                                                           ,@FtprovName
                                                           ,@EdByOtherName
                                                           ,@Section41ApprovedName
                                                           ,@Sen1Name
                                                           ,@Sen2Name
                                                           ,@Sen3Name
                                                           ,@Sen4Name
                                                           ,@Sen5Name
                                                           ,@Sen6Name
                                                           ,@Sen7Name
                                                           ,@Sen8Name
                                                           ,@Sen9Name
                                                           ,@Sen10Name
                                                           ,@Sen11Name
                                                           ,@Sen12Name
                                                           ,@Sen13Name
                                                           ,@TypeOfResourcedProvisionName
                                                           ,@ResourcedProvisionOnRoll
                                                           ,@ResourcedProvisionCapacity
                                                           ,@SenUnitOnRoll
                                                           ,@SenUnitCapacity
                                                           ,@GorCode
                                                           ,@GorName
                                                           ,@DistrictAdministrativeCode
                                                           ,@DistrictAdministrativeName
                                                           ,@AdministrativeWardCode
                                                           ,@AdministrativeWardName
                                                           ,@ParliamentaryConstituencyCode
                                                           ,@ParliamentaryConstituencyName
                                                           ,@UrbanRuralCode
                                                           ,@UrbanRuralName
                                                           ,@GsslacodeName
                                                           ,@Easting
                                                           ,@Northing
                                                           ,@MsoaName
                                                           ,@LsoaName
                                                           ,@Senstat
                                                           ,@SennoStat
                                                           ,@BoardingEstablishmentName
                                                           ,@PropsName
                                                           ,@PreviousLaCode
                                                           ,@PreviousLaName
                                                           ,@PreviousEstablishmentNumber
                                                           ,@OfstedRatingName
                                                           ,@RscregionName
                                                           ,@CountryName
                                                           ,@Uprn
                                                           ,@SiteName
                                                           ,@MsoaCode
                                                           ,@LsoaCode
                                                           ,@Fsm
                                                           ,@BsoinspectorateNameName
                                                           ,@Chnumber
                                                           ,@EstablishmentAccreditedCode
                                                           ,@EstablishmentAccreditedName
                                                           ,@QabnameCode
                                                           ,@QabnameName
                                                           ,@Qabreport
                                                           ,@AccreditationExpiryDate)";
}

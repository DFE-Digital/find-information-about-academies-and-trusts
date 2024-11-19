using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Models.Cdm;

[ExcludeFromCodeCoverage] // Database model POCO
public class CdmAccount
{
    public int? Accountcategorycode { get; set; }

    public int? Accountclassificationcode { get; set; }

    public Guid? Accountid { get; set; }

    public string? Accountnumber { get; set; }

    public int? Accountratingcode { get; set; }

    public Guid? Address1Addressid { get; set; }

    public int? Address1Addresstypecode { get; set; }

    public string? Address1City { get; set; }

    public string? Address1Composite { get; set; }

    public string? Address1Country { get; set; }

    public string? Address1County { get; set; }

    public string? Address1Fax { get; set; }

    public int? Address1Freighttermscode { get; set; }

    public double? Address1Latitude { get; set; }

    public string? Address1Line1 { get; set; }

    public string? Address1Line2 { get; set; }

    public string? Address1Line3 { get; set; }

    public double? Address1Longitude { get; set; }

    public string? Address1Name { get; set; }

    public string? Address1Postalcode { get; set; }

    public string? Address1Postofficebox { get; set; }

    public string? Address1Primarycontactname { get; set; }

    public int? Address1Shippingmethodcode { get; set; }

    public string? Address1Stateorprovince { get; set; }

    public string? Address1Telephone1 { get; set; }

    public string? Address1Telephone2 { get; set; }

    public string? Address1Telephone3 { get; set; }

    public string? Address1Upszone { get; set; }

    public int? Address1Utcoffset { get; set; }

    public Guid? Address2Addressid { get; set; }

    public int? Address2Addresstypecode { get; set; }

    public string? Address2City { get; set; }

    public string? Address2Composite { get; set; }

    public string? Address2Country { get; set; }

    public string? Address2County { get; set; }

    public string? Address2Fax { get; set; }

    public int? Address2Freighttermscode { get; set; }

    public double? Address2Latitude { get; set; }

    public string? Address2Line1 { get; set; }

    public string? Address2Line2 { get; set; }

    public string? Address2Line3 { get; set; }

    public double? Address2Longitude { get; set; }

    public string? Address2Name { get; set; }

    public string? Address2Postalcode { get; set; }

    public string? Address2Postofficebox { get; set; }

    public string? Address2Primarycontactname { get; set; }

    public int? Address2Shippingmethodcode { get; set; }

    public string? Address2Stateorprovince { get; set; }

    public string? Address2Telephone1 { get; set; }

    public string? Address2Telephone2 { get; set; }

    public string? Address2Telephone3 { get; set; }

    public string? Address2Upszone { get; set; }

    public int? Address2Utcoffset { get; set; }

    public string? AdxCreatedbyipaddress { get; set; }

    public string? AdxCreatedbyusername { get; set; }

    public string? AdxModifiedbyipaddress { get; set; }

    public string? AdxModifiedbyusername { get; set; }

    public decimal? Aging30 { get; set; }

    public decimal? Aging30Base { get; set; }

    public decimal? Aging60 { get; set; }

    public decimal? Aging60Base { get; set; }

    public decimal? Aging90 { get; set; }

    public decimal? Aging90Base { get; set; }

    public int? Businesstypecode { get; set; }

    public Guid? Createdby { get; set; }

    public Guid? Createdbyexternalparty { get; set; }

    public string? Createdbyexternalpartyname { get; set; }

    public string? Createdbyexternalpartyyominame { get; set; }

    public string? Createdbyname { get; set; }

    public string? Createdbyyominame { get; set; }

    public DateTime? Createdon { get; set; }

    public Guid? Createdonbehalfby { get; set; }

    public string? Createdonbehalfbyname { get; set; }

    public string? Createdonbehalfbyyominame { get; set; }

    public decimal? Creditlimit { get; set; }

    public decimal? CreditlimitBase { get; set; }

    public bool? Creditonhold { get; set; }

    public int? Customersizecode { get; set; }

    public int? Customertypecode { get; set; }

    public Guid? Defaultpricelevelid { get; set; }

    public string? Defaultpricelevelidname { get; set; }

    public string? Description { get; set; }

    public bool? Donotbulkemail { get; set; }

    public bool? Donotbulkpostalmail { get; set; }

    public bool? Donotemail { get; set; }

    public bool? Donotfax { get; set; }

    public bool? Donotphone { get; set; }

    public bool? Donotpostalmail { get; set; }

    public bool? Donotsendmm { get; set; }

    public string? Emailaddress1 { get; set; }

    public string? Emailaddress2 { get; set; }

    public string? Emailaddress3 { get; set; }

    public long? EntityimageTimestamp { get; set; }

    public string? EntityimageUrl { get; set; }

    public Guid? Entityimageid { get; set; }

    public decimal? Exchangerate { get; set; }

    public string? Fax { get; set; }

    public bool? Followemail { get; set; }

    public string? Ftpsiteurl { get; set; }

    public int? Importsequencenumber { get; set; }

    public int? Industrycode { get; set; }

    public bool? Isprivate { get; set; }

    public DateTime? Lastonholdtime { get; set; }

    public DateTime? Lastusedincampaign { get; set; }

    public decimal? Marketcap { get; set; }

    public decimal? MarketcapBase { get; set; }

    public bool? Marketingonly { get; set; }

    public string? Masteraccountidname { get; set; }

    public string? Masteraccountidyominame { get; set; }

    public Guid? Masterid { get; set; }

    public bool? Merged { get; set; }

    public Guid? Modifiedby { get; set; }

    public Guid? Modifiedbyexternalparty { get; set; }

    public string? Modifiedbyexternalpartyname { get; set; }

    public string? Modifiedbyexternalpartyyominame { get; set; }

    public string? Modifiedbyname { get; set; }

    public string? Modifiedbyyominame { get; set; }

    public DateTime? Modifiedon { get; set; }

    public Guid? Modifiedonbehalfby { get; set; }

    public string? Modifiedonbehalfbyname { get; set; }

    public string? Modifiedonbehalfbyyominame { get; set; }

    public Guid? MsaManagingpartnerid { get; set; }

    public string? MsaManagingpartneridname { get; set; }

    public string? MsaManagingpartneridyominame { get; set; }

    public Guid? MsdynAccountkpiid { get; set; }

    public string? MsdynAccountkpiidname { get; set; }

    public bool? MsdynGdproptout { get; set; }

    public Guid? MsdynSalesaccelerationinsightid { get; set; }

    public string? MsdynSalesaccelerationinsightidname { get; set; }

    public Guid? MsdynSegmentid { get; set; }

    public string? MsdynSegmentidname { get; set; }

    public int? MsftDatastate { get; set; }

    public string? Name { get; set; }

    public int? Numberofemployees { get; set; }

    public int? Onholdtime { get; set; }

    public int? Opendeals { get; set; }

    public DateTime? OpendealsDate { get; set; }

    public int? OpendealsState { get; set; }

    public decimal? Openrevenue { get; set; }

    public decimal? OpenrevenueBase { get; set; }

    public DateTime? OpenrevenueDate { get; set; }

    public int? OpenrevenueState { get; set; }

    public Guid? Originatingleadid { get; set; }

    public string? Originatingleadidname { get; set; }

    public string? Originatingleadidyominame { get; set; }

    public DateTime? Overriddencreatedon { get; set; }

    public Guid? Ownerid { get; set; }

    public string? Owneridname { get; set; }

    public string? Owneridtype { get; set; }

    public string? Owneridyominame { get; set; }

    public int? Ownershipcode { get; set; }

    public Guid? Owningbusinessunit { get; set; }

    public string? Owningbusinessunitname { get; set; }

    public Guid? Owningteam { get; set; }

    public Guid? Owninguser { get; set; }

    public Guid? Parentaccountid { get; set; }

    public string? Parentaccountidname { get; set; }

    public string? Parentaccountidyominame { get; set; }

    public bool? Participatesinworkflow { get; set; }

    public int? Paymenttermscode { get; set; }

    public int? Preferredappointmentdaycode { get; set; }

    public int? Preferredappointmenttimecode { get; set; }

    public int? Preferredcontactmethodcode { get; set; }

    public Guid? Preferredequipmentid { get; set; }

    public string? Preferredequipmentidname { get; set; }

    public Guid? Preferredserviceid { get; set; }

    public string? Preferredserviceidname { get; set; }

    public Guid? Preferredsystemuserid { get; set; }

    public string? Preferredsystemuseridname { get; set; }

    public string? Preferredsystemuseridyominame { get; set; }

    public Guid? Primarycontactid { get; set; }

    public string? Primarycontactidname { get; set; }

    public string? Primarycontactidyominame { get; set; }

    public string? Primarysatoriid { get; set; }

    public string? Primarytwitterid { get; set; }

    public Guid? Processid { get; set; }

    public decimal? Revenue { get; set; }

    public decimal? RevenueBase { get; set; }

    public int? Sharesoutstanding { get; set; }

    public int? Shippingmethodcode { get; set; }

    public string? Sic { get; set; }

    public int? Sip9gridboxbanding { get; set; }

    public string? SipAcademyprojectlead { get; set; }

    public int? SipAcademyriskgrade { get; set; }

    public Guid? SipAcademywheretrustsat { get; set; }

    public string? SipAcademywheretrustsatname { get; set; }

    public string? SipAcademywheretrustsatyominame { get; set; }

    public string? SipAdministrativedistrict { get; set; }

    public string? SipAgerange { get; set; }

    public Guid? SipAmsdlead { get; set; }

    public string? SipAmsdleadname { get; set; }

    public string? SipAmsdleadyominame { get; set; }

    public Guid? SipAmsdterritoryid { get; set; }

    public string? SipAmsdterritoryidname { get; set; }

    public int? SipBehaviourandattitudes { get; set; }

    public string? SipCapacitybeyondopenandpipeline { get; set; }

    public int? SipClosurereasons { get; set; }

    public string? SipCompaddress { get; set; }

    public string? SipCompanieshousefilinghistory { get; set; }

    public string? SipCompanieshousenumber { get; set; }

    public string? SipCompanieshouseofficersurl { get; set; }

    public string? SipCompositeaddress { get; set; }

    public int? SipConcerntotaldart { get; set; }

    public int? SipConcerntotalkim { get; set; }

    public Guid? SipConstituencyid { get; set; }

    public string? SipConstituencyidname { get; set; }

    public int? SipCountofprimaryphase { get; set; }

    public int? SipCurrentsinglelistgrouping { get; set; }

    public DateTime? SipDateactionplannedfor { get; set; }

    public DateTime? SipDateclosed { get; set; }

    public DateTime? SipDateenteredontosinglelist { get; set; }

    public DateTime? SipDateofestablishment { get; set; }

    public DateTime? SipDateofgroupingdecision { get; set; }

    public DateTime? SipDateofjoiningtrust { get; set; }

    public DateTime? SipDateoflasttrustreview { get; set; }

    public DateTime? SipDateoflatestsection8inspection { get; set; }

    public DateTime? SipDateoftrustreviewmeeting { get; set; }

    public string? SipDfenumber { get; set; }

    public bool? SipDiocesanmat { get; set; }

    public decimal? SipDiocesanmatpercentagego { get; set; }

    public Guid? SipDioceseid { get; set; }

    public string? SipDioceseidname { get; set; }

    public bool? SipDisplayepprimarysections { get; set; }

    public bool? SipDisplayepsecondarysections { get; set; }

    public bool? SipDisplayeptrustprimary { get; set; }

    public bool? SipDisplayeptrustsecondary { get; set; }

    public string? SipDtrtoollink { get; set; }

    public int? SipEarlyyearsprovisionwhereapplicable { get; set; }

    public int? SipEffectivenessofleadershipandmanagement { get; set; }

    public string? SipEfficiencyicfpreviewcompleted { get; set; }

    public string? SipEfficiencyicfpreviewother { get; set; }

    public int? SipEstablishmentlinktype { get; set; }

    public int? SipEstablishmentnumber { get; set; }

    public Guid? SipEstablishmenttypeid { get; set; }

    public string? SipEstablishmenttypeidname { get; set; }

    public Guid? SipEstablismenttypegroupid { get; set; }

    public string? SipEstablismenttypegroupidname { get; set; }

    public DateTime? SipExpectedopeningdate { get; set; }

    public string? SipExternalgovernancereviewdate { get; set; }

    public string? SipFinanceriskgroup { get; set; }

    public decimal? SipFinanceriskgroupmajorrisks { get; set; }

    public decimal? SipFinanceriskgroupriskscore { get; set; }

    public decimal? SipFinanceriskgrouptotalrisks { get; set; }

    public string? SipFntistatus { get; set; }

    public int? SipFollowuplettersent { get; set; }

    public int? SipFormswitcher { get; set; }

    public decimal? SipFreeschoolmeals { get; set; }

    public string? SipFundingagreementpageurl { get; set; }

    public string? SipFutureplans { get; set; }

    public int? SipGorregion { get; set; }

    public string? SipGovernanceandtrustboard { get; set; }

    public string? SipGovernanceriskgroup { get; set; }

    public decimal? SipGovernanceriskgroupmajorrisks { get; set; }

    public decimal? SipGovernanceriskgroupriskscore { get; set; }

    public decimal? SipGovernanceriskgrouptotalrisks { get; set; }

    public int? SipHasthematsponsorreceivedragfmdifotherfund { get; set; }

    public string? SipHowmuchandwhatitwasfor { get; set; }

    public int? SipInadequaterequiresimprovementschoolscalc { get; set; }

    public DateTime? SipInadequaterequiresimprovementschoolscalcDate { get; set; }

    public int? SipInadequaterequiresimprovementschoolscalcState { get; set; }

    public DateTime? SipIncorporateddate { get; set; }

    public bool? SipInspectionforcurrenturn { get; set; }

    public string? SipIntegratedcurriculumfinancialplanningicfp { get; set; }

    public string? SipIpamaps { get; set; }

    public string? SipIrregularityriskgroup { get; set; }

    public decimal? SipIrregularityriskgroupmajorrisks { get; set; }

    public decimal? SipIrregularityriskgroupriskscore { get; set; }

    public decimal? SipIrregularityriskgrouptotalrisks { get; set; }

    public string? SipKeypeoplebios { get; set; }

    public Guid? SipKnowledgearticlefinanceglossary { get; set; }

    public string? SipKnowledgearticlefinanceglossaryname { get; set; }

    public int? SipLaestabnumber { get; set; }

    public string? SipLaestabtext { get; set; }

    public int? SipLatestmatbandingsource { get; set; }

    public int? SipLatestmatreviewscore { get; set; }

    public string? SipLatitude { get; set; }

    public bool? SipLbawarelandnotregistered { get; set; }

    public string? SipLbawarelandnotregisteredexplained { get; set; }

    public string? SipLbcurrentbuildinglandowner { get; set; }

    public Guid? SipLeadamsdterritoryid { get; set; }

    public string? SipLeadamsdterritoryidname { get; set; }

    public Guid? SipLeadrscareaid { get; set; }

    public string? SipLeadrscareaidname { get; set; }

    public string? SipLinktoofstedpage { get; set; }

    public string? SipLinktoworkplaceforefficiencyicfpreview { get; set; }

    public Guid? SipLocalauthorityareaid { get; set; }

    public string? SipLocalauthorityareaidname { get; set; }

    public int? SipLocalauthoritynumber { get; set; }

    public string? SipLongitude { get; set; }

    public string? SipMaincontactsemail { get; set; }

    public string? SipMaincontactsphone { get; set; }

    public string? SipMainreasonforintervention { get; set; }

    public Guid? SipMaintrustcontactid { get; set; }

    public string? SipMaintrustcontactidname { get; set; }

    public string? SipManagementletterinternalaudit { get; set; }

    public decimal? SipMathsscore { get; set; }

    public int? SipMathsscorecategory { get; set; }

    public bool? SipNationalsponsoroversight { get; set; }

    public DateTime? SipNewtrustdate { get; set; }

    public string? SipNewurn { get; set; }

    public string? SipNoeducationperformacedatamessage { get; set; }

    public string? SipNoeducationperformancedatatrustmessage { get; set; }

    public int? SipNoofphaseallthrough { get; set; }

    public DateTime? SipNoofphaseallthroughDate { get; set; }

    public int? SipNoofphaseallthroughState { get; set; }

    public int? SipNoofprimaryphase { get; set; }

    public DateTime? SipNoofprimaryphaseDate { get; set; }

    public int? SipNoofprimaryphaseState { get; set; }

    public int? SipNoofsecondaryphase { get; set; }

    public DateTime? SipNoofsecondaryphaseDate { get; set; }

    public int? SipNoofsecondaryphaseState { get; set; }

    public int? SipNumberacademiesintrust { get; set; }

    public int? SipNumberofacademiescalculated { get; set; }

    public DateTime? SipNumberofacademiescalculatedDate { get; set; }

    public int? SipNumberofacademiescalculatedState { get; set; }

    public int? SipNumberofacademiesintrust { get; set; }

    public int? SipNumberofactivedartconcerns { get; set; }

    public DateTime? SipNumberofactivedartconcernsDate { get; set; }

    public int? SipNumberofactivedartconcernsState { get; set; }

    public int? SipNumberofactivekimconcerns { get; set; }

    public DateTime? SipNumberofactivekimconcernsDate { get; set; }

    public int? SipNumberofactivekimconcernsState { get; set; }

    public int? SipNumberofdartconcernsacademy { get; set; }

    public DateTime? SipNumberofdartconcernsacademyDate { get; set; }

    public int? SipNumberofdartconcernsacademyState { get; set; }

    public int? SipNumberofkimconcernsacademy { get; set; }

    public DateTime? SipNumberofkimconcernsacademyDate { get; set; }

    public int? SipNumberofkimconcernsacademyState { get; set; }

    public int? SipNumberofoutstandingschoolscalculated { get; set; }

    public DateTime? SipNumberofoutstandingschoolscalculatedDate { get; set; }

    public int? SipNumberofoutstandingschoolscalculatedState { get; set; }

    public int? SipNumberofpupils { get; set; }

    public int? SipNumberofrequiresimprovementinadequateacad { get; set; }

    public DateTime? SipOfstedgoodschoolsupdateddate { get; set; }

    public DateTime? SipOfstedinspectiondate { get; set; }

    public int? SipOfstedinspectionrating { get; set; }

    public int? SipOfstedrating { get; set; }

    public int? SipOfstedratingupdated { get; set; }

    public int? SipOfstedtotalgood { get; set; }

    public int? SipOfstedtotalinadequat { get; set; }

    public int? SipOfstedtotalinadequate { get; set; }

    public int? SipOfstedtotaloutstanding { get; set; }

    public int? SipOfstedtotalrequiresimprovement { get; set; }

    public int? SipOrganisationtype { get; set; }

    public Guid? SipOriginatingapplicationid { get; set; }

    public string? SipOriginatingapplicationidname { get; set; }

    public Guid? SipOriginatingapplyingschoolid { get; set; }

    public string? SipOriginatingapplyingschoolidname { get; set; }

    public string? SipOverallriskgroup { get; set; }

    public decimal? SipOverallriskgroupmajorrisks { get; set; }

    public decimal? SipOverallriskgroupriskscore { get; set; }

    public decimal? SipOverallriskgrouptotalrisks { get; set; }

    public string? SipOverviewoffinancialstatusofthetrust { get; set; }

    public string? SipParentcompanyreference { get; set; }

    public string? SipPercentageoffreeschoolmeals { get; set; }

    public string? SipPerformanceguid { get; set; }

    public int? SipPersonaldevelopment { get; set; }

    public int? SipPhase { get; set; }

    public int? SipPipelineacademy { get; set; }

    public bool? SipPipelinefreeschool { get; set; }

    public Guid? SipPredecessorestablishment { get; set; }

    public string? SipPredecessorestablishmentname { get; set; }

    public string? SipPredecessorestablishmentyominame { get; set; }

    public string? SipPredecessorurn { get; set; }

    public double? SipPredictedchanceofchangeoccuring { get; set; }

    public string? SipPredictedchangeinprogress8score { get; set; }

    public bool? SipPreviousinspectionfor { get; set; }

    public DateTime? SipPreviousofstedinspectiondate { get; set; }

    public int? SipPreviousofstedinspectionrating { get; set; }

    public int? SipPreviousofstedrating { get; set; }

    public Guid? SipPrevioustrust { get; set; }

    public string? SipPrevioustrustname { get; set; }

    public string? SipPrevioustrustyominame { get; set; }

    public int? SipPrioritisedforareview { get; set; }

    public double? SipProbabilityofdeclining { get; set; }

    public double? SipProbabilityofimproving { get; set; }

    public double? SipProbabilityofstayingthesame { get; set; }

    public string? SipProjectleadforpipelineacademy { get; set; }

    public int? SipProjectroute { get; set; }

    public Guid? SipProposedmemberoftrustid { get; set; }

    public string? SipProposedmemberoftrustidname { get; set; }

    public string? SipProposedmemberoftrustidyominame { get; set; }

    public string? SipPupilpremiumstrategy { get; set; }

    public int? SipQualityofeducation { get; set; }

    public string? SipRatgradedescription { get; set; }

    public decimal? SipReadingscore { get; set; }

    public int? SipReadingscorecategory { get; set; }

    public Guid? SipRelationshipmanagerid { get; set; }

    public string? SipRelationshipmanageridname { get; set; }

    public string? SipRelationshipmanageridyominame { get; set; }

    public Guid? SipReligiouscharacterid { get; set; }

    public string? SipReligiouscharacteridname { get; set; }

    public Guid? SipReligiousethosid { get; set; }

    public string? SipReligiousethosidname { get; set; }

    public int? SipRiskratingnum { get; set; }

    public string? SipRisksissues { get; set; }

    public int? SipRouteofproject { get; set; }

    public DateTime? SipRschtbtrustapprovaldate { get; set; }

    public string? SipSatlaestabnumber { get; set; }

    public string? SipSatpredecessorurn { get; set; }

    public string? SipSaturn { get; set; }

    public int? SipSchoolcapacity { get; set; }

    public string? SipSchoolfinancialbenchmarklink { get; set; }

    public string? SipSchoolimprovementstrategy { get; set; }

    public string? SipScpnassumptions { get; set; }

    public int? SipScpnprojectedpupilonrollyear1 { get; set; }

    public int? SipScpnprojectedpupilonrollyear2 { get; set; }

    public int? SipScpnprojectedpupilonrollyear3 { get; set; }

    public bool? SipSection8inspectionfor { get; set; }

    public int? SipSection8inspectionoveralloutcome { get; set; }

    public int? SipSfsoterritories { get; set; }

    public string? SipSharepointfinancialglossary { get; set; }

    public string? SipSharepointsdocs { get; set; }

    public int? SipShortofstedinspcountsinceofstedinspdate { get; set; }

    public DateTime? SipShortofstedinspectiondate { get; set; }

    public int? SipSixthformprovisionwhereapplicable { get; set; }

    public string? SipSk { get; set; }

    public DateTime? SipSponsorapprovaldate { get; set; }

    public string? SipSponsorcoordinator { get; set; }

    public string? SipSponsorname { get; set; }

    public string? SipSponsoroverview { get; set; }

    public string? SipSponsorreferencenumber { get; set; }

    public int? SipSponsorrestrictions { get; set; }

    public int? SipSponsorstatus { get; set; }

    public int? SipStatutoryhighage { get; set; }

    public int? SipStatutorylowage { get; set; }

    public bool? SipStubtrust { get; set; }

    public string? SipTopsliceorchargingpolicy { get; set; }

    public int? SipTotalnumberofrisks { get; set; }

    public double? SipTotalriskscore { get; set; }

    public Guid? SipTransfer { get; set; }

    public string? SipTransfername { get; set; }

    public Guid? SipTransferstatus { get; set; }

    public string? SipTransferstatusname { get; set; }

    public Guid? SipTransfertype { get; set; }

    public string? SipTransfertypename { get; set; }

    public bool? SipTriggerflow { get; set; }

    public int? SipTrustconcernasnumber { get; set; }

    public int? SipTrusthighestconcernlevel { get; set; }

    public DateTime? SipTrustperformanceandriskdateofmeeting { get; set; }

    public string? SipTrustreferencenumber { get; set; }

    public Guid? SipTrustrelationshipmanager { get; set; }

    public string? SipTrustrelationshipmanagername { get; set; }

    public string? SipTrustrelationshipmanageryominame { get; set; }

    public string? SipTrustreviewwriteup { get; set; }

    public int? SipTrustriskgrade { get; set; }

    public string? SipUid { get; set; }

    public string? SipUkprn { get; set; }

    public string? SipUpin { get; set; }

    public string? SipUrn { get; set; }

    public string? SipUrnatcurrentofstedinspection { get; set; }

    public string? SipUrnatpreviousofstedinspection { get; set; }

    public string? SipUrnsection8ofstedinspection { get; set; }

    public int? SipUrnwholenumber { get; set; }

    public string? SipWipsummarygoestominister { get; set; }

    public decimal? SipWritingscore { get; set; }

    public int? SipWritingscorecategory { get; set; }

    public Guid? Slaid { get; set; }

    public Guid? Slainvokedid { get; set; }

    public string? Slainvokedidname { get; set; }

    public string? Slaname { get; set; }

    public Guid? Stageid { get; set; }

    public int? Statecode { get; set; }

    public int? Statuscode { get; set; }

    public string? Stockexchange { get; set; }

    public int? Teamsfollowed { get; set; }

    public string? Telephone1 { get; set; }

    public string? Telephone2 { get; set; }

    public string? Telephone3 { get; set; }

    public int? Territorycode { get; set; }

    public Guid? Territoryid { get; set; }

    public string? Territoryidname { get; set; }

    public string? Tickersymbol { get; set; }

    public string? Timespentbymeonemailandmeetings { get; set; }

    public int? Timezoneruleversionnumber { get; set; }

    public Guid? Transactioncurrencyid { get; set; }

    public string? Transactioncurrencyidname { get; set; }

    public string? Traversedpath { get; set; }

    public int? Utcconversiontimezonecode { get; set; }

    public long? Versionnumber { get; set; }

    public string? Websiteurl { get; set; }

    public string? Yominame { get; set; }

    public Guid Id { get; set; }
}

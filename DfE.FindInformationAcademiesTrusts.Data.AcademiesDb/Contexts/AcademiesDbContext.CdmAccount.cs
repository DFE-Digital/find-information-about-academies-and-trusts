using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<CdmAccount> CdmAccounts { get; set; }

    [ExcludeFromCodeCoverage]
    protected void OnModelCreatingCdmAccounts(ModelBuilder modelBuilder)
    {
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
    }
}
